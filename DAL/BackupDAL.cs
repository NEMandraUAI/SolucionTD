using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BackupDAL
    {
        public void RealizarBackup(string rutaArchivo)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string query = "BACKUP DATABASE [ProyectoCampo] TO DISK = @ruta WITH FORMAT, MEDIANAME = 'SQLServerBackups', NAME = 'Full Backup of ProyectoCampo';";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.AddWithValue("@ruta", rutaArchivo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RestaurarBackup(string rutaArchivo)
        {
            string cadenaOriginal = ConexionDAL.Instancia.ObtenerConexion().ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(cadenaOriginal);
            builder.InitialCatalog = "master";
            SqlConnection.ClearAllPools();
            using (SqlConnection cn = new SqlConnection(builder.ConnectionString))
            {
                cn.Open();
                try
                {
                    string querySingle = "ALTER DATABASE [ProyectoCampo] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
                    using (SqlCommand cmd = new SqlCommand(querySingle, cn))
                    {
                        cmd.CommandTimeout = 60;
                        cmd.ExecuteNonQuery();
                    }
                    string queryRestore = "RESTORE DATABASE [ProyectoCampo] FROM DISK = @ruta WITH REPLACE;";
                    using (SqlCommand cmd = new SqlCommand(queryRestore, cn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@ruta", rutaArchivo);
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    try
                    {
                        string queryMulti = "ALTER DATABASE [ProyectoCampo] SET MULTI_USER;";
                        using (SqlCommand cmd = new SqlCommand(queryMulti, cn))
                        {
                            cmd.CommandTimeout = 60;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch { }
                }
            }
        }
    }
}