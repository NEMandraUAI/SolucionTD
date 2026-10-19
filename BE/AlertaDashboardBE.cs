using System;

namespace BE
{
    public class AlertaDashboardBE
    {
        public string Area { get; set; } = string.Empty;
        public string Prioridad { get; set; } = "INFO";
        public string Titulo { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public string FechaReferencia { get; set; } = string.Empty;
    }
}