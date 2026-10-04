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
                string sql = @"SELECT p.CodigoPlan, p.FechaCreacion, p.EvaluacionAntropometrica, p.DetalleHabitos, 
                                      p.ID_Socio, p.ID_Nutricionista, p.DVH,
                                      s.DNI, s.Nombre as SocioNombre, s.Apellido as SocioApellido,
                                      u.Nombre as NutriNombre
                               FROM PlanNutricional p
                               INNER JOIN Socio s ON p.ID_Socio = s.ID_Socio
                               INNER JOIN Usuario u ON p.ID_Nutricionista = u.ID
                               WHERE p.ID_Socio = @idSocio 
                               ORDER BY p.FechaCreacion DESC";
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
                                DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : null,
                                Socio = new SocioBE
                                {
                                    ID_Socio = Convert.ToInt32(dr["ID_Socio"]),
                                    DNI = dr["DNI"].ToString(),
                                    Nombre = dr["SocioNombre"].ToString(),
                                    Apellido = dr["SocioApellido"].ToString()
                                },
                                Nutricionista = new UsuarioBE
                                {
                                    ID = Convert.ToInt32(dr["ID_Nutricionista"]),
                                    Nombre = dr["NutriNombre"].ToString()
                                }
                            };
                            planes.Add(plan);
                        }
                    }
                }
            }
            return planes;
        }
        public List<PlanNutricionalBE> LeerTodos()
        {
            List<PlanNutricionalBE> planes = new List<PlanNutricionalBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT CodigoPlan, FechaCreacion, EvaluacionAntropometrica, DetalleHabitos, ID_Socio, ID_Nutricionista, DVH FROM PlanNutricional";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
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
                                DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : null,
                                Socio = new SocioBE
                                {
                                    ID_Socio = Convert.ToInt32(dr["ID_Socio"]),
                                    DNI = dr["DNI"].ToString(),
                                    Nombre = dr["SocioNombre"].ToString(),
                                    Apellido = dr["SocioApellido"].ToString()
                                },
                                Nutricionista = new UsuarioBE
                                {
                                    ID = Convert.ToInt32(dr["ID_Nutricionista"]),
                                    Nombre = dr["NutriNombre"].ToString()
                                }
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
