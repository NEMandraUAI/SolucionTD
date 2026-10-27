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
        private readonly UnidadTrabajoSeguridadDAL? unidad;
        public TurnoDAL(UnidadTrabajoSeguridadDAL? unidad = null) { this.unidad = unidad; }

        public int InsertarTurno(TurnoBE turno)
        {
            int idGenerado = 0;
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string sql = @"INSERT INTO Turno (FechaHora, Estado, ID_Socio, ID_Nutricionista) 
                               VALUES (@fecha, @estado, @idSocio, @idNutri);
                               SELECT CAST(SCOPE_IDENTITY() AS INT);";
                using (SqlCommand cmd = acceso.CrearComando(sql))
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
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string sql = "UPDATE Turno SET DVH = @dvh WHERE NroTurno = @id";
                using (SqlCommand cmd = acceso.CrearComando(sql))
                {
                    cmd.Parameters.AddWithValue("@dvh", dvh);
                    cmd.Parameters.AddWithValue("@id", idTurno);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarEstado(int idTurno, string estado, string nuevoDVH, string estadoEsperado = "Pendiente")
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string sql = "UPDATE Turno SET Estado = @estado, DVH = @dvh WHERE NroTurno = @id AND Estado = @esperado";
                using (SqlCommand cmd = acceso.CrearComando(sql))
                {
                    cmd.Parameters.AddWithValue("@estado", estado);
                    cmd.Parameters.AddWithValue("@dvh", nuevoDVH);
                    cmd.Parameters.AddWithValue("@id", idTurno);
                    cmd.Parameters.AddWithValue("@esperado", estadoEsperado);
                    if (cmd.ExecuteNonQuery() != 1)
                        throw new InvalidOperationException("El turno ya cambió de estado o no existe. Actualice la agenda.");
                }
            }
        }

        public List<string> ObtenerTodosLosDVH()
        {
            List<string> listaDVH = new List<string>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string sql = "SELECT DVH FROM Turno ORDER BY NroTurno ASC";
                using (SqlCommand cmd = acceso.CrearComando(sql))
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

        public TurnoBE? ObtenerPorID(int id)
        {
            using var acceso = new AccesoSeguridadDAL(unidad);
            using var cmd = acceso.CrearComando("SELECT NroTurno, FechaHora, Estado, ID_Socio, ID_Nutricionista, DVH FROM dbo.Turno WHERE NroTurno=@id");
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;
            return new TurnoBE { NroTurno=reader.GetInt32(0), FechaHora=reader.GetDateTime(1), Estado=reader.GetString(2),
                ID_Socio=reader.GetInt32(3), ID_Nutricionista=reader.GetInt32(4), DVH=reader.IsDBNull(5)?string.Empty:reader.GetString(5) };
        }

        public bool ExisteConflicto(int profesional, DateTime fecha)
        {
            using var acceso = new AccesoSeguridadDAL(unidad);
            using var cmd = acceso.CrearComando("SELECT COUNT(*) FROM dbo.Turno WHERE ID_Nutricionista=@id AND Estado<>N'Cancelado' AND FechaHora>@desde AND FechaHora<@hasta");
            cmd.Parameters.AddWithValue("@id", profesional);
            cmd.Parameters.AddWithValue("@desde", fecha.AddMinutes(-30));
            cmd.Parameters.AddWithValue("@hasta", fecha.AddMinutes(30));
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public List<TurnoBE> ConsultarTurnosPorFecha(DateTime fecha)
        {
            List<TurnoBE> turnos = new List<TurnoBE>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string sql = @"SELECT t.NroTurno, t.FechaHora, t.Estado, t.ID_Socio, t.ID_Nutricionista, t.DVH, 
                                      s.DNI, s.Nombre as SocioNombre, s.Apellido as SocioApellido,
                                      u.Nombre as NutriNombre
                               FROM Turno t
                               INNER JOIN Socio s ON t.ID_Socio = s.ID_Socio
                               INNER JOIN Usuario u ON t.ID_Nutricionista = u.ID
                               WHERE CAST(t.FechaHora AS DATE) = CAST(@fecha AS DATE) 
                               ORDER BY t.FechaHora ASC";
                using (SqlCommand cmd = acceso.CrearComando(sql))
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
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string query = "SELECT NroTurno, FechaHora FROM Turno WHERE ID_Nutricionista = @idNutricionista AND CAST(FechaHora AS DATE) = CAST(@fecha AS DATE)";
                SqlCommand cmd = acceso.CrearComando(query);
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
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string query = @"SELECT t.NroTurno, t.FechaHora, t.Estado, t.ID_Socio, t.ID_Nutricionista, t.DVH,
                                        s.DNI, s.Nombre AS SocioNombre, s.Apellido AS SocioApellido,
                                        u.Nombre AS NutriNombre
                                 FROM Turno t
                                 INNER JOIN Socio s ON t.ID_Socio = s.ID_Socio
                                 INNER JOIN Usuario u ON t.ID_Nutricionista = u.ID
                                 WHERE t.ID_Socio = @idSocio AND t.Estado = 'Pendiente'
                                 ORDER BY t.FechaHora ASC";
                using (SqlCommand cmd = acceso.CrearComando(query))
                {
                    cmd.Parameters.AddWithValue("@idSocio", idSocio);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TurnoBE turno = new TurnoBE
                            {
                                NroTurno = Convert.ToInt32(reader["NroTurno"]),
                                FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                                Estado = reader["Estado"].ToString(),
                                ID_Socio = Convert.ToInt32(reader["ID_Socio"]),
                                ID_Nutricionista = Convert.ToInt32(reader["ID_Nutricionista"]),
                                DVH = reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : null,
                                Socio = new SocioBE
                                {
                                    ID_Socio = Convert.ToInt32(reader["ID_Socio"]),
                                    DNI = reader["DNI"].ToString(),
                                    Nombre = reader["SocioNombre"].ToString(),
                                    Apellido = reader["SocioApellido"].ToString()
                                },
                                Nutricionista = new UsuarioBE
                                {
                                    ID = Convert.ToInt32(reader["ID_Nutricionista"]),
                                    Nombre = reader["NutriNombre"].ToString()
                                }
                            };
                            listaPendientes.Add(turno);
                        }
                    }
                }
            }
            return listaPendientes;
        }

        public List<TurnoBE> LeerTodos()
        {
            List<TurnoBE> turnos = new List<TurnoBE>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                string query = "SELECT NroTurno, FechaHora, Estado, ID_Socio, ID_Nutricionista, DVH FROM Turno ORDER BY NroTurno ASC";
                SqlCommand cmd = acceso.CrearComando(query);
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
