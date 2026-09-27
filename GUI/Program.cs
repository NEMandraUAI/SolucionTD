using BLL;

namespace GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--init-db")
            {
                try
                {
                    string serverConnectionString = args.Length > 1 ? args[1] : null;
                    if (string.IsNullOrEmpty(serverConnectionString))
                    {
                        throw new Exception("El instalador no proporcionó el string de conexión.");
                    }
                    InstaladorBLL.InicializarBaseDeDatos(serverConnectionString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ATENCIÓN: Falló la creación de la Base de Datos.\n\nMotivo: " + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace,
                    "Error de Instalación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
                return;
            }
            ApplicationConfiguration.Initialize();
            Application.Run(new frmMenu());
        }
    }
}