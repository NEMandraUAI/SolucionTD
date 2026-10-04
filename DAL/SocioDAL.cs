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
        public SocioBE ObtenerPorDNI(string dni)
        {
            SocioBE socio = null;
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("SELECT ID_Socio, DNI, Nombre, Apellido, Telefono, Email, EstadoActivo, CodigoPlan, DVH FROM Socio WHERE DNI = @DNI", cx);
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

        public void InsertarSocio(SocioBE socio)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Socio (DNI, Nombre, Apellido, Telefono, Email, EstadoActivo, DVH) OUTPUT INSERTED.ID_Socio VALUES (@DNI, @Nombre, @Apellido, @Telefono, @Email, 1, @DVH)", cx);
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
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Socio SET CodigoPlan = @CodigoPlan, EstadoActivo = @EstadoActivo, DVH = @DVH WHERE ID_Socio = @ID", cx);
                cmd.Parameters.AddWithValue("@ID", socio.ID_Socio);
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
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("SELECT DVH FROM Socio ORDER BY ID_Socio", cx);
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
            string query = "SELECT ID_Socio, DNI, Nombre, Apellido, Telefono, Email, EstadoActivo, CodigoPlan, DVH FROM Socio";
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cx))
                {
                    cx.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SocioBE socio = new SocioBE();
                            socio.ID_Socio = Convert.ToInt32(reader["ID_Socio"]);
                            socio.DNI = reader["DNI"].ToString();
                            socio.Nombre = reader["Nombre"].ToString();
                            socio.Apellido = reader["Apellido"].ToString();
                            socio.Telefono = reader["Telefono"].ToString();
                            socio.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null;
                            socio.EstadoActivo = Convert.ToBoolean(reader["EstadoActivo"]);
                            if (reader["CodigoPlan"] != DBNull.Value)
                            {
                                socio.PlanAsociado = new PlanSuscripcionBE { CodigoPlan = Convert.ToInt32(reader["CodigoPlan"]) };
                            }
                            socio.EstadoActivo = Convert.ToBoolean(reader["EstadoActivo"]);
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
