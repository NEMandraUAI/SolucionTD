using BE;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ComprobantePagoDAL
    {
        public void InsertarComprobante(ComprobantePagoBE comprobante)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO ComprobantePago (Fecha, MontoTotal, MetodoPago, ID_Socio, ID_Usuario, DVH) OUTPUT INSERTED.NroComprobante VALUES (@Fecha, @MontoTotal, @MetodoPago, @ID_Socio, @ID_Usuario, @DVH)", cx);
                cmd.Parameters.AddWithValue("@Fecha", comprobante.Fecha);
                cmd.Parameters.AddWithValue("@MontoTotal", comprobante.MontoTotal);
                cmd.Parameters.AddWithValue("@MetodoPago", comprobante.MetodoPago);
                cmd.Parameters.AddWithValue("@ID_Socio", comprobante.Socio.ID_Socio);
                cmd.Parameters.AddWithValue("@ID_Usuario", comprobante.EmpleadoCobrador.ID);
                cmd.Parameters.AddWithValue("@DVH", (object)comprobante.DVH ?? DBNull.Value);
                comprobante.NroComprobante = (int)cmd.ExecuteScalar();
            }
        }

        public List<string> ObtenerTodosLosDVH()
        {
            List<string> lista = new List<string>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                SqlCommand cmd = new SqlCommand("SELECT DVH FROM ComprobantePago ORDER BY NroComprobante", cx);
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

        public List<ComprobantePagoBE> ConsultarComprobantesFiltros(DateTime desde, DateTime hasta, string dniSocio)
        {
            List<ComprobantePagoBE> lista = new List<ComprobantePagoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string query = @"SELECT c.NroComprobante, c.Fecha, c.MontoTotal, c.MetodoPago, 
                                s.ID_Socio, s.DNI, s.Nombre as NombreSocio, s.Apellido as ApellidoSocio, 
                                u.ID as ID_Usuario, u.Nombre as NombreUsuario 
                         FROM ComprobantePago c 
                         INNER JOIN Socio s ON c.ID_Socio = s.ID_Socio 
                         INNER JOIN Usuario u ON c.ID_Usuario = u.ID 
                         WHERE c.Fecha >= @Desde AND c.Fecha <= @Hasta";
                if (!string.IsNullOrWhiteSpace(dniSocio))
                {
                    query += " AND s.DNI LIKE @DNI";
                }
                SqlCommand cmd = new SqlCommand(query, cx);
                cmd.Parameters.AddWithValue("@Desde", desde.Date);
                cmd.Parameters.AddWithValue("@Hasta", hasta.Date.AddDays(1).AddSeconds(-1));
                if (!string.IsNullOrWhiteSpace(dniSocio))
                {
                    cmd.Parameters.AddWithValue("@DNI", "%" + dniSocio + "%");
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ComprobantePagoBE comp = new ComprobantePagoBE();
                        comp.NroComprobante = Convert.ToInt32(dr["NroComprobante"]);
                        comp.Fecha = Convert.ToDateTime(dr["Fecha"]);
                        comp.MontoTotal = Convert.ToDecimal(dr["MontoTotal"]);
                        comp.MetodoPago = dr["MetodoPago"].ToString();
                        comp.Socio = new SocioBE();
                        comp.Socio.ID_Socio = Convert.ToInt32(dr["ID_Socio"]);
                        comp.Socio.DNI = dr["DNI"].ToString();
                        comp.Socio.Nombre = dr["NombreSocio"].ToString();
                        comp.Socio.Apellido = dr["ApellidoSocio"].ToString();
                        comp.EmpleadoCobrador = new UsuarioBE();
                        comp.EmpleadoCobrador.ID = Convert.ToInt32(dr["ID_Usuario"]);
                        comp.EmpleadoCobrador.Nombre = dr["NombreUsuario"].ToString();
                        lista.Add(comp);
                    }
                }
            }
            return lista;
        }
    }
}
