using System;

namespace BE
{
    public class PuntoEvolucionBE
    {
        public DateTime Fecha { get; set; }
        public string EtiquetaFecha { get; set; } = string.Empty;
        public decimal ValorPrimario { get; set; }
        public decimal ValorSecundario { get; set; }
        public string Detalle { get; set; } = string.Empty;
    }
}