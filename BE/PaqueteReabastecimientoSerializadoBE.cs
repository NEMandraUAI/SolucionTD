using System;
using System.Collections.Generic;

namespace BE
{
    public class PaqueteReabastecimientoSerializadoBE
    {
        public string SistemaOrigen { get; set; } = "UAI-FIT | Gestión Comercial y de Mercado Interno";
        public string VersionFormato { get; set; } = "1.0";
        public DateTime FechaGeneracion { get; set; }
        public string UsuarioAdministrador { get; set; } = string.Empty;
        public int? NroOrdenReabastecimientoAsociada { get; set; }
        public string MetodoPrediccionUtilizado { get; set; } = string.Empty;
        public int HorizontePlanificacionDias { get; set; } = 30;
        public bool SinergiaEntrenamientoNutricionActiva { get; set; }
        public decimal FacturacionBrutaPeriodo { get; set; }
        public decimal CostoMercaderiaVendidaPeriodo { get; set; }
        public decimal GananciaBrutaPeriodo { get; set; }
        public decimal MargenPromedioPorcentaje { get; set; }
        public int UnidadesTotalesReabastecer { get; set; }
        public decimal InversionTotalEstimadaReabastecimiento { get; set; }
        public string ResumenSinergiaPoblacion { get; set; } = string.Empty;
        public List<ItemReabastecimientoSerializadoBE> ProductosEvaluados { get; set; } = new List<ItemReabastecimientoSerializadoBE>();
        public string HashIntegridadSHA256 { get; set; } = string.Empty;
    }

    public class ItemReabastecimientoSerializadoBE
    {
        public int CodigoProducto { get; set; }
        public string CodigoSKU { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string HistorialMensualVentas { get; set; } = string.Empty;
        public string TendenciaDetectada { get; set; } = string.Empty;
        public decimal DemandaProyectadaUnidades { get; set; }
        public string ErrorPronosticoMAPE { get; set; } = string.Empty;
        public int StockFisicoDisponible { get; set; }
        public int StockEnTransitoPendiente { get; set; }
        public int PuntoReordenROP { get; set; }
        public string NivelRiesgoRuptura { get; set; } = string.Empty;
        public string AfinidadEntrenamientoNutricion { get; set; } = string.Empty;
        public int CantidadSugeridaPorSistema { get; set; }
        public int CantidadDecididaPorAdministrador { get; set; }
        public decimal CostoUnitarioReposicion { get; set; }
        public decimal SubtotalReposicion { get; set; }
    }
}