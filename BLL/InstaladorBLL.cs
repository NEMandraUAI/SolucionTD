using DAL;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace BLL
{
    public enum EstadoServicioSQL { NoExiste, EnEjecucion, Detenido, Desconocido }

    public static class InstaladorBLL
    {
        private const string BasePredeterminada = "ProyectoCampo";
        private const string ConexionPredeterminada = @"Server=(localdb)\UAIFIT;Database=master;Integrated Security=True;TrustServerCertificate=True;";
        public static string RutaArchivoLog => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UAI-FIT", "Logs", "startup_install.log");

        public static void RegistrarLog(string mensaje)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(RutaArchivoLog)!);
                File.AppendAllText(RutaArchivoLog, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] {mensaje}{Environment.NewLine}", Encoding.UTF8);
            }
            catch (Exception ex) { Trace.TraceError("No se pudo registrar diagnóstico UAI-FIT: {0}", ex.Message); }
        }

        private static string NombreBase(string catalogo)
        {
            string nombre = string.IsNullOrWhiteSpace(catalogo) || catalogo.Equals("master", StringComparison.OrdinalIgnoreCase) ? BasePredeterminada : catalogo;
            if (!Regex.IsMatch(nombre, @"^[A-Za-z0-9_]{1,128}$") || new[] { "master", "model", "msdb", "tempdb" }.Contains(nombre, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException("El nombre de la base de UAI-FIT no es válido.");
            return nombre;
        }

        public static void InicializarBaseDeDatos(string serverConnectionString, bool persistirConfiguracion = true)
        {
            var solicitada = new SqlConnectionStringBuilder(string.IsNullOrWhiteSpace(serverConnectionString) ? ConexionPredeterminada : serverConnectionString);
            string nombre = NombreBase(solicitada.InitialCatalog);
            string master = ResolverYValidarConexionServidorSQL(solicitada.ConnectionString, false);
            var app = new SqlConnectionStringBuilder(master) { InitialCatalog = nombre, TrustServerCertificate = true, ConnectTimeout = 8 };
            bool nueva;
            using (var cn = new SqlConnection(master))
            {
                cn.Open();
                using var version = new SqlCommand("SELECT CONVERT(int,SERVERPROPERTY('ProductMajorVersion'));", cn);
                if (Convert.ToInt32(version.ExecuteScalar()) < 12)
                    throw new InvalidOperationException("UAI-FIT requiere SQL Server 2014 (v12) o posterior.");
                using var existe = new SqlCommand("SELECT DB_ID(@nombre);", cn);
                existe.Parameters.AddWithValue("@nombre", nombre);
                nueva = existe.ExecuteScalar() is DBNull;
                if (nueva)
                {
                    nueva = CrearOAdjuntarBaseDeDatos(cn, app.DataSource, nombre);
                }
            }
            string? anterior = ConexionDAL.ConnectionStringResuelta;
            try
            {
                using (var cn = new SqlConnection(app.ConnectionString))
                {
                    cn.Open();
                    using var identificar = new SqlCommand(@"SELECT (SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped=0),
                        (SELECT COUNT(*) FROM sys.tables WHERE name IN(N'Usuario',N'Socio',N'Permiso') AND schema_id=SCHEMA_ID(N'dbo'));", cn);
                    using var r = identificar.ExecuteReader(); r.Read();
                    if (r.GetInt32(0) == 0) nueva = true;
                    else if (r.GetInt32(1) < 2) throw new InvalidOperationException("La base seleccionada no se reconoce como una base UAI-FIT. No se modificarán sus tablas.");
                }
                bool actualizar = nueva || SchemaMigradorBLL.RequiereActualizacion(app.ConnectionString);
                if (nueva)
                {
                    using var unidad = new UnidadTrabajoSeguridadDAL(app.ConnectionString);
                    var integridad = new IntegridadBLL();
                    using var contar = new SqlCommand("SELECT COUNT(*) FROM sys.tables WHERE is_ms_shipped=0;", unidad.Conexion, unidad.Transaccion);
                    if (Convert.ToInt32(contar.ExecuteScalar()) == 0)
                    {
                        EjecutarEsquemaInicial(unidad);
                        SchemaMigradorBLL.Aplicar(unidad, true);
                        integridad.ForzarRecalculoDeTodaLaBase(unidad);
                    }
                    else
                    {
                        SchemaMigradorBLL.Aplicar(unidad, false);
                    }
                    integridad.VerificarIntegridadSistema(unidad);
                    unidad.Confirmar();
                }
                else if (actualizar)
                {
                    RespaldarAntesDeActualizar(app.ConnectionString);
                    using var unidad = new UnidadTrabajoSeguridadDAL(app.ConnectionString);
                    var integridad = new IntegridadBLL();
                    bool faltabanTablas = !TieneTablasPrincipales(unidad);
                    if (faltabanTablas)
                    {
                        EjecutarEsquemaInicial(unidad);
                    }
                    SchemaMigradorBLL.Aplicar(unidad, false);
                    if (faltabanTablas)
                    {
                        integridad.ForzarRecalculoDeTodaLaBase(unidad);
                    }
                    integridad.VerificarIntegridadSistema(unidad);
                    unidad.Confirmar();
                }
                SchemaMigradorBLL.Validar(app.ConnectionString);
                ConexionDAL.ConnectionStringResuelta = app.ConnectionString;
                new IntegridadBLL().VerificarIntegridadSistema();
                ValidarPermisosOperacion(app.ConnectionString);
                if (persistirConfiguracion) ConexionDAL.GuardarConnectionStringEnConfig(app.ConnectionString);
                RegistrarLog($"Inicialización completada: servidor '{app.DataSource}', base '{nombre}', esquema {SchemaMigradorBLL.VersionActual}.");
            }
            catch
            {
                ConexionDAL.ConnectionStringResuelta = anterior;
                throw;
            }
        }

        private static bool CrearOAdjuntarBaseDeDatos(SqlConnection cnMaster, string dataSource, string nombre)
        {
            if (dataSource.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase))
            {
                string instancia = dataSource.Substring("(localdb)\\".Length).Trim();
                string carpetaInstancia = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "UAI-FIT",
                    "Databases",
                    instancia);
                Directory.CreateDirectory(carpetaInstancia);
                string mdf = Path.Combine(carpetaInstancia, $"{nombre}.mdf");
                string ldf = Path.Combine(carpetaInstancia, $"{nombre}_log.ldf");
                if (File.Exists(mdf))
                {
                    if (IntentarAdjuntarArchivosMdf(cnMaster, nombre, mdf, ldf))
                    {
                        RegistrarLog($"Base de datos existente re-adjuntada desde '{mdf}'.");
                        return false;
                    }
                    string sufijo = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    mdf = Path.Combine(carpetaInstancia, $"{nombre}_{sufijo}.mdf");
                    ldf = Path.Combine(carpetaInstancia, $"{nombre}_{sufijo}_log.ldf");
                }
                CrearBaseConArchivosExplicitos(cnMaster, nombre, mdf, ldf);
                RegistrarLog($"Base de datos LocalDB '{nombre}' creada en '{mdf}'.");
                return true;
            }
            try
            {
                using var crear = new SqlCommand($"CREATE DATABASE [{nombre}];", cnMaster) { CommandTimeout = 90 };
                crear.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex) when (ex.Number == 5170)
            {
                RegistrarLog($"Aviso SQL 5170 al crear '{nombre}': ya existen archivos físicos previos en la ruta por defecto. Intentando resolver...");
                using var cmdPaths = new SqlCommand(@"
                    SELECT 
                        CONVERT(nvarchar(4000), SERVERPROPERTY('InstanceDefaultDataPath')),
                        CONVERT(nvarchar(4000), SERVERPROPERTY('InstanceDefaultLogPath'));", cnMaster);
                using var rdr = cmdPaths.ExecuteReader();
                string dataPath = string.Empty;
                string logPath = string.Empty;
                if (rdr.Read())
                {
                    dataPath = rdr.IsDBNull(0) ? string.Empty : rdr.GetString(0).TrimEnd('\\', '/');
                    logPath = rdr.IsDBNull(1) ? string.Empty : rdr.GetString(1).TrimEnd('\\', '/');
                }
                rdr.Close();
                if (string.IsNullOrWhiteSpace(dataPath))
                {
                    dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UAI-FIT", "Databases", "Default");
                    Directory.CreateDirectory(dataPath);
                }
                if (string.IsNullOrWhiteSpace(logPath)) logPath = dataPath;
                string mdfExistente = Path.Combine(dataPath, $"{nombre}.mdf");
                string ldfExistente = Path.Combine(logPath, $"{nombre}_log.ldf");
                if (File.Exists(mdfExistente) && IntentarAdjuntarArchivosMdf(cnMaster, nombre, mdfExistente, ldfExistente))
                {
                    RegistrarLog($"Base de datos huérfana re-adjuntada exitosamente desde '{mdfExistente}'.");
                    return false;
                }
                string sufijo = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string mdfNuevo = Path.Combine(dataPath, $"{nombre}_{sufijo}.mdf");
                string ldfNuevo = Path.Combine(logPath, $"{nombre}_{sufijo}_log.ldf");
                CrearBaseConArchivosExplicitos(cnMaster, nombre, mdfNuevo, ldfNuevo);
                RegistrarLog($"Base de datos '{nombre}' creada con archivos únicos en '{mdfNuevo}'.");
                return true;
            }
        }

        private static bool IntentarAdjuntarArchivosMdf(SqlConnection cnMaster, string nombre, string rutaMdf, string rutaLdf)
        {
            try
            {
                string mdfEsc = rutaMdf.Replace("'", "''");
                string ldfEsc = rutaLdf.Replace("'", "''");
                string sqlAttach = File.Exists(rutaLdf)
                    ? $"CREATE DATABASE [{nombre}] ON (FILENAME = N'{mdfEsc}'), (FILENAME = N'{ldfEsc}') FOR ATTACH;"
                    : $"CREATE DATABASE [{nombre}] ON (FILENAME = N'{mdfEsc}') FOR ATTACH_REBUILD_LOG;";
                using var cmd = new SqlCommand(sqlAttach, cnMaster) { CommandTimeout = 60 };
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                RegistrarLog($"No fue posible adjuntar '{rutaMdf}' (se creará un archivo nuevo): {ex.Message}");
                return false;
            }
        }

        private static void CrearBaseConArchivosExplicitos(SqlConnection cnMaster, string nombre, string rutaMdf, string rutaLdf)
        {
            string mdfEsc = rutaMdf.Replace("'", "''");
            string ldfEsc = rutaLdf.Replace("'", "''");
            string sql = $@"
                CREATE DATABASE [{nombre}]
                ON PRIMARY (NAME = N'{nombre}', FILENAME = N'{mdfEsc}')
                LOG ON (NAME = N'{nombre}_log', FILENAME = N'{ldfEsc}');";
            using var cmd = new SqlCommand(sql, cnMaster) { CommandTimeout = 90 };
            cmd.ExecuteNonQuery();
        }

        private static bool TieneTablasPrincipales(UnidadTrabajoSeguridadDAL unidad)
        {
            using var cmd = new SqlCommand(@"SELECT COUNT(*) FROM sys.tables WHERE schema_id=SCHEMA_ID(N'dbo') AND name IN
                (N'Bitacora',N'ComprobantePago',N'DetalleOrdenReabastecimiento',N'DetalleVentaProducto',N'DigitoVerificador',
                 N'Etiqueta',N'Idioma',N'OrdenReabastecimiento',N'Permiso',N'Permiso_Permiso',N'PlanNutricional',N'PlanSuscripcion',
                 N'Producto',N'RutinaEntrenamiento',N'Socio',N'Traduccion',N'Turno',N'Usuario',N'Usuario_Historico',N'Usuario_Permiso',N'VentaProducto');", unidad.Conexion, unidad.Transaccion);
            return Convert.ToInt32(cmd.ExecuteScalar()) == 21;
        }

        private static void EjecutarEsquemaInicial(UnidadTrabajoSeguridadDAL unidad)
        {
            string ruta = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath) ?? AppContext.BaseDirectory, "InitDB.sql");
            if (!File.Exists(ruta)) ruta = Path.Combine(AppContext.BaseDirectory, "InitDB.sql");
            if (!File.Exists(ruta)) throw new FileNotFoundException("Falta InitDB.sql. Repare la instalación de UAI-FIT.", ruta);
            string script = File.ReadAllText(ruta, Encoding.UTF8);
            int lote = 0;
            foreach (string texto in Regex.Split(script, @"^\s*GO\s*(?:--.*)?$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
            {
                lote++;
                string sql = texto.Trim();
                if (string.IsNullOrWhiteSpace(sql)) continue;
                if (Regex.IsMatch(sql, @"^(USE\s|ALTER\s+DATABASE\s|IF\s+NOT\s+EXISTS\s*\(SELECT\s+name\s+FROM\s+sys\.databases)", RegexOptions.IgnoreCase)) continue;
                using var cmd = new SqlCommand(sql, unidad.Conexion, unidad.Transaccion) { CommandTimeout = 180 };
                try { cmd.ExecuteNonQuery(); }
                catch (SqlException ex) { throw new InvalidOperationException($"InitDB.sql falló en el lote {lote} (SQL {ex.Number}). No se confirmó el script. Consulte el log.", ex); }
            }
        }

        private static void RespaldarAntesDeActualizar(string connectionString)
        {
            using var cn = new SqlConnection(connectionString); cn.Open();
            var b = new SqlConnectionStringBuilder(connectionString);
            string carpeta = string.Empty;
            if (!b.DataSource.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase))
            {
                using var ruta = new SqlCommand("SELECT CONVERT(nvarchar(4000),SERVERPROPERTY('InstanceDefaultBackupPath'));", cn);
                carpeta = Convert.ToString(ruta.ExecuteScalar()) ?? string.Empty;
            }
            if (string.IsNullOrWhiteSpace(carpeta) || (b.DataSource.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase)))
            {
                carpeta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UAI-FIT", "BackupsPrevios");
                Directory.CreateDirectory(carpeta);
            }
            if (string.IsNullOrWhiteSpace(carpeta)) throw new InvalidOperationException("No se pudo resolver la carpeta SQL de respaldo. Un administrador debe respaldar la base antes de actualizar.");
            string archivo = carpeta.TrimEnd('\\', '/') + "\\" + b.InitialCatalog + "_antes_500_" + Guid.NewGuid().ToString("N") + ".bak";
            using var backup = new SqlCommand($"BACKUP DATABASE [{NombreBase(b.InitialCatalog)}] TO DISK=@ruta WITH COPY_ONLY,CHECKSUM,NAME=N'UAI-FIT;Schema=1'; RESTORE VERIFYONLY FROM DISK=@ruta WITH CHECKSUM;", cn) { CommandTimeout = 180 };
            backup.Parameters.AddWithValue("@ruta", archivo);
            backup.ExecuteNonQuery();
            RegistrarLog("Respaldo previo a actualización verificado en servidor SQL: " + archivo);
        }

        private static void ValidarPermisosOperacion(string connectionString)
        {
            using var cn = new SqlConnection(connectionString); cn.Open();
            var requeridos = new Dictionary<string, string[]>
            {
                ["Usuario"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["Usuario_Historico"] = new[] { "SELECT", "INSERT" },
                ["Usuario_Permiso"] = new[] { "SELECT", "INSERT", "DELETE" },
                ["Permiso"] = new[] { "SELECT", "INSERT", "DELETE" },
                ["Permiso_Permiso"] = new[] { "SELECT", "INSERT", "DELETE" },
                ["Bitacora"] = new[] { "SELECT", "INSERT" },
                ["DigitoVerificador"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["Socio"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["ComprobantePago"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["Producto"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["VentaProducto"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["DetalleVentaProducto"] = new[] { "SELECT", "INSERT" },
                ["OrdenReabastecimiento"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["DetalleOrdenReabastecimiento"] = new[] { "SELECT", "INSERT" },
                ["Turno"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["PlanNutricional"] = new[] { "SELECT", "INSERT", "UPDATE" },
                ["RutinaEntrenamiento"] = new[] { "SELECT", "INSERT", "UPDATE", "DELETE" },
                ["sp_Usuario_ActualizarEstado"] = new[] { "EXECUTE" },
                ["sp_Usuario_ObtenerPorUsername"] = new[] { "EXECUTE" }
            };
            foreach (var requisito in requeridos)
                foreach (string permiso in requisito.Value)
                {
                    using var cmd = new SqlCommand("SELECT HAS_PERMS_BY_NAME(@objeto,'OBJECT',@permiso);", cn);
                    cmd.Parameters.AddWithValue("@objeto", "dbo." + requisito.Key); cmd.Parameters.AddWithValue("@permiso", permiso);
                    if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                        throw new UnauthorizedAccessException($"La identidad SQL no tiene {permiso} sobre dbo.{requisito.Key}. Solicite al administrador los permisos de operación UAI-FIT.");
                }
        }

        private static bool ExisteBaseDeDatos(string masterConnectionString, string nombreBase)
        {
            using var cn = new SqlConnection(masterConnectionString);
            cn.Open();
            using var cmd = new SqlCommand("SELECT DB_ID(@nombre);", cn);
            cmd.Parameters.AddWithValue("@nombre", nombreBase);
            object? res = cmd.ExecuteScalar();
            return res != null && res != DBNull.Value;
        }

        public static bool AsegurarConexionYBaseDisponibleEnArranque()
        {
            try
            {
                string connection = ConexionDAL.ObtenerConnectionStringConfigurada();
                var builderInicial = new SqlConnectionStringBuilder(connection);
                string nombreBase = NombreBase(builderInicial.InitialCatalog);
                bool conectado = ProbarConexion(connection, 5);
                if (!conectado && builderInicial.DataSource.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        IniciarLocalDb(builderInicial.DataSource);
                        conectado = ProbarConexion(connection, 8);
                    }
                    catch (Exception exLocal)
                    {
                        RegistrarLog("No se pudo auto-iniciar LocalDB en el arranque: " + exLocal.Message);
                    }
                }
                if (!conectado)
                {
                    if (MessageBox.Show(
                        $"No se pudo establecer conexión con la base de datos '{nombreBase}' en la instancia '{builderInicial.DataSource}'.\r\n\r\n" +
                        "¿Desea comprobar el estado del servicio SQL Server o seleccionar la instancia disponible?",
                        "Conexión UAI-FIT",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return false;
                    }
                    string master = ResolverYValidarConexionServidorSQL(connection, true);
                    var bResuelto = new SqlConnectionStringBuilder(master)
                    {
                        InitialCatalog = nombreBase,
                        TrustServerCertificate = true,
                        ConnectTimeout = 8
                    };
                    if (!ExisteBaseDeDatos(master, nombreBase))
                    {
                        if (MessageBox.Show(
                            $"Se estableció conexión con el servidor '{bResuelto.DataSource}', pero la base de datos '{nombreBase}' no existe en esa instancia.\r\n\r\n" +
                            $"¿Desea crear e inicializar una nueva base de datos '{nombreBase}' en '{bResuelto.DataSource}'?",
                            "Base de datos inexistente en la instancia",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                        {
                            return false;
                        }
                        InicializarBaseDeDatos(bResuelto.ConnectionString);
                    }
                    else
                    {
                        if (SchemaMigradorBLL.RequiereActualizacion(bResuelto.ConnectionString))
                        {
                            if (!RepararEsquemaBaseDeDatosInteractivo(bResuelto.ConnectionString)) return false;
                        }
                        else
                        {
                            SchemaMigradorBLL.Validar(bResuelto.ConnectionString);
                            ValidarPermisosOperacion(bResuelto.ConnectionString);
                            ConexionDAL.GuardarConnectionStringEnConfig(bResuelto.ConnectionString);
                        }
                    }
                }
                else if (SchemaMigradorBLL.RequiereActualizacion(connection))
                {
                    if (!RepararEsquemaBaseDeDatosInteractivo(connection)) return false;
                }
                string connActiva = ConexionDAL.ObtenerConnectionStringConfigurada();
                SchemaMigradorBLL.Validar(connActiva);
                ValidarPermisosOperacion(connActiva);
                return true;
            }
            catch (Exception ex)
            {
                RegistrarLog("Inicio no disponible: " + ex);
                MessageBox.Show(ex.Message + "\r\n\r\nRegistro: " + RutaArchivoLog, "No se puede iniciar UAI-FIT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool RepararEsquemaBaseDeDatosInteractivo(string? appConnectionString = null)
        {
            if (MessageBox.Show("La base necesita una actualización. Se creará primero un respaldo SQL y se aplicarán cambios compatibles, conservando sus datos. ¿Desea continuar?", "Actualizar UAI-FIT", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return false;
            try { InicializarBaseDeDatos(appConnectionString ?? ConexionDAL.ObtenerConnectionStringConfigurada()); return true; }
            catch (Exception ex)
            {
                RegistrarLog("Actualización no completada: " + ex);
                MessageBox.Show(ex.Message + "\r\nConsulte: " + RutaArchivoLog, "Actualización no completada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool EsquemaBaseDeDatosCompleto(string connectionString)
        {
            try { SchemaMigradorBLL.Validar(connectionString); return true; }
            catch (Exception ex) { RegistrarLog("Validación de esquema: " + ex.Message); return false; }
        }

        public static string ResolverYValidarConexionServidorSQL(string connectionString, bool permitirInteractivo)
        {
            var b = new SqlConnectionStringBuilder(string.IsNullOrWhiteSpace(connectionString) ? ConexionPredeterminada : connectionString)
            { InitialCatalog = "master", TrustServerCertificate = true, ConnectTimeout = 5 };
            for (int intento = 0; intento < (permitirInteractivo ? 3 : 1); intento++)
            {
                if (b.DataSource.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase)) IniciarLocalDb(b.DataSource);
                if (ProbarConexion(b.ConnectionString, 5)) return b.ConnectionString;
                if (!permitirInteractivo) break;
                string? servicio = ServicioLocal(b.DataSource);
                if (servicio != null && ConsultarEstadoServicioLocal(servicio) == EstadoServicioSQL.Detenido &&
                    MessageBox.Show($"No respondió '{b.DataSource}'. ¿Desea intentar iniciar el servicio local {servicio}? Requiere autorización administrativa.", "Servicio SQL", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    IntentarIniciarServicioSQL(servicio);
                    if (ProbarConexion(b.ConnectionString, 8)) return b.ConnectionString;
                }
                string entrada = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la instancia SQL que desea usar (ej.: (localdb)\\UAIFIT, .\\SQLEXPRESS o .). No se seleccionará otra automáticamente.", "Instancia SQL", b.DataSource);
                if (string.IsNullOrWhiteSpace(entrada)) break;
                b.DataSource = entrada.Trim();
            }
            throw new InvalidOperationException($"No se pudo acceder a '{b.DataSource}'. Compruebe la instalación del motor, el servicio, la conectividad y el acceso de su cuenta Windows. No se cambió la instancia configurada.");
        }

        private static bool ProbarConexion(string connectionString, int timeout)
        {
            try { using var cn = new SqlConnection(new SqlConnectionStringBuilder(connectionString) { ConnectTimeout = timeout }.ConnectionString); cn.Open(); return true; }
            catch (SqlException ex) { RegistrarLog("Prueba SQL fallida (" + ex.Number + "): " + ex.Message); return false; }
        }

        private static void IniciarLocalDb(string servidor)
        {
            string instancia = servidor.Substring("(localdb)\\".Length);
            if (!Regex.IsMatch(instancia, @"^[A-Za-z0-9_-]{1,64}$")) throw new ArgumentException("Nombre de instancia LocalDB inválido.");
            string? exe = BuscarLocalDb();
            if (exe == null) throw new InvalidOperationException("Falta SQL Server LocalDB compatible. Ejecute nuevamente el instalador y autorice la instalación del motor, o seleccione una instancia SQL existente.");
            Ejecutar(exe, new[] { "create", instancia }, 20000, false);
            if (!Ejecutar(exe, new[] { "start", instancia }, 30000, false))
                throw new InvalidOperationException("No se pudo iniciar la instancia LocalDB seleccionada. Consulte el diagnóstico y la instalación del motor.");
        }

        private static string? BuscarLocalDb()
        {
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            foreach (string version in new[] { "170", "160", "150", "140", "130" })
            {
                string candidato = Path.Combine(programFiles, "Microsoft SQL Server", version, "Tools", "Binn", "SqlLocalDB.exe");
                if (File.Exists(candidato)) return candidato;
            }
            try
            {
                string sqlRoot = Path.Combine(programFiles, "Microsoft SQL Server");
                if (Directory.Exists(sqlRoot))
                {
                    string? encontrado = Directory.GetFiles(sqlRoot, "SqlLocalDB.exe", SearchOption.AllDirectories)
                        .OrderByDescending(x => x)
                        .FirstOrDefault();
                    if (!string.IsNullOrEmpty(encontrado)) return encontrado;
                }
            }
            catch { }
            return null;
        }

        private static EstadoServicioSQL ConsultarEstadoServicioLocal(string nombreServicio)
        {
            try
            {
                var psi = new ProcessStartInfo(Path.Combine(Environment.SystemDirectory, "sc.exe"))
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                psi.ArgumentList.Add("query");
                psi.ArgumentList.Add(nombreServicio);
                using var proceso = Process.Start(psi) ?? throw new InvalidOperationException("No se pudo consultar el servicio SQL.");
                var lectura = proceso.StandardOutput.ReadToEndAsync();
                if (!proceso.WaitForExit(5000))
                {
                    try { proceso.Kill(); } catch { }
                    throw new TimeoutException("La consulta del servicio SQL no respondió.");
                }
                string salida = lectura.GetAwaiter().GetResult();
                if (proceso.ExitCode == 1060) return EstadoServicioSQL.NoExiste;
                if (Regex.IsMatch(salida, @"STATE\s*:\s*4\b|\b(RUNNING|EN_EJECUCION)\b", RegexOptions.IgnoreCase))
                    return EstadoServicioSQL.EnEjecucion;
                if (Regex.IsMatch(salida, @"STATE\s*:\s*1\b|\b(STOPPED|DETENIDO)\b", RegexOptions.IgnoreCase))
                    return EstadoServicioSQL.Detenido;
                return EstadoServicioSQL.Desconocido;
            }
            catch (Exception ex)
            {
                RegistrarLog($"Error al consultar estado del servicio '{nombreServicio}': {ex.Message}");
                return EstadoServicioSQL.Desconocido;
            }
        }

        public static int ComprobarEntorno(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            if (builder.DataSource.StartsWith("(localdb)\\", StringComparison.OrdinalIgnoreCase))
            {
                bool tieneLocalDb = BuscarLocalDb() != null;
                RegistrarLog($"ComprobarEntorno LocalDB ('{builder.DataSource}'): instalado={tieneLocalDb}");
                return tieneLocalDb ? 0 : 10;
            }
            string? servicio = ServicioLocal(builder.DataSource);
            if (servicio == null) return 0;
            EstadoServicioSQL estado = ConsultarEstadoServicioLocal(servicio);
            RegistrarLog($"ComprobarEntorno servicio local '{servicio}' ('{builder.DataSource}'): estado={estado}");
            if (estado == EstadoServicioSQL.NoExiste) return 30;
            if (estado == EstadoServicioSQL.Detenido) return 20;
            return 0;
        }

        public static bool IniciarServicioConfigurado(string connectionString)
        {
            string? servicio = ServicioLocal(new SqlConnectionStringBuilder(connectionString).DataSource);
            return servicio != null && IntentarIniciarServicioSQL(servicio);
        }

        private static bool Ejecutar(string exe, string[] argumentos, int timeout, bool elevar)
        {
            var psi = new ProcessStartInfo(exe) { UseShellExecute = elevar, CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden };
            foreach (string argumento in argumentos) psi.ArgumentList.Add(argumento);
            if (elevar) psi.Verb = "runas";
            using var proceso = Process.Start(psi) ?? throw new InvalidOperationException("No se pudo iniciar " + Path.GetFileName(exe));
            if (!proceso.WaitForExit(timeout)) { try { proceso.Kill(); } catch { } throw new TimeoutException("Se agotó la espera de " + Path.GetFileName(exe)); }
            return proceso.ExitCode == 0;
        }

        private static string? ServicioLocal(string servidor)
        {
            string[] partes = servidor.Split('\\');
            if (!new[] { ".", "localhost", "(local)", Environment.MachineName }.Contains(partes[0], StringComparer.OrdinalIgnoreCase)) return null;
            string servicio = partes.Length == 1 ? "MSSQLSERVER" : "MSSQL$" + partes[1];
            return Regex.IsMatch(servicio, @"^MSSQL(SERVER|\$[A-Za-z0-9_]+)$") ? servicio : null;
        }

        public static bool IntentarIniciarServicioSQL(string nombreServicio)
        {
            if (!Regex.IsMatch(nombreServicio, @"^MSSQL(SERVER|\$[A-Za-z0-9_]+)$")) return false;
            try
            {
                if (ConsultarEstadoServicioLocal(nombreServicio) == EstadoServicioSQL.EnEjecucion)
                    return true;
                string scPath = Path.Combine(Environment.SystemDirectory, "sc.exe");
                try
                {
                    Ejecutar(scPath, new[] { "start", nombreServicio }, 15000, false);
                }
                catch
                {
                    Ejecutar(scPath, new[] { "start", nombreServicio }, 30000, true);
                }
                if (ConsultarEstadoServicioLocal(nombreServicio) == EstadoServicioSQL.Detenido)
                {
                    Ejecutar(scPath, new[] { "start", nombreServicio }, 30000, true);
                }
                for (int i = 0; i < 40; i++)
                {
                    if (ConsultarEstadoServicioLocal(nombreServicio) == EstadoServicioSQL.EnEjecucion)
                    {
                        RegistrarLog($"Servicio '{nombreServicio}' en estado RUNNING.");
                        return true;
                    }
                    Thread.Sleep(500);
                }
                return ConsultarEstadoServicioLocal(nombreServicio) == EstadoServicioSQL.EnEjecucion;
            }
            catch (Exception ex)
            {
                RegistrarLog("No se pudo iniciar servicio: " + ex.Message);
                return ConsultarEstadoServicioLocal(nombreServicio) == EstadoServicioSQL.EnEjecucion;
            }
        }
    }
}