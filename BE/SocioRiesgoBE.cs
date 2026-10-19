using System;
using System.ComponentModel;

namespace BE
{
    public class SocioRiesgoBE
    {
        [Browsable(false)]
        public int ID_Socio { get; set; }

        public string DNI { get; set; } = string.Empty;
        public string Socio { get; set; } = string.Empty;
        public string PlanActual { get; set; } = string.Empty;
        public string NivelRiesgo { get; set; } = string.Empty;

        [Browsable(false)]
        public int PuntajeFuga { get; set; }

        public string ProbabilidadFuga { get; set; } = string.Empty;
        public string MotivoDetectado { get; set; } = string.Empty;
        public string UltimaActividad { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;

        [Browsable(false)]
        public string Telefono { get; set; } = string.Empty;

        [Browsable(false)]
        public string Email { get; set; } = string.Empty;
    }
}