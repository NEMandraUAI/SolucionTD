using System;
using System.ComponentModel;

namespace BE
{
    public class DetalleOrdenReabastecimientoBE
    {
        [Browsable(false)]
        public int ID_DetalleOrden { get; set; }

        [Browsable(false)]
        public int NroOrden { get; set; }

        [Browsable(false)]
        public int CodigoProducto { get; set; }

        public string CodigoSKU { get; set; } = string.Empty;

        public string NombreProducto { get; set; } = string.Empty;

        public int StockAlMomento { get; set; }

        public decimal DemandaProyectada { get; set; }

        public int CantidadSugeridaSistema { get; set; }

        public int CantidadAprobadaAdmin { get; set; }

        public decimal CostoUnitarioCompra { get; set; }

        public decimal SubtotalCosto { get; set; }
    }
}