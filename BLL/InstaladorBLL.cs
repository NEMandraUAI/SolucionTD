using System;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using DAL;

namespace BLL
{
    public static class InstaladorBLL
    {
        public static void InicializarBaseDeDatos(string serverConnectionString)
        {
            ConexionDAL.ConnectionStringTemporal = serverConnectionString;
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InitDB.sql");
            if (!File.Exists(scriptPath)) throw new Exception("No se encontró InitDB.sql");
            string scriptContent = File.ReadAllText(scriptPath);
            string[] comandos = Regex.Split(scriptContent, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            using (SqlConnection conexion = ConexionDAL.Instancia.ObtenerConexion())
            {
                conexion.Open();
                foreach (string comandoSql in comandos)
                {
                    if (string.IsNullOrWhiteSpace(comandoSql)) continue;
                    using (SqlCommand cmd = new SqlCommand(comandoSql, conexion))
                    {
                        cmd.CommandTimeout = 120;
                        try
                        {
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            if (ex.Number == 2714 || ex.Number == 1801)
                            {
                                continue;
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }
                }
            }
            ConexionDAL.ConnectionStringTemporal = null;
        }
    }
}
