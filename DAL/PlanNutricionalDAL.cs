using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PlanNutricionalDAL
    {
        public int InsertarPlan(PlanNutricionalBE plan)
        {
            int idGenerado = 0;
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"INSERT INTO PlanNutricional (FechaCreacion, EvaluacionAntropometrica, DetalleHabitos, ID_Socio, ID_Nutricionista) 
                               VALUES (@fecha, @evaluacion, @habitos, @idSocio, @idNutri);
                               SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@fecha", plan.FechaCreacion);
                    cmd.Parameters.AddWithValue("@evaluacion", plan.EvaluacionAntropometrica);
                    cmd.Parameters.AddWithValue("@habitos", plan.DetalleHabitos);
                    cmd.Parameters.AddWithValue("@idSocio", plan.ID_Socio);
                    cmd.Parameters.AddWithValue("@idNutri", plan.ID_Nutricionista);
                    idGenerado = (int)cmd.ExecuteScalar();
                }
            }
            return idGenerado;
        }

        public void ActualizarDVH(int codigoPlan, string dvh)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = "UPDATE PlanNutricional SET DVH = @dvh WHERE CodigoPlan = @id";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@dvh", dvh);
                    cmd.Parameters.AddWithValue("@id", codigoPlan);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> ObtenerTodosLosDVH()
        {
            List<string> listaDVH = new List<string>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = "SELECT DVH FROM PlanNutricional ORDER BY CodigoPlan ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaDVH.Add(dr["DVH"].ToString());
                        }
                    }
                }
            }
            return listaDVH;
        }

        public List<PlanNutricionalBE> ConsultarPlanesPorSocio(int idSocio)
        {
            List<PlanNutricionalBE> planes = new List<PlanNutricionalBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = "SELECT * FROM PlanNutricional WHERE ID_Socio = @idSocio ORDER BY FechaCreacion DESC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@idSocio", idSocio);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            PlanNutricionalBE plan = new PlanNutricionalBE
                            {
                                CodigoPlan = Convert.ToInt32(dr["CodigoPlan"]),
                                FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                                EvaluacionAntropometrica = dr["EvaluacionAntropometrica"].ToString(),
                                DetalleHabitos = dr["DetalleHabitos"].ToString(),
                                ID_Socio = Convert.ToInt32(dr["ID_Socio"]),
                                ID_Nutricionista = Convert.ToInt32(dr["ID_Nutricionista"]),
                                DVH = dr["DVH"].ToString()
                            };
                            planes.Add(plan);
                        }
                    }
                }
            }
            return planes;
        }
    }
}
