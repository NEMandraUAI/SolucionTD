using System;

namespace BE
{
    public class EvaluacionBiometricaBE
    {
        public decimal PesoKg { get; set; }
        public decimal AlturaCm { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; } = "Masculino";
        public decimal IndiceMasaCorporal { get; set; }
        public decimal TasaMetabolicaBasal { get; set; }
        public string DiagnosticoIMC { get; set; } = string.Empty;
        public string ObjetivoNutricional { get; set; } = "Mantenimiento (Normocalórico)";
        public decimal CaloriasObjetivo { get; set; }
    }
}