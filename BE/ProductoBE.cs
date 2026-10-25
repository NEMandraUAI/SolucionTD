using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BE
{
    public class ProductoBE : IEntidadVerificable
    {
        [DigitoVerificador(1)]
        public int CodigoProducto { get; set; }

        [DigitoVerificador(2)]
        public string CodigoSKU { get; set; } = string.Empty;

        [DigitoVerificador(3)]
        public string Nombre { get; set; } = string.Empty;

        [DigitoVerificador(4)]
        public string Categoria { get; set; } = string.Empty;

        [DigitoVerificador(5)]
        public decimal PrecioVenta { get; set; }

        [DigitoVerificador(6)]
        public decimal CostoCompra { get; set; }

        [DigitoVerificador(7)]
        public int StockActual { get; set; }

        [DigitoVerificador(8)]
        public int StockMinimo { get; set; }

        public int TiempoReposicionDias { get; set; } = 5;

        public string ObjetivoFisicoAfin { get; set; } = "Todos";

        public string ObjetivoNutricionalAfin { get; set; } = "Todos";

        [DigitoVerificador(9)]
        public bool Activo { get; set; } = true;

        [Browsable(false)]
        public string DVH { get; set; } = string.Empty;

        public decimal MargenUnitario
        {
            get { return Math.Max(0m, PrecioVenta - CostoCompra); }
        }

        public decimal MargenPorcentaje
        {
            get
            {
                if (PrecioVenta <= 0m) return 0m;
                return Math.Round(((PrecioVenta - CostoCompra) / PrecioVenta) * 100m, 1);
            }
        }

        [JsonIgnore]
        public string DescripcionCombo
        {
            get { return $"[{CodigoSKU}] {Nombre} — $ {PrecioVenta:N2} (Stock: {StockActual})"; }
        }

        public override string ToString()
        {
            return DescripcionCombo;
        }

        public string ObtenerID()
        {
            return CodigoProducto.ToString();
        }
    }
}