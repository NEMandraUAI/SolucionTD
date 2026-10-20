using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class IdiomaDAL
    {
        public List<IdiomaBE> ObtenerIdiomas()
        {
            List<IdiomaBE> lista = new List<IdiomaBE>();
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID, Nombre FROM Idioma", cn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new IdiomaBE { ID = Convert.ToInt32(reader["ID"]), Nombre = reader["Nombre"].ToString() });
                    }
                }
            }
            return lista;
        }
        public Dictionary<string, string> ObtenerTraduccionesPorFormulario(int idIdioma, string formulario)
        {
            Dictionary<string, string> traducciones = new Dictionary<string, string>();
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string query = @"SELECT e.NombreControl, t.Texto 
                                 FROM Traduccion t 
                                 INNER JOIN Etiqueta e ON t.ID_Etiqueta = e.ID 
                                 WHERE t.ID_Idioma = @IdIdioma AND e.Formulario = @Formulario";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                cmd.Parameters.AddWithValue("@Formulario", formulario);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string clave = reader["NombreControl"].ToString();
                        string texto = reader["Texto"].ToString();
                        traducciones[clave] = texto;
                    }
                }
            }
            return traducciones;
        }
        public void CrearIdiomaConSufijo(string nombreIdioma, string sufijo)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    SqlCommand cmdInsertIdioma = new SqlCommand("INSERT INTO Idioma (Nombre) VALUES (@Nombre); SELECT SCOPE_IDENTITY();", cn, tx);
                    cmdInsertIdioma.Parameters.AddWithValue("@Nombre", nombreIdioma);
                    int nuevoIdIdioma = Convert.ToInt32(cmdInsertIdioma.ExecuteScalar());
                    string queryCopia = @"INSERT INTO Traduccion (ID_Idioma, ID_Etiqueta, Texto)
                                          SELECT @NuevoId, ID_Etiqueta, Texto + ' _ ' + @Sufijo
                                          FROM Traduccion WHERE ID_Idioma = 1";
                    SqlCommand cmdCopia = new SqlCommand(queryCopia, cn, tx);
                    cmdCopia.Parameters.AddWithValue("@NuevoId", nuevoIdIdioma);
                    cmdCopia.Parameters.AddWithValue("@Sufijo", sufijo);
                    cmdCopia.ExecuteNonQuery();
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
        public List<TraduccionBE> ObtenerTodasLasTraducciones(int idIdioma)
        {
            List<TraduccionBE> lista = new List<TraduccionBE>();
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string query = @"SELECT t.ID, e.Formulario, e.NombreControl, t.Texto 
                                FROM Traduccion t 
                                INNER JOIN Etiqueta e ON t.ID_Etiqueta = e.ID 
                                WHERE t.ID_Idioma = @IdIdioma";
                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@IdIdioma", idIdioma);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new TraduccionBE
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Formulario = reader["Formulario"].ToString(),
                            NombreControl = reader["NombreControl"].ToString(),
                            Texto = reader["Texto"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
        public void ActualizarTraducciones(List<TraduccionBE> traducciones)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    foreach (var trad in traducciones)
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE Traduccion SET Texto = @Texto WHERE ID = @ID", cn, tx);
                        cmd.Parameters.AddWithValue("@Texto", trad.Texto);
                        cmd.Parameters.AddWithValue("@ID", trad.ID);
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
        public void EliminarIdioma(int idIdioma)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                SqlTransaction tx = cn.BeginTransaction();
                try
                {
                    SqlCommand cmdUsu = new SqlCommand("UPDATE Usuario SET ID_Idioma = 1 WHERE ID_Idioma = @IdIdioma", cn, tx);
                    cmdUsu.Parameters.AddWithValue("@IdIdioma", idIdioma);
                    cmdUsu.ExecuteNonQuery();
                    SqlCommand cmdTrad = new SqlCommand("DELETE FROM Traduccion WHERE ID_Idioma = @IdIdioma", cn, tx);
                    cmdTrad.Parameters.AddWithValue("@IdIdioma", idIdioma);
                    cmdTrad.ExecuteNonQuery();
                    SqlCommand cmdIdi = new SqlCommand("DELETE FROM Idioma WHERE ID = @IdIdioma", cn, tx);
                    cmdIdi.Parameters.AddWithValue("@IdIdioma", idIdioma);
                    cmdIdi.ExecuteNonQuery();
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
    }
}
