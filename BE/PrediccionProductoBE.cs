using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BE
{
    public class PrediccionProductoBE
    {
        [Browsable(false)]
        public int CodigoProducto { get; set; }

        public string CodigoSKU { get; set; } = string.Empty;

        public string Producto { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public string HistorialMensual { get; set; } = string.Empty;

        [Browsable(false)]
        public int TotalUnidadesVendidasHistoria { get; set; }

        [Browsable(false)]
        public int PeriodosConDatos { get; set; }

        public string Tendencia { get; set; } = string.Empty;

        [Browsable(false)]
        public decimal DemandaBaseSerie { get; set; }

        [Browsable(false)]
        public decimal AjusteSinergiaPlanes { get; set; }

        public decimal DemandaProyectada { get; set; }

        public string ErrorPronosticoMAPE { get; set; } = string.Empty;

        [Browsable(false)]
        public decimal ValorMAE { get; set; }

        [Browsable(false)]
        public decimal ValorMAPE { get; set; }

        public int StockMinimo { get; set; }

        public int StockActual { get; set; }

        public int StockEnTransito { get; set; }

        [Browsable(false)]
        public int TiempoReposicionDias { get; set; }

        public int PuntoReordenROP { get; set; }

        public string AutonomiaDias { get; set; } = string.Empty;

        public string NivelRiesgoStock { get; set; } = string.Empty;

        [Browsable(false)]
        public int CodigoSeveridadRiesgo { get; set; }

        public string AfinidadDeportivaNutricional { get; set; } = string.Empty;

        public string CalculoSugerencia { get; set; } = string.Empty;

        public int CantidadSugerida { get; set; }

        public int CantidadAComprarAdmin { get; set; }

        public decimal CostoUnitario { get; set; }

        [Browsable(false)]
        public decimal PrecioVentaUnitario { get; set; }

        [Browsable(false)]
        public decimal MargenPorcentaje { get; set; }

        [Browsable(false)]
        public decimal IndiceRotacion { get; set; }

        public decimal CostoTotalReposicion
        {
            get { return Math.Round(CantidadAComprarAdmin * CostoUnitario, 2); }
        }

        [Browsable(false)]
        public List<PuntoEvolucionBE> SerieTemporalGrafico { get; set; } = new List<PuntoEvolucionBE>();
    }
}