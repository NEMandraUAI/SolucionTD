using System;
using System.Collections.Generic;
using BE;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class ProductoDAL
    {
        private ProductoBE MapearProducto(SqlDataReader dr)
        {
            return new ProductoBE
            {
                CodigoProducto = Convert.ToInt32(dr["CodigoProducto"]),
                CodigoSKU = dr["CodigoSKU"].ToString() ?? string.Empty,
                Nombre = dr["Nombre"].ToString() ?? string.Empty,
                Categoria = dr["Categoria"].ToString() ?? string.Empty,
                PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                CostoCompra = Convert.ToDecimal(dr["CostoCompra"]),
                StockActual = Convert.ToInt32(dr["StockActual"]),
                StockMinimo = Convert.ToInt32(dr["StockMinimo"]),
                TiempoReposicionDias = Convert.ToInt32(dr["TiempoReposicionDias"]),
                ObjetivoFisicoAfin = dr["ObjetivoFisicoAfin"] != DBNull.Value ? dr["ObjetivoFisicoAfin"].ToString() : "Todos",
                ObjetivoNutricionalAfin = dr["ObjetivoNutricionalAfin"] != DBNull.Value ? dr["ObjetivoNutricionalAfin"].ToString() : "Todos",
                Activo = Convert.ToBoolean(dr["Activo"]),
                DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : string.Empty
            };
        }

        public List<ProductoBE> LeerTodos()
        {
            List<ProductoBE> lista = new List<ProductoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT CodigoProducto, CodigoSKU, Nombre, Categoria, PrecioVenta, CostoCompra,
                                      StockActual, StockMinimo, TiempoReposicionDias, ObjetivoFisicoAfin,
                                      ObjetivoNutricionalAfin, Activo, DVH
                               FROM Producto
                               ORDER BY CodigoProducto ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(MapearProducto(dr));
                    }
                }
            }
            return lista;
        }

        public List<ProductoBE> ListarProductosActivos()
        {
            List<ProductoBE> lista = new List<ProductoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT CodigoProducto, CodigoSKU, Nombre, Categoria, PrecioVenta, CostoCompra,
                                      StockActual, StockMinimo, TiempoReposicionDias, ObjetivoFisicoAfin,
                                      ObjetivoNutricionalAfin, Activo, DVH
                               FROM Producto
                               WHERE Activo = 1
                               ORDER BY Categoria ASC, Nombre ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(MapearProducto(dr));
                    }
                }
            }
            return lista;
        }

        public ProductoBE ObtenerPorCodigo(int codigoProducto)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT CodigoProducto, CodigoSKU, Nombre, Categoria, PrecioVenta, CostoCompra,
                                      StockActual, StockMinimo, TiempoReposicionDias, ObjetivoFisicoAfin,
                                      ObjetivoNutricionalAfin, Activo, DVH
                               FROM Producto
                               WHERE CodigoProducto = @id";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@id", codigoProducto);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return MapearProducto(dr);
                        }
                    }
                }
            }
            return null;
        }

        public ProductoBE ObtenerPorSKU(string codigoSKU)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT CodigoProducto, CodigoSKU, Nombre, Categoria, PrecioVenta, CostoCompra,
                                      StockActual, StockMinimo, TiempoReposicionDias, ObjetivoFisicoAfin,
                                      ObjetivoNutricionalAfin, Activo, DVH
                               FROM Producto
                               WHERE CodigoSKU = @sku";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@sku", codigoSKU);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return MapearProducto(dr);
                        }
                    }
                }
            }
            return null;
        }

        public int InsertarProducto(ProductoBE p)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"INSERT INTO Producto
                               (CodigoSKU, Nombre, Categoria, PrecioVenta, CostoCompra, StockActual, StockMinimo,
                                TiempoReposicionDias, ObjetivoFisicoAfin, ObjetivoNutricionalAfin, Activo, DVH)
                               OUTPUT INSERTED.CodigoProducto
                               VALUES
                               (@sku, @nom, @cat, @pv, @cc, @stock, @min, @lead, @objFis, @objNut, @act, @dvh)";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@sku", p.CodigoSKU);
                    cmd.Parameters.AddWithValue("@nom", p.Nombre);
                    cmd.Parameters.AddWithValue("@cat", p.Categoria);
                    cmd.Parameters.AddWithValue("@pv", p.PrecioVenta);
                    cmd.Parameters.AddWithValue("@cc", p.CostoCompra);
                    cmd.Parameters.AddWithValue("@stock", p.StockActual);
                    cmd.Parameters.AddWithValue("@min", p.StockMinimo);
                    cmd.Parameters.AddWithValue("@lead", p.TiempoReposicionDias);
                    cmd.Parameters.AddWithValue("@objFis", !string.IsNullOrWhiteSpace(p.ObjetivoFisicoAfin) ? (object)p.ObjetivoFisicoAfin : DBNull.Value);
                    cmd.Parameters.AddWithValue("@objNut", !string.IsNullOrWhiteSpace(p.ObjetivoNutricionalAfin) ? (object)p.ObjetivoNutricionalAfin : DBNull.Value);
                    cmd.Parameters.AddWithValue("@act", p.Activo);
                    cmd.Parameters.AddWithValue("@dvh", !string.IsNullOrWhiteSpace(p.DVH) ? (object)p.DVH : DBNull.Value);
                    p.CodigoProducto = (int)cmd.ExecuteScalar();
                    return p.CodigoProducto;
                }
            }
        }

        public void ActualizarProducto(ProductoBE p)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"UPDATE Producto
                               SET CodigoSKU = @sku,
                                   Nombre = @nom,
                                   Categoria = @cat,
                                   PrecioVenta = @pv,
                                   CostoCompra = @cc,
                                   StockActual = @stock,
                                   StockMinimo = @min,
                                   TiempoReposicionDias = @lead,
                                   ObjetivoFisicoAfin = @objFis,
                                   ObjetivoNutricionalAfin = @objNut,
                                   Activo = @act,
                                   DVH = @dvh
                               WHERE CodigoProducto = @id";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@id", p.CodigoProducto);
                    cmd.Parameters.AddWithValue("@sku", p.CodigoSKU);
                    cmd.Parameters.AddWithValue("@nom", p.Nombre);
                    cmd.Parameters.AddWithValue("@cat", p.Categoria);
                    cmd.Parameters.AddWithValue("@pv", p.PrecioVenta);
                    cmd.Parameters.AddWithValue("@cc", p.CostoCompra);
                    cmd.Parameters.AddWithValue("@stock", p.StockActual);
                    cmd.Parameters.AddWithValue("@min", p.StockMinimo);
                    cmd.Parameters.AddWithValue("@lead", p.TiempoReposicionDias);
                    cmd.Parameters.AddWithValue("@objFis", !string.IsNullOrWhiteSpace(p.ObjetivoFisicoAfin) ? (object)p.ObjetivoFisicoAfin : DBNull.Value);
                    cmd.Parameters.AddWithValue("@objNut", !string.IsNullOrWhiteSpace(p.ObjetivoNutricionalAfin) ? (object)p.ObjetivoNutricionalAfin : DBNull.Value);
                    cmd.Parameters.AddWithValue("@act", p.Activo);
                    cmd.Parameters.AddWithValue("@dvh", !string.IsNullOrWhiteSpace(p.DVH) ? (object)p.DVH : DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarDVH(int codigoProducto, string dvh)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Producto SET DVH = @dvh WHERE CodigoProducto = @id", cx))
                {
                    cmd.Parameters.AddWithValue("@dvh", dvh);
                    cmd.Parameters.AddWithValue("@id", codigoProducto);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> ObtenerTodosLosDVH()
        {
            List<string> lista = new List<string>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT DVH FROM Producto ORDER BY CodigoProducto ASC", cx))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : string.Empty);
                    }
                }
            }
            return lista;
        }
    }
}