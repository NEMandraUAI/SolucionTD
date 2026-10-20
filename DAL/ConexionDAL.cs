using Microsoft.Data.SqlClient;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public sealed class ConexionDAL
    {
        private static ConexionDAL instancia;
        private static readonly object candado = new object();
        public static string ConnectionStringTemporal { get; set; }
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
        public SqlConnection ObtenerConexion()
        {
            string connectionString;
            if (!string.IsNullOrEmpty(ConnectionStringTemporal))
            {
                connectionString = ConnectionStringTemporal;
            }
            else
            {
                string configConn = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(configConn) || configConn == "DB_CONNECTION_STRING_PLACEHOLDER")
                {
                    connectionString = "Server=.;Database=ProyectoCampo;Trusted_Connection=True;TrustServerCertificate=True;";
                }
                else
                {
                    connectionString = configConn;
                }
            }
            return new SqlConnection(connectionString);
        }
    }
}
