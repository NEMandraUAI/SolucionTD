using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TurnoDAL
    {
        public int InsertarTurno(TurnoBE turno)
        {
            int idGenerado = 0;
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"INSERT INTO Turno (FechaHora, Estado, ID_Socio, ID_Nutricionista) 
                               VALUES (@fecha, @estado, @idSocio, @idNutri);
                               SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@fecha", turno.FechaHora);
                    cmd.Parameters.AddWithValue("@estado", turno.Estado);
                    cmd.Parameters.AddWithValue("@idSocio", turno.ID_Socio);
                    cmd.Parameters.AddWithValue("@idNutri", turno.ID_Nutricionista);
                    idGenerado = (int)cmd.ExecuteScalar();
                }
            }
            return idGenerado;
        }

        public void ActualizarDVH(int idTurno, string dvh)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = "UPDATE Turno SET DVH = @dvh WHERE NroTurno = @id";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@dvh", dvh);
                    cmd.Parameters.AddWithValue("@id", idTurno);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEstado(int idTurno, string estado, string nuevoDVH)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = "UPDATE Turno SET Estado = @estado, DVH = @dvh WHERE NroTurno = @id";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@dvh", nuevoDVH);
                    cmd.Parameters.AddWithValue("@id", idTurno);
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
                string sql = "SELECT DVH FROM Turno ORDER BY NroTurno ASC";
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

        public List<TurnoBE> ConsultarTurnosPorFecha(DateTime fecha)
        {
            List<TurnoBE> turnos = new List<TurnoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT t.NroTurno, t.FechaHora, t.Estado, t.ID_Socio, t.ID_Nutricionista, t.DVH, 
                                      s.DNI, s.Nombre as SocioNombre, s.Apellido as SocioApellido,
                                      u.Nombre as NutriNombre
                               FROM Turno t
                               INNER JOIN Socio s ON t.ID_Socio = s.ID_Socio
                               INNER JOIN Usuario u ON t.ID_Nutricionista = u.ID
                               WHERE CAST(t.FechaHora AS DATE) = CAST(@fecha AS DATE) 
                               ORDER BY t.FechaHora ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@fecha", fecha);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            TurnoBE turno = new TurnoBE
                            {
                                NroTurno = Convert.ToInt32(dr["NroTurno"]),
                                FechaHora = Convert.ToDateTime(dr["FechaHora"]),
                                Estado = dr["Estado"].ToString(),
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
                            turnos.Add(turno);
                        }
                    }
                }
            }
            return turnos;
        }

        public List<TurnoBE> ObtenerTurnosPorNutricionistaYFecha(int idNutricionista, DateTime fechaElegida)
        {
            List<TurnoBE> turnosDelDia = new List<TurnoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string query = "SELECT NroTurno, FechaHora FROM Turno WHERE ID_Nutricionista = @idNutricionista AND CAST(FechaHora AS DATE) = CAST(@fecha AS DATE)";
                SqlCommand cmd = new SqlCommand(query, cx);
                cmd.Parameters.AddWithValue("@idNutricionista", idNutricionista);
                cmd.Parameters.AddWithValue("@fecha", fechaElegida);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TurnoBE turno = new TurnoBE();
                    turno.NroTurno = Convert.ToInt32(reader["NroTurno"]);
                    turno.FechaHora = Convert.ToDateTime(reader["FechaHora"]);
                    turnosDelDia.Add(turno);
                }
                reader.Close();
            }
            return turnosDelDia;
        }

        public List<TurnoBE> ObtenerTurnosPendientesPorSocio(int idSocio)
        {
            List<TurnoBE> listaPendientes = new List<TurnoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string query = "SELECT NroTurno, FechaHora, Estado, ID_Socio, ID_Nutricionista FROM Turno WHERE ID_Socio = @idSocio AND Estado = 'Pendiente'";
                SqlCommand cmd = new SqlCommand(query, cx);
                cmd.Parameters.AddWithValue("@idSocio", idSocio);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TurnoBE turno = new TurnoBE();
                    turno.NroTurno = Convert.ToInt32(reader["NroTurno"]);
                    turno.FechaHora = Convert.ToDateTime(reader["FechaHora"]);
                    turno.Estado = reader["Estado"].ToString();
                    turno.ID_Socio = int.Parse(reader["ID_Socio"].ToString());
                    turno.ID_Nutricionista = int.Parse(reader["ID_Nutricionista"].ToString());
                    listaPendientes.Add(turno);
                }
                reader.Close();
            }
            return listaPendientes;
        }
        public List<TurnoBE> LeerTodos()
        {
            List<TurnoBE> turnos = new List<TurnoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string query = "SELECT NroTurno, FechaHora, Estado, ID_Socio, ID_Nutricionista, DVH FROM Turno";
                SqlCommand cmd = new SqlCommand(query, cx);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TurnoBE turno = new TurnoBE();
                    turno.NroTurno = Convert.ToInt32(reader["NroTurno"]);
                    turno.FechaHora = Convert.ToDateTime(reader["FechaHora"]);
                    turno.Estado = reader["Estado"].ToString();
                    turno.ID_Socio = int.Parse(reader["ID_Socio"].ToString());
                    turno.ID_Nutricionista = int.Parse(reader["ID_Nutricionista"].ToString());
                    turno.DVH = reader["DVH"].ToString();
                    turnos.Add(turno);
                }
                reader.Close();
            }
            return turnos;
        }
    }
}
