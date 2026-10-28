using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Text;

namespace DAL
{
    public sealed class ConexionDAL
    {
        private static ConexionDAL instancia;
        private static readonly object candado = new object();
        public const string PlaceholderConfig = "DB_CONNECTION_STRING_PLACEHOLDER";
        private const string FallbackDefaultConn = @"Server=(localdb)\UAIFIT;Database=ProyectoCampo;Trusted_Connection=True;TrustServerCertificate=True;";
        public static string ConnectionStringTemporal { get; set; }
        public static string ConnectionStringResuelta { get; set; }
        public static string RutaConfigUsuario =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "UAI-FIT", "dbconfig.cfg");

        private ConexionDAL()
        {
        }

        public static ConexionDAL Instancia
        {
            get
            {
                lock (candado)
                {
                    if (instancia == null)
                    {
                        instancia = new ConexionDAL();
                    }
                    return instancia;
                }
            }
        }

        public static string ObtenerConnectionStringConfigurada()
        {
            if (!string.IsNullOrWhiteSpace(ConnectionStringResuelta))
            {
                return ConnectionStringResuelta;
            }
            try
            {
                if (File.Exists(RutaConfigUsuario))
                {
                    string localConn = File.ReadAllText(RutaConfigUsuario, Encoding.UTF8).Trim();
                    if (string.IsNullOrWhiteSpace(localConn) || string.Equals(localConn, PlaceholderConfig, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("El archivo de conexión está vacío o no fue configurado.");
                    if (!System.Text.RegularExpressions.Regex.IsMatch(localConn, @"(^|;)\s*(Data\s+Source|Server)\s*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase) ||
                        !System.Text.RegularExpressions.Regex.IsMatch(localConn, @"(^|;)\s*(Initial\s+Catalog|Database)\s*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        throw new InvalidOperationException("El archivo de configuración no contiene los parámetros obligatorios de servidor y base de datos.");
                    }
                    var builder = new SqlConnectionStringBuilder(localConn);
                    string[] basesSistema = { "master", "model", "msdb", "tempdb" };
                    if (string.IsNullOrWhiteSpace(builder.DataSource) ||
                        string.IsNullOrWhiteSpace(builder.InitialCatalog) ||
                        Array.Exists(basesSistema, b => b.Equals(builder.InitialCatalog.Trim(), StringComparison.OrdinalIgnoreCase)) ||
                        !builder.IntegratedSecurity)
                    {
                        throw new InvalidOperationException("Los parámetros de conexión SQL en el archivo de configuración son incompletos o inválidos.");
                    }
                    return builder.ConnectionString;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("No se pudo leer una configuración SQL válida en " + RutaConfigUsuario + ". Repare la configuración desde el instalador.", ex);
            }
            return FallbackDefaultConn;
        }

        public SqlConnection ObtenerConexion()
        {
            string connectionString = !string.IsNullOrWhiteSpace(ConnectionStringTemporal)
                ? ConnectionStringTemporal
                : ObtenerConnectionStringConfigurada();
            return new SqlConnection(connectionString);
        }

        public static void GuardarConnectionStringEnConfig(string nuevaConnectionString)
        {
            string limpia = new SqlConnectionStringBuilder(nuevaConnectionString).ConnectionString;
            string dir = Path.GetDirectoryName(RutaConfigUsuario)!;
            Directory.CreateDirectory(dir);
            string temporal = Path.Combine(dir, "dbconfig-" + Guid.NewGuid().ToString("N") + ".tmp");
            try
            {
                File.WriteAllText(temporal, limpia, new UTF8Encoding(false));
                File.Move(temporal, RutaConfigUsuario, true);
                ConnectionStringResuelta = limpia;
            }
            finally
            {
                if (File.Exists(temporal)) File.Delete(temporal);
            }
        }
    }
}
