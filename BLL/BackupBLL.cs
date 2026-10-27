using BE;
using DAL;
using Seguridad;
using Microsoft.Data.SqlClient;
using System.Security.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BackupBLL
    {
        private BackupDAL backupDAL = new BackupDAL();
        private RegistroBLL registroBLL = new RegistroBLL();
        public void RealizarBackup(string rutaArchivo)
        {
            UsuarioBE usuario;
            using (var unidad = new UnidadTrabajoSeguridadDAL())
            {
                var sesion = SessionManager.Instancia.UsuarioActual;
                if (sesion == null) throw new UnauthorizedAccessException("Debe iniciar sesión para generar un respaldo.");
                new IntegridadBLL().VerificarIntegridadUsuario(unidad);
                usuario = new UsuarioDAL(unidad).ObtenerPorID(sesion.ID)
                    ?? throw new UnauthorizedAccessException("La cuenta de la sesión no existe.");
                new PermisoBLL(unidad).LlenarPermisosDeUsuario(usuario);
                if (usuario.Bloqueado || !usuario.TienePermiso("REALIZAR_BACKUP"))
                    throw new UnauthorizedAccessException("No posee el permiso requerido (REALIZAR_BACKUP).");
                unidad.Confirmar();
            }
            backupDAL.RealizarBackup(rutaArchivo);
            try { registroBLL.RegistrarEvento("Backup del sistema generado y verificado en ruta local.", usuario, "ALTA"); }
            catch (Exception ex) { InstaladorBLL.RegistrarLog("Backup generado correctamente; falló su registro en bitácora: " + ex.Message); }
        }
        public void RestaurarBackup(string rutaArchivo)
        {
            AutorizarRecuperacionAdministrativa();
            InstaladorBLL.RegistrarLog("Recuperación administrativa: inicio de restauración de backup.");
            try
            {
                backupDAL.RestaurarBackup(rutaArchivo);
                SchemaMigradorBLL.Aplicar(ConexionDAL.ObtenerConnectionStringConfigurada());
                new IntegridadBLL().VerificarIntegridadSistema();
                InstaladorBLL.RegistrarLog("Recuperación administrativa: restauración y verificación completadas.");
            }
            catch (Exception ex)
            {
                InstaladorBLL.RegistrarLog("ERROR en recuperación administrativa: " + ex.Message);
                throw;
            }
            finally
            {
                if (backupDAL.UltimoBackupPrevio != null)
                    InstaladorBLL.RegistrarLog("Backup conservado antes de restauración: " + backupDAL.UltimoBackupPrevio);
                // La base restaurada puede tener otras credenciales y permisos.
                SessionManager.Instancia.Cerrar();
            }
        }

        public void AutorizarRecuperacionAdministrativa()
        {
            // La recuperación no confía en Usuario/Permiso: su integridad puede
            // ser precisamente la causa que impide iniciar sesión.
            using var identidad = WindowsIdentity.GetCurrent();
            if (!new WindowsPrincipal(identidad).IsInRole(WindowsBuiltInRole.Administrator))
                throw new UnauthorizedAccessException("La recuperación requiere abrir UAI-FIT excepcionalmente como administrador de Windows, con la misma cuenta que accede a la base. El uso normal no requiere elevación.");

            var builder = new SqlConnectionStringBuilder(ConexionDAL.ObtenerConnectionStringConfigurada());
            if (string.IsNullOrWhiteSpace(builder.InitialCatalog) ||
                new[] { "master", "model", "msdb", "tempdb" }.Contains(builder.InitialCatalog, StringComparer.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("Seleccione una base de UAI-FIT válida antes de intentar recuperarla.");
            string baseObjetivo = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            using var cn = new SqlConnection(builder.ConnectionString);
            cn.Open();
            using var cmd = new SqlCommand(@"SELECT CASE WHEN IS_SRVROLEMEMBER('sysadmin') = 1
                OR HAS_PERMS_BY_NAME(@base, 'DATABASE', 'CONTROL') = 1
                THEN 1 ELSE 0 END;", cn);
            cmd.Parameters.AddWithValue("@base", baseObjetivo);
            cmd.CommandTimeout = 10;
            if (Convert.ToInt32(cmd.ExecuteScalar()) != 1)
                throw new UnauthorizedAccessException("La identidad de SQL Server debe tener CONTROL sobre la base configurada o pertenecer a sysadmin para recuperarla.");
            InstaladorBLL.RegistrarLog("Recuperación autorizada mediante identidad Windows elevada: " + identidad.Name);
        }

        public void RecalcularIntegridadAdministrativamente()
        {
            AutorizarRecuperacionAdministrativa();
            InstaladorBLL.RegistrarLog("Recuperación administrativa: inicio de recálculo de integridad autorizado.");
            try
            {
                var integridad = new IntegridadBLL();
                integridad.ForzarRecalculoDeTodaLaBase();
                integridad.VerificarIntegridadSistema();
                InstaladorBLL.RegistrarLog("Recuperación administrativa: recálculo y verificación completados.");
            }
            catch (Exception ex)
            {
                InstaladorBLL.RegistrarLog("ERROR en recálculo administrativo: " + ex.Message);
                throw;
            }
            finally
            {
                SessionManager.Instancia.Cerrar();
            }
        }
    }
}
