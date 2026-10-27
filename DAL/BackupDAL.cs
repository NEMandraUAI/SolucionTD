using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace DAL
{
    public class BackupDAL
    {
        public string? UltimoBackupPrevio { get; private set; }
        // Punto de extensión sin comportamiento por defecto. Permite verificar
        // recuperación ante una interrupción del cliente en pruebas aisladas.
        protected virtual void AntesDeRestaurar(SqlConnection connection) { }
        private static string Identificador(string value) => "[" + value.Replace("]", "]]") + "]";
        private static string Literal(string value) => "N'" + value.Replace("'", "''") + "'";

        private static string NombreBase(string connection)
        {
            var name=new SqlConnectionStringBuilder(connection).InitialCatalog;
            if(string.IsNullOrWhiteSpace(name) || new[]{"master","model","msdb","tempdb"}.Contains(name,StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException("La operación requiere una base de UAI-FIT válida.");
            return name;
        }

        private static (bool LocalDb,string Servicio) VerificarServidorLocal(SqlConnection connection)
        {
            using var cmd=new SqlCommand("SELECT CONVERT(nvarchar(128),SERVERPROPERTY('MachineName')), CONVERT(nvarchar(128),SERVERPROPERTY('InstanceName'));",connection);
            using var reader=cmd.ExecuteReader();
            reader.Read();
            string machine=reader.GetString(0);
            if(!string.Equals(machine,Environment.MachineName,StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException("El respaldo/restauración por archivo local solo está disponible para SQL Server en este equipo. En un servidor remoto debe realizarlo su administrador con una ruta del servidor.");
            bool localDb=new SqlConnectionStringBuilder(connection.ConnectionString).DataSource.IndexOf("(localdb)",StringComparison.OrdinalIgnoreCase)>=0;
            string instance=reader.IsDBNull(1)?string.Empty:reader.GetString(1);
            return (localDb,string.IsNullOrEmpty(instance)?"MSSQLSERVER":"MSSQL$"+instance);
        }

        private static string PrepararStaging(SqlConnection connection)
        {
            var entorno=VerificarServidorLocal(connection);
            string root=Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"UAI-FIT","BackupStaging");
            string dir=Path.Combine(root,Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            var acl=new DirectorySecurity();
            acl.SetAccessRuleProtection(true,false);
            var inherit=InheritanceFlags.ContainerInherit|InheritanceFlags.ObjectInherit;
            void Grant(SecurityIdentifier sid,FileSystemRights rights) => acl.AddAccessRule(new FileSystemAccessRule(sid,rights,inherit,PropagationFlags.None,AccessControlType.Allow));
            using var identity=WindowsIdentity.GetCurrent();
            Grant(identity.User ?? throw new InvalidOperationException("No se pudo resolver la identidad de Windows."),FileSystemRights.FullControl);
            Grant(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid,null),FileSystemRights.FullControl);
            Grant(new SecurityIdentifier(WellKnownSidType.LocalSystemSid,null),FileSystemRights.FullControl);
            if(!entorno.LocalDb)
            {
                using var key=Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\"+entorno.Servicio);
                string account=Convert.ToString(key?.GetValue("ObjectName")) ?? throw new InvalidOperationException("No se pudo identificar la cuenta del servicio SQL Server para autorizar el staging privado.");
                if(account.StartsWith(@".\",StringComparison.Ordinal)) account=Environment.MachineName+account[1..];
                var sid=account.Equals("LocalSystem",StringComparison.OrdinalIgnoreCase)
                    ?new SecurityIdentifier(WellKnownSidType.LocalSystemSid,null)
                    :(SecurityIdentifier)new NTAccount(account).Translate(typeof(SecurityIdentifier));
                Grant(sid,FileSystemRights.Modify);
            }
            new DirectoryInfo(dir).SetAccessControl(acl);
            return dir;
        }

        private static void LimpiarStaging(string dir)
        {
            string root=Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"UAI-FIT","BackupStaging"));
            string target=Path.GetFullPath(dir);
            if(!target.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase) || !Guid.TryParseExact(Path.GetFileName(target),"N",out _))
                throw new InvalidOperationException("Se rechazó limpiar una ruta ajena al staging de UAI-FIT.");
            try { if(Directory.Exists(target)) Directory.Delete(target,true); }
            catch(Exception ex) { System.Diagnostics.Trace.TraceWarning("No se pudo limpiar el staging privado de backup: {0}",ex.Message); }
        }

        private static void HeredarPermisosPrivados(string archivo)
        {
            var file=new FileInfo(archivo);
            var acl=file.GetAccessControl();
            foreach(FileSystemAccessRule rule in acl.GetAccessRules(true,false,typeof(SecurityIdentifier)))
                acl.RemoveAccessRuleSpecific(rule);
            acl.SetAccessRuleProtection(false,false);
            file.SetAccessControl(acl);
        }

        private static void Ejecutar(SqlConnection cn,string sql,string? ruta=null,int timeout=180)
        {
            using var cmd=new SqlCommand(sql,cn) { CommandTimeout=timeout };
            if(ruta!=null)cmd.Parameters.AddWithValue("@ruta",ruta);
            cmd.ExecuteNonQuery();
        }

        public void RealizarBackup(string rutaArchivo)
        {
            if(string.IsNullOrWhiteSpace(rutaArchivo)) throw new ArgumentException("Debe indicar el archivo de respaldo.");
            string destino=Path.GetFullPath(rutaArchivo);
            string directorio=Path.GetDirectoryName(destino) ?? throw new ArgumentException("La ruta de destino no es válida.");
            if(!Directory.Exists(directorio))throw new DirectoryNotFoundException("La carpeta de destino del respaldo no existe.");
            using var cn=ConexionDAL.Instancia.ObtenerConexion();
            string db=NombreBase(cn.ConnectionString);
            cn.Open();
            string staging=PrepararStaging(cn);
            string temporal=Path.Combine(staging,"respaldo.bak");
            try
            {
                Ejecutar(cn,$"BACKUP DATABASE {Identificador(db)} TO DISK=@ruta WITH COPY_ONLY,CHECKSUM,NAME=N'UAI-FIT;Schema=2';",temporal);
                Ejecutar(cn,"RESTORE VERIFYONLY FROM DISK=@ruta WITH CHECKSUM;",temporal);
                // Copia completa antes de reemplazar: un disco lleno no debe
                // destruir el último respaldo que el usuario ya conservaba.
                string copia=Path.Combine(directorio,".UAIFIT_Backup_"+Guid.NewGuid().ToString("N")+".tmp");
                try
                {
                    File.Copy(temporal,copia,overwrite:false);
                    if(File.Exists(destino))File.Replace(copia,destino,null);
                    else File.Move(copia,destino);
                }
                finally
                {
                    try{if(File.Exists(copia))File.Delete(copia);}
                    catch(Exception ex){System.Diagnostics.Trace.TraceWarning("No se pudo limpiar una copia temporal de backup: {0}",ex.Message);}
                }
            }
            finally { LimpiarStaging(staging); }
        }

        public void ValidarBackup(string rutaArchivo)
        {
            using var app=ConexionDAL.Instancia.ObtenerConexion();
            string target=NombreBase(app.ConnectionString);
            app.Open();
            string staging=PrepararStaging(app);
            try
            {
                string staged=Path.Combine(staging,"validar.bak");
                File.Copy(rutaArchivo,staged,overwrite:false);
                HeredarPermisosPrivados(staged);
                Prevalidar(app,staged,target);
            }
            finally { LimpiarStaging(staging); }
        }

        private static DataTable Consultar(SqlConnection cn,string sql,string? ruta=null)
        {
            using var cmd=new SqlCommand(sql,cn) { CommandTimeout=180 };
            if(ruta!=null)cmd.Parameters.AddWithValue("@ruta",ruta);
            using var reader=cmd.ExecuteReader();
            var table=new DataTable();table.Load(reader);return table;
        }

        private static void Prevalidar(SqlConnection cn,string path,string target)
        {
            var headers=Consultar(cn,"RESTORE HEADERONLY FROM DISK=@ruta;",path);
            if(headers.Rows.Count!=1) throw new InvalidOperationException("Seleccione un archivo con un único backup completo de UAI-FIT.");
            var row=headers.Rows[0];
            string name=Convert.ToString(row["DatabaseName"]) ?? string.Empty;
            if(Convert.ToInt32(row["BackupType"])!=1 || (name!="ProyectoCampo" && !name.Equals(target,StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("El archivo no es un backup completo de la base UAI-FIT seleccionada.");
            int sourceMajor=Convert.ToInt32(row["SoftwareVersionMajor"]);
            using var engine=new SqlCommand("SELECT CONVERT(int,SERVERPROPERTY('ProductMajorVersion'));",cn);
            if(sourceMajor>Convert.ToInt32(engine.ExecuteScalar()))
                throw new InvalidOperationException("El backup fue creado con una versión de SQL Server más nueva y no puede restaurarse en este motor.");
            string label=Convert.ToString(row["BackupName"]) ?? string.Empty;
            int marker=label.IndexOf("UAI-FIT;Schema=",StringComparison.Ordinal);
            if(marker>=0 && (!int.TryParse(label[(marker+15)..],out int version) || version>2 || version<1))
                throw new InvalidOperationException("El backup declara una versión de esquema incompatible con esta aplicación.");
            if(row.Table.Columns.Contains("HasBackupChecksums") && !Convert.ToBoolean(row["HasBackupChecksums"]))
                throw new InvalidOperationException("El respaldo no contiene checksum verificable. Genere un respaldo nuevo con UAI-FIT o solicite una recuperación técnica supervisada.");
            Ejecutar(cn,"RESTORE VERIFYONLY FROM DISK=@ruta WITH CHECKSUM;",path);
        }

        public void RestaurarBackup(string rutaArchivo)
        {
            if(!File.Exists(rutaArchivo))throw new FileNotFoundException("No se encontró el respaldo seleccionado.",rutaArchivo);
            string appConnection=ConexionDAL.ObtenerConnectionStringConfigurada();
            string target=NombreBase(appConnection);
            var builder=new SqlConnectionStringBuilder(appConnection){InitialCatalog="master"};
            using var cn=new SqlConnection(builder.ConnectionString);
            cn.Open();
            string staging=PrepararStaging(cn);
            try
            {
                string staged=Path.Combine(staging,"restaurar.bak");
                File.Copy(rutaArchivo,staged,overwrite:false);
                HeredarPermisosPrivados(staged);
                Prevalidar(cn,staged,target);
                var backupFiles=Consultar(cn,"RESTORE FILELISTONLY FROM DISK=@ruta;",staged);
                using var filesCmd=new SqlCommand("SELECT name,physical_name,type FROM sys.master_files WHERE database_id=DB_ID(@db) ORDER BY file_id;",cn);
                filesCmd.Parameters.AddWithValue("@db",target);
                var currentFiles=new DataTable();using(var reader=filesCmd.ExecuteReader())currentFiles.Load(reader);
                if(currentFiles.Rows.Count==0)throw new InvalidOperationException("La base destino no existe. Inicialícela antes de restaurar.");
                using (var identidad=new SqlCommand($"SELECT COUNT(*) FROM {Identificador(target)}.sys.tables WHERE schema_id=SCHEMA_ID(N'dbo') AND name IN(N'Usuario',N'Permiso',N'DigitoVerificador',N'Socio',N'ComprobantePago');",cn))
                    if(Convert.ToInt32(identidad.ExecuteScalar())!=5)
                        throw new InvalidOperationException("La base configurada no se identifica como UAI-FIT. Se rechazó reemplazar una base ajena o incompleta.");
                var moves=new List<string>();
                int dataIndex=0,logIndex=0;
                foreach(DataRow file in backupFiles.Rows)
                {
                    string type=Convert.ToString(file["Type"])!;
                    if(type!="D" && type!="L")throw new InvalidOperationException("El backup contiene archivos especiales no admitidos por UAI-FIT.");
                    int index=type=="D"?dataIndex++:logIndex++;
                    var targets=currentFiles.Rows.Cast<DataRow>().Where(r=>Convert.ToInt32(r["type"])==(type=="D"?0:1)).ToList();
                    string physical;
                    if(index<targets.Count)physical=Convert.ToString(targets[index]["physical_name"])!;
                    else
                    {
                        if(targets.Count==0)throw new InvalidOperationException("No se pudo resolver un directorio SQL seguro para los archivos restaurados.");
                        physical=Path.Combine(Path.GetDirectoryName(Convert.ToString(targets[0]["physical_name"]))!,"UAIFIT_Restore_"+Guid.NewGuid().ToString("N")+(type=="L"?".ldf":".ndf"));
                    }
                    moves.Add("MOVE "+Literal(Convert.ToString(file["LogicalName"])!)+" TO "+Literal(physical));
                }
                using var defaultPath=new SqlCommand("SELECT CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultBackupPath'));",cn);
                string? backupDir=Convert.ToString(defaultPath.ExecuteScalar());
                if(string.IsNullOrWhiteSpace(backupDir)) backupDir=Path.GetDirectoryName(Convert.ToString(currentFiles.Rows[0]["physical_name"]));
                if(string.IsNullOrWhiteSpace(backupDir)) throw new InvalidOperationException("No se pudo resolver una ubicación privada para el backup previo a restaurar.");
                UltimoBackupPrevio=Path.Combine(backupDir,"UAIFIT_AntesRestore_"+Guid.NewGuid().ToString("N")+".bak");
                Ejecutar(cn,$"BACKUP DATABASE {Identificador(target)} TO DISK=@ruta WITH COPY_ONLY,CHECKSUM,NAME=N'UAI-FIT;Schema=2';",UltimoBackupPrevio);
                Ejecutar(cn,"RESTORE VERIFYONLY FROM DISK=@ruta WITH CHECKSUM;",UltimoBackupPrevio);
                System.Diagnostics.Trace.TraceInformation("Backup previo a restaurar: {0}",UltimoBackupPrevio);
                SqlConnection.ClearAllPools();
                Exception? fallo=null;
                try
                {
                    Ejecutar(cn,$"ALTER DATABASE {Identificador(target)} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;",timeout:60);
                    AntesDeRestaurar(cn);
                    Ejecutar(cn,$"RESTORE DATABASE {Identificador(target)} FROM DISK=@ruta WITH FILE=1,REPLACE,CHECKSUM,RECOVERY,"+string.Join(",",moves)+";",staged,300);
                }
                catch(Exception ex){fallo=ex;}
                finally
                {
                    try{Ejecutar(cn,$"ALTER DATABASE {Identificador(target)} SET MULTI_USER;",timeout:60);}
                    catch(Exception multi)
                    {
                        throw new AggregateException("No se pudo devolver la base a MULTI_USER. Requiere intervención administrativa. Backup previo: "+UltimoBackupPrevio,fallo==null?new[]{multi}:new[]{fallo,multi});
                    }
                }
                if(fallo!=null)throw new InvalidOperationException("La restauración falló. Se conservó el backup previo: "+UltimoBackupPrevio,fallo);
            }
            finally{LimpiarStaging(staging);}
        }
    }
}
