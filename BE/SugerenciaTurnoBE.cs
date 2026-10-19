using System;

namespace BE
{
    public class SugerenciaTurnoBE
    {
        public bool TieneSugerencia { get; set; }
        public DateTime FechaHoraSugerida { get; set; }
        public int ID_NutricionistaSugerido { get; set; }
        public string NombreNutricionistaSugerido { get; set; } = string.Empty;
        public int CoincidenciasDia { get; set; }
        public int CoincidenciasHora { get; set; }
        public int TotalTurnosAnalizados { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}