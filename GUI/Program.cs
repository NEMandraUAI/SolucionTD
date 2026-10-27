using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BLL;
using DAL;

namespace GUI
{
    internal static class Program
    {
        private static bool informandoError;

        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();
            if (args.Length > 0 && new[] { "--init-db", "--verify-db", "--check-environment", "--start-sql-service" }.Contains(args[0]))
            {
                try
                {
                    if (!args.Contains("--use-current-config") && (args.Length < 2 || args[1].StartsWith("--")))
                        throw new ArgumentException("Debe indicar la conexión SQL de destino.");
                    string connection = args.Contains("--use-current-config")
                        ? ConexionDAL.ObtenerConnectionStringConfigurada() : args[1];
                    if (args[0] == "--check-environment")
                    {
                        Environment.ExitCode = InstaladorBLL.ComprobarEntorno(connection);
                        return;
                    }
                    if (args[0] == "--start-sql-service")
                    {
                        Environment.ExitCode = InstaladorBLL.IniciarServicioConfigurado(connection) ? 0 : 1;
                        return;
                    }
                    if (args[0] == "--init-db")
                        InstaladorBLL.InicializarBaseDeDatos(connection, !args.Contains("--no-save-config"));
                    else
                    {
                        ConexionDAL.ConnectionStringResuelta = connection;
                        SchemaMigradorBLL.Validar(connection);
                        new IntegridadBLL().VerificarIntegridadSistema();
                        InstaladorBLL.RegistrarLog("Verificación solicitada completada en lectura.");
                    }
                    Environment.ExitCode = 0;
                }
                catch (Exception ex)
                {
                    InstaladorBLL.RegistrarLog("ERROR en inicialización/verificación: " + ex);
                    if (!args.Contains("--quiet")) MessageBox.Show(ex.Message + "\r\n\r\nConsulte: " + InstaladorBLL.RutaArchivoLog,
                        "UAI-FIT no pudo completar la configuración", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.ExitCode = 1;
                }
                return;
            }

            Mutex instancia;
            bool nuevaInstancia;
            try { instancia = new Mutex(true, @"Global\UAI-FIT-9FA8AE34", out nuevaInstancia); }
            catch (Exception ex) when (ex is UnauthorizedAccessException || ex is System.IO.IOException)
            {
                InstaladorBLL.RegistrarLog("No se pudo adquirir la instancia única: " + ex);
                MessageBox.Show("UAI-FIT podría estar abierto en otra sesión de Windows. Cierre esa instancia y vuelva a intentarlo.", "UAI-FIT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using var mutexInstancia = instancia;
            if (!nuevaInstancia)
            {
                MessageBox.Show("UAI-FIT ya está abierto en este equipo.", "UAI-FIT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, e) => ErrorFinal(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            {
                InstaladorBLL.RegistrarLog("Error no recuperable: " + e.ExceptionObject);
            };
            try { Application.Run(new frmMenu()); }
            catch (Exception ex) { ErrorFinal(ex); }
        }

        private static void ErrorFinal(Exception ex)
        {
            if (informandoError) return;
            informandoError = true;
            InstaladorBLL.RegistrarLog("Error de interfaz; cierre controlado: " + ex);
            Seguridad.SessionManager.Instancia.Cerrar();
            MessageBox.Show("La operación no pudo completarse de forma segura. UAI-FIT se cerrará. Si ocurrió durante un cobro, consulte el historial antes de repetirlo.\r\n\r\n" +
                "Diagnóstico: " + InstaladorBLL.RutaArchivoLog, "UAI-FIT — error controlado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Environment.ExitCode = 1;
            Application.Exit();
        }
    }
}
