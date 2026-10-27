using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SocioDAL
    {
        private readonly UnidadTrabajoSeguridadDAL? unidad;
        public SocioDAL(UnidadTrabajoSeguridadDAL? unidad = null) { this.unidad = unidad; }
        public SocioBE ObtenerPorDNI(string dni)
        {
            SocioBE socio = null;
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                using SqlCommand cmd = acceso.CrearComando("SELECT ID_Socio, DNI, Nombre, Apellido, Telefono, Email, EstadoActivo, CodigoPlan, DVH FROM Socio WHERE DNI = @DNI");
                cmd.Parameters.AddWithValue("@DNI", dni);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        socio = new SocioBE();
                        socio.ID_Socio = Convert.ToInt32(dr["ID_Socio"]);
                        socio.DNI = dr["DNI"].ToString();
                        socio.Nombre = dr["Nombre"].ToString();
                        socio.Apellido = dr["Apellido"].ToString();
                        socio.Telefono = dr["Telefono"] != DBNull.Value ? dr["Telefono"].ToString() : null;
                        socio.Email = dr["Email"] != DBNull.Value ? dr["Email"].ToString() : null;
                        socio.EstadoActivo = Convert.ToBoolean(dr["EstadoActivo"]);
                        if (dr["CodigoPlan"] != DBNull.Value)
                        {
                            socio.PlanAsociado = new PlanSuscripcionBE { CodigoPlan = Convert.ToInt32(dr["CodigoPlan"]) };
                        }
                        socio.DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : null;
                    }
                }
            }
            return socio;
        }

        public SocioBE? ObtenerPorID(int id)
        {
            using var acceso = new AccesoSeguridadDAL(unidad);
            using var cmd = acceso.CrearComando("SELECT ID_Socio,DNI,Nombre,Apellido,Telefono,Email,EstadoActivo,CodigoPlan,DVH FROM dbo.Socio WHERE ID_Socio=@id");
            cmd.Parameters.AddWithValue("@id", id);
            using var dr = cmd.ExecuteReader();
            if (!dr.Read()) return null;
            return new SocioBE
            {
                ID_Socio = dr.GetInt32(0), DNI = dr.GetString(1), Nombre = dr.GetString(2), Apellido = dr.GetString(3),
                Telefono = dr.IsDBNull(4) ? null : dr.GetString(4), Email = dr.IsDBNull(5) ? null : dr.GetString(5),
                EstadoActivo = dr.GetBoolean(6), PlanAsociado = dr.IsDBNull(7) ? null : new PlanSuscripcionBE { CodigoPlan = dr.GetInt32(7) },
                DVH = dr.IsDBNull(8) ? null : dr.GetString(8)
            };
        }

        public void InsertarSocio(SocioBE socio)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                using SqlCommand cmd = acceso.CrearComando("INSERT INTO Socio (DNI, Nombre, Apellido, Telefono, Email, EstadoActivo, DVH) VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email, 1, @DVH); SELECT CONVERT(int,SCOPE_IDENTITY());");
                cmd.Parameters.AddWithValue("@DNI", socio.DNI);
                cmd.Parameters.AddWithValue("@Nombre", socio.Nombre);
                cmd.Parameters.AddWithValue("@Apellido", socio.Apellido);
                cmd.Parameters.AddWithValue("@Telefono", (object)socio.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)socio.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DVH", (object)socio.DVH ?? DBNull.Value);
                socio.ID_Socio = (int)cmd.ExecuteScalar();
            }
        }

        public void ActualizarSocio(SocioBE socio)
        {
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                using SqlCommand cmd = acceso.CrearComando("UPDATE Socio SET Telefono = @Telefono, Email = @Email, CodigoPlan = @CodigoPlan, EstadoActivo = @EstadoActivo, DVH = @DVH WHERE ID_Socio = @ID");
                cmd.Parameters.AddWithValue("@ID", socio.ID_Socio);
                cmd.Parameters.AddWithValue("@Telefono", !string.IsNullOrWhiteSpace(socio.Telefono) ? (object)socio.Telefono : DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", !string.IsNullOrWhiteSpace(socio.Email) ? (object)socio.Email : DBNull.Value);
                cmd.Parameters.AddWithValue("@EstadoActivo", socio.EstadoActivo);
                cmd.Parameters.AddWithValue("@DVH", (object)socio.DVH ?? DBNull.Value);
                if (socio.PlanAsociado != null && socio.PlanAsociado.CodigoPlan > 0)
                    cmd.Parameters.AddWithValue("@CodigoPlan", socio.PlanAsociado.CodigoPlan);
                else
                    cmd.Parameters.AddWithValue("@CodigoPlan", DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public List<string> ObtenerTodosLosDVH()
        {
            List<string> lista = new List<string>();
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                using SqlCommand cmd = acceso.CrearComando("SELECT DVH FROM Socio ORDER BY ID_Socio");
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
        public List<SocioBE> LeerTodos()
        {
            List<SocioBE> lista = new List<SocioBE>();
            string query = "SELECT ID_Socio, DNI, Nombre, Apellido, Telefono, Email, EstadoActivo, CodigoPlan, DVH FROM Socio ORDER BY ID_Socio ASC";
            using (var acceso = new AccesoSeguridadDAL(unidad))
            {
                SqlConnection cx = acceso.Conexion;
                using (SqlCommand cmd = acceso.CrearComando(query))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SocioBE socio = new SocioBE();
                            socio.ID_Socio = Convert.ToInt32(reader["ID_Socio"]);
                            socio.DNI = reader["DNI"].ToString();
                            socio.Nombre = reader["Nombre"].ToString();
                            socio.Apellido = reader["Apellido"].ToString();
                            socio.Telefono = reader["Telefono"] != DBNull.Value ? reader["Telefono"].ToString() : null;
                            socio.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null;
                            socio.EstadoActivo = Convert.ToBoolean(reader["EstadoActivo"]);
                            if (reader["CodigoPlan"] != DBNull.Value)
                            {
                                socio.PlanAsociado = new PlanSuscripcionBE { CodigoPlan = Convert.ToInt32(reader["CodigoPlan"]) };
                            }
                            socio.DVH = reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : null;
                            lista.Add(socio);
                        }
                    }
                }
            }
            return lista;
        }
    }
}
