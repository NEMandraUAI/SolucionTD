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
    }
}
