using System;
using System.ComponentModel;

namespace BE
{
    public class DetalleVentaProductoBE
    {
        [Browsable(false)]
        public int ID_DetalleVenta { get; set; }

        [Browsable(false)]
        public int NroVenta { get; set; }

        [Browsable(false)]
        public int CodigoProducto { get; set; }

        public string CodigoSKU { get; set; } = string.Empty;

        public string NombreProducto { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        [Browsable(false)]
        public decimal CostoUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }
}