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

        public List<RutinaEntrenamientoBE> ObtenerRutinasPorDNI(string dni)
        {
            List<RutinaEntrenamientoBE> lista = new List<RutinaEntrenamientoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT r.CodigoRutina, r.FechaCreacion, r.ObjetivoFisico, r.FrecuenciaSemanal, r.DetalleEjercicios, r.ID_Socio, r.ID_Usuario, r.DVH, s.DNI 
                       FROM RutinaEntrenamiento r 
                       INNER JOIN Socio s ON r.ID_Socio = s.ID_Socio 
                       WHERE s.DNI = @DNI";
                SqlCommand cmd = new SqlCommand(sql, cx);
                cmd.Parameters.AddWithValue("@DNI", dni);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        RutinaEntrenamientoBE rut = new RutinaEntrenamientoBE();
                        rut.CodigoRutina = Convert.ToInt32(dr["CodigoRutina"]);
                        rut.FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]);
                        rut.ObjetivoFisico = dr["ObjetivoFisico"].ToString();
                        rut.FrecuenciaSemanal = Convert.ToInt32(dr["FrecuenciaSemanal"]);
                        rut.DetalleEjercicios = dr["DetalleEjercicios"].ToString();
                        rut.DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : null;
                        rut.Socio = new SocioBE { ID_Socio = Convert.ToInt32(dr["ID_Socio"]), DNI = dr["DNI"].ToString() };
                        rut.Entrenador = new UsuarioBE { ID = Convert.ToInt32(dr["ID_Usuario"]) };
                        lista.Add(rut);
                    }
                }
            }
            return lista;
        }

        public void ActualizarRutina(RutinaEntrenamientoBE rutina)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UPDATE RutinaEntrenamiento SET ObjetivoFisico = @Obj, FrecuenciaSemanal = @Frec, DetalleEjercicios = @Det, DVH = @DVH WHERE CodigoRutina = @Id", cx);
                cmd.Parameters.AddWithValue("@Obj", rutina.ObjetivoFisico);
                cmd.Parameters.AddWithValue("@Frec", rutina.FrecuenciaSemanal);
                cmd.Parameters.AddWithValue("@Det", rutina.DetalleEjercicios);
                cmd.Parameters.AddWithValue("@DVH", (object)rutina.DVH ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", rutina.CodigoRutina);
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarRutina(int codigoRutina)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM RutinaEntrenamiento WHERE CodigoRutina = @Id", cx);
                cmd.Parameters.AddWithValue("@Id", codigoRutina);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
