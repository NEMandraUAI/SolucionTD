using System;
using System.Collections.Generic;
using BE;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class MercadoInternoDAL
    {
        public void RegistrarVentaProductoTransaccional(VentaProductoBE venta, Func<object, string> calcularDVH)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                using (SqlTransaction tx = cx.BeginTransaction())
                {
                    try
                    {
                        foreach (var det in venta.Detalles)
                        {
                            ProductoBE prod = null;
                            string sqlProd = @"SELECT CodigoProducto, CodigoSKU, Nombre, Categoria, PrecioVenta, CostoCompra,
                                                      StockActual, StockMinimo, TiempoReposicionDias, ObjetivoFisicoAfin,
                                                      ObjetivoNutricionalAfin, Activo, DVH
                                               FROM Producto WITH (UPDLOCK, ROWLOCK)
                                               WHERE CodigoProducto = @idProd";
                            using (SqlCommand cmdGet = new SqlCommand(sqlProd, cx, tx))
                            {
                                cmdGet.Parameters.AddWithValue("@idProd", det.CodigoProducto);
                                using (SqlDataReader dr = cmdGet.ExecuteReader())
                                {
                                    if (dr.Read())
                                    {
                                        prod = new ProductoBE
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
                                            Activo = Convert.ToBoolean(dr["Activo"])
                                        };
                                    }
                                }
                            }
                            if (prod == null || !prod.Activo)
                                throw new Exception($"El producto con código #{det.CodigoProducto} no existe o se encuentra inactivo.");
                            if (prod.StockActual < det.Cantidad)
                                throw new Exception($"Stock insuficiente para '{prod.Nombre}' (Disponible: {prod.StockActual} u. | Solicitado: {det.Cantidad} u.).");
                            det.PrecioUnitario = prod.PrecioVenta;
                            det.CostoUnitario = prod.CostoCompra;
                            det.Subtotal = Math.Round(prod.PrecioVenta * det.Cantidad, 2);
                            prod.StockActual -= det.Cantidad;
                            prod.DVH = calcularDVH(prod);
                            string sqlUpdStock = "UPDATE Producto SET StockActual = @nuevoStock, DVH = @dvh WHERE CodigoProducto = @idProd";
                            using (SqlCommand cmdUpd = new SqlCommand(sqlUpdStock, cx, tx))
                            {
                                cmdUpd.Parameters.AddWithValue("@nuevoStock", prod.StockActual);
                                cmdUpd.Parameters.AddWithValue("@dvh", prod.DVH);
                                cmdUpd.Parameters.AddWithValue("@idProd", prod.CodigoProducto);
                                cmdUpd.ExecuteNonQuery();
                            }
                        }
                        string sqlInsVenta = @"INSERT INTO VentaProducto (Fecha, MontoTotal, CostoTotalEstimado, MetodoPago, ID_Socio, ID_Usuario, DVH)
                                               OUTPUT INSERTED.NroVenta
                                               VALUES (@fecha, @monto, @costo, @metodo, @idSocio, @idUsuario, NULL)";
                        using (SqlCommand cmdVenta = new SqlCommand(sqlInsVenta, cx, tx))
                        {
                            cmdVenta.Parameters.AddWithValue("@fecha", venta.Fecha);
                            cmdVenta.Parameters.AddWithValue("@monto", venta.MontoTotal);
                            cmdVenta.Parameters.AddWithValue("@costo", venta.CostoTotalEstimado);
                            cmdVenta.Parameters.AddWithValue("@metodo", venta.MetodoPago);
                            cmdVenta.Parameters.AddWithValue("@idSocio", venta.ID_SocioParaDVH);
                            cmdVenta.Parameters.AddWithValue("@idUsuario", venta.ID_UsuarioParaDVH);
                            venta.NroVenta = (int)cmdVenta.ExecuteScalar();
                        }
                        venta.DVH = calcularDVH(venta);
                        using (SqlCommand cmdDvhVenta = new SqlCommand("UPDATE VentaProducto SET DVH = @dvh WHERE NroVenta = @nro", cx, tx))
                        {
                            cmdDvhVenta.Parameters.AddWithValue("@dvh", venta.DVH);
                            cmdDvhVenta.Parameters.AddWithValue("@nro", venta.NroVenta);
                            cmdDvhVenta.ExecuteNonQuery();
                        }
                        string sqlInsDet = @"INSERT INTO DetalleVentaProducto (NroVenta, CodigoProducto, Cantidad, PrecioUnitario, CostoUnitario, Subtotal)
                                             VALUES (@nroVenta, @codProd, @cant, @precio, @costo, @sub)";
                        foreach (var det in venta.Detalles)
                        {
                            det.NroVenta = venta.NroVenta;
                            using (SqlCommand cmdDet = new SqlCommand(sqlInsDet, cx, tx))
                            {
                                cmdDet.Parameters.AddWithValue("@nroVenta", venta.NroVenta);
                                cmdDet.Parameters.AddWithValue("@codProd", det.CodigoProducto);
                                cmdDet.Parameters.AddWithValue("@cant", det.Cantidad);
                                cmdDet.Parameters.AddWithValue("@precio", det.PrecioUnitario);
                                cmdDet.Parameters.AddWithValue("@costo", det.CostoUnitario);
                                cmdDet.Parameters.AddWithValue("@sub", det.Subtotal);
                                cmdDet.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<VentaProductoBE> ConsultarVentasFiltros(DateTime desde, DateTime hasta, string dniSocio, string metodoPago)
        {
            List<VentaProductoBE> lista = new List<VentaProductoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT v.NroVenta, v.Fecha, v.MontoTotal, v.CostoTotalEstimado, v.MetodoPago,
                                      v.ID_Socio, v.ID_Usuario, v.DVH,
                                      s.DNI, s.Nombre AS SocioNombre, s.Apellido AS SocioApellido,
                                      u.Nombre AS VendedorNombre
                               FROM VentaProducto v
                               INNER JOIN Socio s ON v.ID_Socio = s.ID_Socio
                               INNER JOIN Usuario u ON v.ID_Usuario = u.ID
                               WHERE v.Fecha >= @desde AND v.Fecha <= @hasta";
                if (!string.IsNullOrWhiteSpace(dniSocio))
                    sql += " AND s.DNI LIKE @dni";
                if (!string.IsNullOrWhiteSpace(metodoPago) && metodoPago != "Todos")
                    sql += " AND v.MetodoPago = @metodo";
                sql += " ORDER BY v.Fecha DESC, v.NroVenta DESC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@desde", desde.Date);
                    cmd.Parameters.AddWithValue("@hasta", hasta.Date.AddDays(1).AddSeconds(-1));
                    if (!string.IsNullOrWhiteSpace(dniSocio))
                        cmd.Parameters.AddWithValue("@dni", "%" + dniSocio.Trim() + "%");
                    if (!string.IsNullOrWhiteSpace(metodoPago) && metodoPago != "Todos")
                        cmd.Parameters.AddWithValue("@metodo", metodoPago);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int idSoc = Convert.ToInt32(dr["ID_Socio"]);
                            int idUsu = Convert.ToInt32(dr["ID_Usuario"]);
                            lista.Add(new VentaProductoBE
                            {
                                NroVenta = Convert.ToInt32(dr["NroVenta"]),
                                Fecha = Convert.ToDateTime(dr["Fecha"]),
                                MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                                CostoTotalEstimado = Convert.ToDecimal(dr["CostoTotalEstimado"]),
                                MetodoPago = dr["MetodoPago"].ToString() ?? string.Empty,
                                ID_Socio = idSoc,
                                ID_Usuario = idUsu,
                                DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : string.Empty,
                                Socio = new SocioBE
                                {
                                    ID_Socio = idSoc,
                                    DNI = dr["DNI"].ToString() ?? string.Empty,
                                    Nombre = dr["SocioNombre"].ToString() ?? string.Empty,
                                    Apellido = dr["SocioApellido"].ToString() ?? string.Empty
                                },
                                Vendedor = new UsuarioBE
                                {
                                    ID = idUsu,
                                    Nombre = dr["VendedorNombre"].ToString() ?? string.Empty
                                }
                            });
                        }
                    }
                }
                foreach (var v in lista)
                {
                    v.Detalles = ObtenerDetallesDeVentaInterno(cx, v.NroVenta);
                }
            }
            return lista;
        }

        public List<DetalleVentaProductoBE> ObtenerDetallesDeVenta(int nroVenta)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                return ObtenerDetallesDeVentaInterno(cx, nroVenta);
            }
        }

        private List<DetalleVentaProductoBE> ObtenerDetallesDeVentaInterno(SqlConnection cx, int nroVenta)
        {
            List<DetalleVentaProductoBE> detalles = new List<DetalleVentaProductoBE>();
            string sql = @"SELECT d.ID_DetalleVenta, d.NroVenta, d.CodigoProducto, d.Cantidad,
                                  d.PrecioUnitario, d.CostoUnitario, d.Subtotal,
                                  p.CodigoSKU, p.Nombre AS NombreProducto, p.Categoria
                           FROM DetalleVentaProducto d
                           INNER JOIN Producto p ON d.CodigoProducto = p.CodigoProducto
                           WHERE d.NroVenta = @nro
                           ORDER BY d.ID_DetalleVenta ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cx))
            {
                cmd.Parameters.AddWithValue("@nro", nroVenta);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        detalles.Add(new DetalleVentaProductoBE
                        {
                            ID_DetalleVenta = Convert.ToInt32(dr["ID_DetalleVenta"]),
                            NroVenta = Convert.ToInt32(dr["NroVenta"]),
                            CodigoProducto = Convert.ToInt32(dr["CodigoProducto"]),
                            CodigoSKU = dr["CodigoSKU"].ToString() ?? string.Empty,
                            NombreProducto = dr["NombreProducto"].ToString() ?? string.Empty,
                            Categoria = dr["Categoria"].ToString() ?? string.Empty,
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            PrecioUnitario = Convert.ToDecimal(dr["PrecioUnitario"]),
                            CostoUnitario = Convert.ToDecimal(dr["CostoUnitario"]),
                            Subtotal = Convert.ToDecimal(dr["Subtotal"])
                        });
                    }
                }
            }
            return detalles;
        }

        public List<VentaProductoBE> LeerTodasLasVentas()
        {
            List<VentaProductoBE> lista = new List<VentaProductoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT NroVenta, Fecha, MontoTotal, CostoTotalEstimado, MetodoPago, ID_Socio, ID_Usuario, DVH
                               FROM VentaProducto
                               ORDER BY NroVenta ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int idSoc = Convert.ToInt32(dr["ID_Socio"]);
                        int idUsu = Convert.ToInt32(dr["ID_Usuario"]);
                        lista.Add(new VentaProductoBE
                        {
                            NroVenta = Convert.ToInt32(dr["NroVenta"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            MontoTotal = Convert.ToDecimal(dr["MontoTotal"]),
                            CostoTotalEstimado = Convert.ToDecimal(dr["CostoTotalEstimado"]),
                            MetodoPago = dr["MetodoPago"].ToString() ?? string.Empty,
                            ID_Socio = idSoc,
                            ID_Usuario = idUsu,
                            Socio = new SocioBE { ID_Socio = idSoc },
                            Vendedor = new UsuarioBE { ID = idUsu },
                            DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : string.Empty
                        });
                    }
                }
            }
            return lista;
        }

        public List<string> ObtenerTodosLosDVHVentas()
        {
            List<string> lista = new List<string>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT DVH FROM VentaProducto ORDER BY NroVenta ASC", cx))
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

        public List<(int CodigoProducto, int Anio, int Mes, int CantidadVendida, decimal MontoVendido, decimal CostoVendido)> ObtenerVentasMensualesPorProducto(DateTime desde, DateTime hasta)
        {
            var resultado = new List<(int, int, int, int, decimal, decimal)>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT d.CodigoProducto,
                                      YEAR(v.Fecha) AS Anio,
                                      MONTH(v.Fecha) AS Mes,
                                      SUM(d.Cantidad) AS CantidadVendida,
                                      SUM(d.Subtotal) AS MontoVendido,
                                      SUM(d.Cantidad * d.CostoUnitario) AS CostoVendido
                               FROM DetalleVentaProducto d
                               INNER JOIN VentaProducto v ON d.NroVenta = v.NroVenta
                               WHERE v.Fecha >= @desde AND v.Fecha <= @hasta
                               GROUP BY d.CodigoProducto, YEAR(v.Fecha), MONTH(v.Fecha)
                               ORDER BY d.CodigoProducto ASC, YEAR(v.Fecha) ASC, MONTH(v.Fecha) ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                {
                    cmd.Parameters.AddWithValue("@desde", desde.Date);
                    cmd.Parameters.AddWithValue("@hasta", hasta.Date.AddDays(1).AddSeconds(-1));
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            resultado.Add((
                                Convert.ToInt32(dr["CodigoProducto"]),
                                Convert.ToInt32(dr["Anio"]),
                                Convert.ToInt32(dr["Mes"]),
                                Convert.ToInt32(dr["CantidadVendida"]),
                                Convert.ToDecimal(dr["MontoVendido"]),
                                Convert.ToDecimal(dr["CostoVendido"])
                            ));
                        }
                    }
                }
            }
            return resultado;
        }

        public Dictionary<int, int> ObtenerCantidadesEnTransitoPorProducto()
        {
            Dictionary<int, int> enTransito = new Dictionary<int, int>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT d.CodigoProducto, SUM(d.CantidadAprobadaAdmin) AS TotalEnTransito
                               FROM DetalleOrdenReabastecimiento d
                               INNER JOIN OrdenReabastecimiento o ON d.NroOrden = o.NroOrden
                               WHERE o.Estado = N'Pendiente'
                               GROUP BY d.CodigoProducto";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        enTransito[Convert.ToInt32(dr["CodigoProducto"])] = Convert.ToInt32(dr["TotalEnTransito"]);
                    }
                }
            }
            return enTransito;
        }

        public void RegistrarOrdenReabastecimientoTransaccional(OrdenReabastecimientoBE orden, Func<object, string> calcularDVH)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                using (SqlTransaction tx = cx.BeginTransaction())
                {
                    try
                    {
                        string sqlInsOrden = @"INSERT INTO OrdenReabastecimiento
                                               (FechaGeneracion, FechaRecepcion, MetodoPrediccion, HorizonteDias, Estado, CostoTotalOrden, Observaciones, ID_Usuario, DVH)
                                               OUTPUT INSERTED.NroOrden
                                               VALUES (@fechaGen, NULL, @metodo, @horizonte, @estado, @costoTotal, @obs, @idUsuario, NULL)";
                        using (SqlCommand cmdOrden = new SqlCommand(sqlInsOrden, cx, tx))
                        {
                            cmdOrden.Parameters.AddWithValue("@fechaGen", orden.FechaGeneracion);
                            cmdOrden.Parameters.AddWithValue("@metodo", orden.MetodoPrediccion);
                            cmdOrden.Parameters.AddWithValue("@horizonte", orden.HorizonteDias);
                            cmdOrden.Parameters.AddWithValue("@estado", orden.Estado);
                            cmdOrden.Parameters.AddWithValue("@costoTotal", orden.CostoTotalOrden);
                            cmdOrden.Parameters.AddWithValue("@obs", !string.IsNullOrWhiteSpace(orden.Observaciones) ? (object)orden.Observaciones : DBNull.Value);
                            cmdOrden.Parameters.AddWithValue("@idUsuario", orden.ID_UsuarioParaDVH);
                            orden.NroOrden = (int)cmdOrden.ExecuteScalar();
                        }
                        orden.DVH = calcularDVH(orden);
                        using (SqlCommand cmdDvh = new SqlCommand("UPDATE OrdenReabastecimiento SET DVH = @dvh WHERE NroOrden = @nro", cx, tx))
                        {
                            cmdDvh.Parameters.AddWithValue("@dvh", orden.DVH);
                            cmdDvh.Parameters.AddWithValue("@nro", orden.NroOrden);
                            cmdDvh.ExecuteNonQuery();
                        }
                        string sqlInsDet = @"INSERT INTO DetalleOrdenReabastecimiento
                                             (NroOrden, CodigoProducto, StockAlMomento, DemandaProyectada, CantidadSugeridaSistema, CantidadAprobadaAdmin, CostoUnitarioCompra, SubtotalCosto)
                                             VALUES (@nroOrden, @codProd, @stockMom, @demProy, @cantSug, @cantAprob, @costoUnit, @subCosto)";
                        foreach (var det in orden.Detalles)
                        {
                            det.NroOrden = orden.NroOrden;
                            using (SqlCommand cmdDet = new SqlCommand(sqlInsDet, cx, tx))
                            {
                                cmdDet.Parameters.AddWithValue("@nroOrden", orden.NroOrden);
                                cmdDet.Parameters.AddWithValue("@codProd", det.CodigoProducto);
                                cmdDet.Parameters.AddWithValue("@stockMom", det.StockAlMomento);
                                cmdDet.Parameters.AddWithValue("@demProy", det.DemandaProyectada);
                                cmdDet.Parameters.AddWithValue("@cantSug", det.CantidadSugeridaSistema);
                                cmdDet.Parameters.AddWithValue("@cantAprob", det.CantidadAprobadaAdmin);
                                cmdDet.Parameters.AddWithValue("@costoUnit", det.CostoUnitarioCompra);
                                cmdDet.Parameters.AddWithValue("@subCosto", det.SubtotalCosto);
                                cmdDet.ExecuteNonQuery();
                            }
                        }
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void RecibirOrdenReabastecimientoTransaccional(int nroOrden, Func<object, string> calcularDVH)
        {
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                using (SqlTransaction tx = cx.BeginTransaction())
                {
                    try
                    {
                        OrdenReabastecimientoBE orden = null;
                        string sqlOrd = @"SELECT NroOrden, FechaGeneracion, FechaRecepcion, MetodoPrediccion, HorizonteDias,
                                                 Estado, CostoTotalOrden, Observaciones, ID_Usuario, DVH
                                          FROM OrdenReabastecimiento WITH (UPDLOCK, ROWLOCK)
                                          WHERE NroOrden = @nro";
                        using (SqlCommand cmdGetOrd = new SqlCommand(sqlOrd, cx, tx))
                        {
                            cmdGetOrd.Parameters.AddWithValue("@nro", nroOrden);
                            using (SqlDataReader dr = cmdGetOrd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    int idUsu = Convert.ToInt32(dr["ID_Usuario"]);
                                    orden = new OrdenReabastecimientoBE
                                    {
                                        NroOrden = Convert.ToInt32(dr["NroOrden"]),
                                        FechaGeneracion = Convert.ToDateTime(dr["FechaGeneracion"]),
                                        MetodoPrediccion = dr["MetodoPrediccion"].ToString() ?? string.Empty,
                                        HorizonteDias = Convert.ToInt32(dr["HorizonteDias"]),
                                        Estado = dr["Estado"].ToString() ?? string.Empty,
                                        CostoTotalOrden = Convert.ToDecimal(dr["CostoTotalOrden"]),
                                        ID_Usuario = idUsu,
                                        Administrador = new UsuarioBE { ID = idUsu }
                                    };
                                }
                            }
                        }
                        if (orden == null)
                            throw new Exception($"No se encontró la Orden de Reabastecimiento #{nroOrden}.");
                        if (!string.Equals(orden.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase))
                            throw new Exception($"La Orden #{nroOrden} ya se encuentra en estado '{orden.Estado}' y no puede volver a recepcionarse.");
                        List<(int CodigoProducto, int CantidadAprobada, decimal CostoUnitario)> items = new List<(int, int, decimal)>();
                        string sqlDets = "SELECT CodigoProducto, CantidadAprobadaAdmin, CostoUnitarioCompra FROM DetalleOrdenReabastecimiento WHERE NroOrden = @nro";
                        using (SqlCommand cmdDets = new SqlCommand(sqlDets, cx, tx))
                        {
                            cmdDets.Parameters.AddWithValue("@nro", nroOrden);
                            using (SqlDataReader dr = cmdDets.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    items.Add((
                                        Convert.ToInt32(dr["CodigoProducto"]),
                                        Convert.ToInt32(dr["CantidadAprobadaAdmin"]),
                                        Convert.ToDecimal(dr["CostoUnitarioCompra"])
                                    ));
                                }
                            }
                        }
                        foreach (var item in items)
                        {
                            if (item.CantidadAprobada <= 0) continue;
                            ProductoBE prod = null;
                            string sqlProd = @"SELECT CodigoProducto, CodigoSKU, Nombre, Categoria, PrecioVenta, CostoCompra,
                                                      StockActual, StockMinimo, TiempoReposicionDias, ObjetivoFisicoAfin,
                                                      ObjetivoNutricionalAfin, Activo
                                               FROM Producto WITH (UPDLOCK, ROWLOCK)
                                               WHERE CodigoProducto = @idProd";
                            using (SqlCommand cmdP = new SqlCommand(sqlProd, cx, tx))
                            {
                                cmdP.Parameters.AddWithValue("@idProd", item.CodigoProducto);
                                using (SqlDataReader dr = cmdP.ExecuteReader())
                                {
                                    if (dr.Read())
                                    {
                                        prod = new ProductoBE
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
                                            Activo = Convert.ToBoolean(dr["Activo"])
                                        };
                                    }
                                }
                            }
                            if (prod != null)
                            {
                                prod.StockActual += item.CantidadAprobada;
                                if (item.CostoUnitario > 0)
                                    prod.CostoCompra = item.CostoUnitario;
                                prod.DVH = calcularDVH(prod);
                                string sqlUpdP = "UPDATE Producto SET StockActual = @stock, CostoCompra = @costo, DVH = @dvh WHERE CodigoProducto = @idProd";
                                using (SqlCommand cmdUpdP = new SqlCommand(sqlUpdP, cx, tx))
                                {
                                    cmdUpdP.Parameters.AddWithValue("@stock", prod.StockActual);
                                    cmdUpdP.Parameters.AddWithValue("@costo", prod.CostoCompra);
                                    cmdUpdP.Parameters.AddWithValue("@dvh", prod.DVH);
                                    cmdUpdP.Parameters.AddWithValue("@idProd", prod.CodigoProducto);
                                    cmdUpdP.ExecuteNonQuery();
                                }
                            }
                        }
                        orden.Estado = "Recibida";
                        orden.FechaRecepcion = DateTime.Now;
                        orden.DVH = calcularDVH(orden);
                        string sqlUpdOrd = "UPDATE OrdenReabastecimiento SET Estado = @estado, FechaRecepcion = @fechaRec, DVH = @dvh WHERE NroOrden = @nro";
                        using (SqlCommand cmdUpdOrd = new SqlCommand(sqlUpdOrd, cx, tx))
                        {
                            cmdUpdOrd.Parameters.AddWithValue("@estado", orden.Estado);
                            cmdUpdOrd.Parameters.AddWithValue("@fechaRec", orden.FechaRecepcion.Value);
                            cmdUpdOrd.Parameters.AddWithValue("@dvh", orden.DVH);
                            cmdUpdOrd.Parameters.AddWithValue("@nro", orden.NroOrden);
                            cmdUpdOrd.ExecuteNonQuery();
                        }
                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public List<OrdenReabastecimientoBE> ConsultarOrdenesReabastecimiento()
        {
            List<OrdenReabastecimientoBE> lista = new List<OrdenReabastecimientoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT o.NroOrden, o.FechaGeneracion, o.FechaRecepcion, o.MetodoPrediccion,
                                      o.HorizonteDias, o.Estado, o.CostoTotalOrden, o.Observaciones,
                                      o.ID_Usuario, o.DVH, u.Nombre AS AdminNombre
                               FROM OrdenReabastecimiento o
                               INNER JOIN Usuario u ON o.ID_Usuario = u.ID
                               ORDER BY o.FechaGeneracion DESC, o.NroOrden DESC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int idUsu = Convert.ToInt32(dr["ID_Usuario"]);
                        lista.Add(new OrdenReabastecimientoBE
                        {
                            NroOrden = Convert.ToInt32(dr["NroOrden"]),
                            FechaGeneracion = Convert.ToDateTime(dr["FechaGeneracion"]),
                            FechaRecepcion = dr["FechaRecepcion"] != DBNull.Value ? Convert.ToDateTime(dr["FechaRecepcion"]) : (DateTime?)null,
                            MetodoPrediccion = dr["MetodoPrediccion"].ToString() ?? string.Empty,
                            HorizonteDias = Convert.ToInt32(dr["HorizonteDias"]),
                            Estado = dr["Estado"].ToString() ?? string.Empty,
                            CostoTotalOrden = Convert.ToDecimal(dr["CostoTotalOrden"]),
                            Observaciones = dr["Observaciones"] != DBNull.Value ? dr["Observaciones"].ToString() : string.Empty,
                            ID_Usuario = idUsu,
                            Administrador = new UsuarioBE { ID = idUsu, Nombre = dr["AdminNombre"].ToString() ?? string.Empty },
                            DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : string.Empty
                        });
                    }
                }
                foreach (var ord in lista)
                {
                    ord.Detalles = ObtenerDetallesDeOrdenInterno(cx, ord.NroOrden);
                }
            }
            return lista;
        }

        private List<DetalleOrdenReabastecimientoBE> ObtenerDetallesDeOrdenInterno(SqlConnection cx, int nroOrden)
        {
            List<DetalleOrdenReabastecimientoBE> detalles = new List<DetalleOrdenReabastecimientoBE>();
            string sql = @"SELECT d.ID_DetalleOrden, d.NroOrden, d.CodigoProducto, d.StockAlMomento,
                                  d.DemandaProyectada, d.CantidadSugeridaSistema, d.CantidadAprobadaAdmin,
                                  d.CostoUnitarioCompra, d.SubtotalCosto,
                                  p.CodigoSKU, p.Nombre AS NombreProducto
                           FROM DetalleOrdenReabastecimiento d
                           INNER JOIN Producto p ON d.CodigoProducto = p.CodigoProducto
                           WHERE d.NroOrden = @nro
                           ORDER BY d.ID_DetalleOrden ASC";
            using (SqlCommand cmd = new SqlCommand(sql, cx))
            {
                cmd.Parameters.AddWithValue("@nro", nroOrden);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        detalles.Add(new DetalleOrdenReabastecimientoBE
                        {
                            ID_DetalleOrden = Convert.ToInt32(dr["ID_DetalleOrden"]),
                            NroOrden = Convert.ToInt32(dr["NroOrden"]),
                            CodigoProducto = Convert.ToInt32(dr["CodigoProducto"]),
                            CodigoSKU = dr["CodigoSKU"].ToString() ?? string.Empty,
                            NombreProducto = dr["NombreProducto"].ToString() ?? string.Empty,
                            StockAlMomento = Convert.ToInt32(dr["StockAlMomento"]),
                            DemandaProyectada = Convert.ToDecimal(dr["DemandaProyectada"]),
                            CantidadSugeridaSistema = Convert.ToInt32(dr["CantidadSugeridaSistema"]),
                            CantidadAprobadaAdmin = Convert.ToInt32(dr["CantidadAprobadaAdmin"]),
                            CostoUnitarioCompra = Convert.ToDecimal(dr["CostoUnitarioCompra"]),
                            SubtotalCosto = Convert.ToDecimal(dr["SubtotalCosto"])
                        });
                    }
                }
            }
            return detalles;
        }

        public List<OrdenReabastecimientoBE> LeerTodasLasOrdenes()
        {
            List<OrdenReabastecimientoBE> lista = new List<OrdenReabastecimientoBE>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                string sql = @"SELECT NroOrden, FechaGeneracion, FechaRecepcion, MetodoPrediccion,
                                      HorizonteDias, Estado, CostoTotalOrden, Observaciones, ID_Usuario, DVH
                               FROM OrdenReabastecimiento
                               ORDER BY NroOrden ASC";
                using (SqlCommand cmd = new SqlCommand(sql, cx))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int idUsu = Convert.ToInt32(dr["ID_Usuario"]);
                        lista.Add(new OrdenReabastecimientoBE
                        {
                            NroOrden = Convert.ToInt32(dr["NroOrden"]),
                            FechaGeneracion = Convert.ToDateTime(dr["FechaGeneracion"]),
                            FechaRecepcion = dr["FechaRecepcion"] != DBNull.Value ? Convert.ToDateTime(dr["FechaRecepcion"]) : (DateTime?)null,
                            MetodoPrediccion = dr["MetodoPrediccion"].ToString() ?? string.Empty,
                            HorizonteDias = Convert.ToInt32(dr["HorizonteDias"]),
                            Estado = dr["Estado"].ToString() ?? string.Empty,
                            CostoTotalOrden = Convert.ToDecimal(dr["CostoTotalOrden"]),
                            Observaciones = dr["Observaciones"] != DBNull.Value ? dr["Observaciones"].ToString() : string.Empty,
                            ID_Usuario = idUsu,
                            Administrador = new UsuarioBE { ID = idUsu },
                            DVH = dr["DVH"] != DBNull.Value ? dr["DVH"].ToString() : string.Empty
                        });
                    }
                }
            }
            return lista;
        }

        public List<string> ObtenerTodosLosDVHOrdenes()
        {
            List<string> lista = new List<string>();
            using (SqlConnection cx = ConexionDAL.Instancia.ObtenerConexion())
            {
                cx.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT DVH FROM OrdenReabastecimiento ORDER BY NroOrden ASC", cx))
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