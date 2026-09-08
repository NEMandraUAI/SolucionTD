using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RutinaEntrenamientoDAL
    {
        public void InsertarRutina(RutinaEntrenamientoBE rutina)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO RutinaEntrenamiento (FechaCreacion, ObjetivoFisico, FrecuenciaSemanal, DetalleEjercicios, ID_Socio, ID_Usuario, DVH) OUTPUT INSERTED.CodigoRutina VALUES (@FechaCreacion, @ObjetivoFisico, @FrecuenciaSemanal, @DetalleEjercicios, @ID_Socio, @ID_Usuario, @DVH)", cx);
                cmd.Parameters.AddWithValue("@FechaCreacion", rutina.FechaCreacion);
                cmd.Parameters.AddWithValue("@ObjetivoFisico", rutina.ObjetivoFisico);
                cmd.Parameters.AddWithValue("@FrecuenciaSemanal", rutina.FrecuenciaSemanal);
                cmd.Parameters.AddWithValue("@DetalleEjercicios", rutina.DetalleEjercicios);
                cmd.Parameters.AddWithValue("@ID_Socio", rutina.Socio.ID_Socio);
                cmd.Parameters.AddWithValue("@ID_Usuario", rutina.Entrenador.ID);
                cmd.Parameters.AddWithValue("@DVH", (object)rutina.DVH ?? DBNull.Value);
                rutina.CodigoRutina = (int)cmd.ExecuteScalar();
            }
        }

        public List<string> ObtenerTodosLosDVH()
        {
            List<string> lista = new List<string>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("SELECT DVH FROM RutinaEntrenamiento ORDER BY CodigoRutina", cx);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : "");
                    }
                }
            }
            return lista;
        }
    }
}
