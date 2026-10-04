using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class IntegridadDAL
    {
        public List<UsuarioBE> LeerTodosLosUsuariosCrudo()
        {
            List<UsuarioBE> lista = new List<UsuarioBE>();
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = "SELECT ID, Nombre, Clave, IntentosFallidos, Bloqueado, DVH, NivelJerarquia FROM Usuario ORDER BY ID";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new UsuarioBE
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Nombre = reader["Nombre"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]),
                                Bloqueado = Convert.ToBoolean(reader["Bloqueado"]),
                                DVH = reader["DVH"] != DBNull.Value ? reader["DVH"].ToString() : null,
                                NivelJerarquia = Convert.ToInt32(reader["NivelJerarquia"])
                            });
                        }
                    }
                }
            }
            return lista;
        }
        public string ObtenerDVV(string entidad)
        {
            string dvv = null;
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = "SELECT DVV FROM DigitoVerificador WHERE Entidad = @Entidad";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@Entidad", entidad);
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        dvv = result.ToString();
                    }
                }
            }
            return dvv;
        }
        public void ActualizarDVV(string entidad, string nuevoDVV)
        {
            using (SqlConnection cn = ConexionDAL.Instancia.ObtenerConexion())
            {
                cn.Open();
                string consulta = @"
                    IF EXISTS (SELECT 1 FROM DigitoVerificador WHERE Entidad = @Entidad)
                        UPDATE DigitoVerificador SET DVV = @DVV WHERE Entidad = @Entidad
                    ELSE
                        INSERT INTO DigitoVerificador (Entidad, DVV) VALUES (@Entidad, @DVV)";
                using (SqlCommand cmd = new SqlCommand(consulta, cn))
                {
                    cmd.Parameters.AddWithValue("@Entidad", entidad);
                    cmd.Parameters.AddWithValue("@DVV", nuevoDVV);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void ActualizarDVHRegistro(string nombreTabla, int id, string nuevoDVH)
        {
            string columnaPK = ObtenerNombrePK(nombreTabla);
            string query = $"UPDATE {nombreTabla} SET DVH = @DVH WHERE {columnaPK} = @ID";
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                using (SqlCommand cmd = new SqlCommand(query, cx))
                {
                    cmd.Parameters.AddWithValue("@DVH", nuevoDVH);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cx.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private string ObtenerNombrePK(string tabla)
        {
            switch (tabla)
            {
                case "Usuario": return "ID";
                case "Socio": return "ID_Socio";
                case "Turno": return "NroTurno";
                case "ComprobantePago": return "NroComprobante";
                case "PlanNutricional": return "CodigoPlan";
                case "PlanSuscripcion": return "CodigoPlan";
                case "RutinaEntrenamiento": return "CodigoRutina";
                default: throw new Exception("Tabla no configurada para DVH.");
            }
        }
    }
}
