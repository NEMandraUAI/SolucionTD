using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BE
{
    public class PlanNutricionalBE : IEntidadVerificable
    {
        private string _evaluacionAntropometrica = string.Empty;

        [DigitoVerificador(1)]
        public int CodigoPlan { get; set; }

        [DigitoVerificador(2)]
        public DateTime FechaCreacion { get; set; }

        [DigitoVerificador(3)]
        public string EvaluacionAntropometrica
        {
            get { return _evaluacionAntropometrica; }
            set
            {
                _evaluacionAntropometrica = value ?? string.Empty;
                DeserializarBiometriaDesdeJson(_evaluacionAntropometrica);
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public EvaluacionBiometricaBE DatosBiometricos { get; set; } = new EvaluacionBiometricaBE();

        public string ResumenBiometrico
        {
            get
            {
                if (DatosBiometricos != null && DatosBiometricos.PesoKg > 0 && DatosBiometricos.AlturaCm > 0)
                {
                    return $"{DatosBiometricos.PesoKg:0.#} kg | {DatosBiometricos.AlturaCm:0} cm | IMC: {DatosBiometricos.IndiceMasaCorporal:0.00} ({DatosBiometricos.DiagnosticoIMC}) | {DatosBiometricos.CaloriasObjetivo:0} kcal";
                }
                return _evaluacionAntropometrica;
            }
        }

        [DigitoVerificador(4)]
        public string DetalleHabitos { get; set; } = string.Empty;

        [DigitoVerificador(5)]
        public int ID_Socio { get; set; }

        [DigitoVerificador(6)]
        public int ID_Nutricionista { get; set; }

        public string DVH { get; set; } = string.Empty;

        public SocioBE Socio { get; set; }
        public UsuarioBE Nutricionista { get; set; }
        public string NombreSocio { get { return Socio != null ? Socio.Nombre + " " + Socio.Apellido : ""; } }
        public string NombreNutricionista { get { return Nutricionista != null ? Nutricionista.Nombre : ""; } }

        public string ObtenerID()
        {
            return CodigoPlan.ToString();
        }

        public void SincronizarBiometriaAJson()
        {
            if (DatosBiometricos != null && DatosBiometricos.PesoKg > 0 && DatosBiometricos.AlturaCm > 0)
            {
                _evaluacionAntropometrica = JsonSerializer.Serialize(DatosBiometricos);
            }
        }

        private void DeserializarBiometriaDesdeJson(string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido))
            {
                DatosBiometricos = new EvaluacionBiometricaBE();
                return;
            }
            try
            {
                string limpio = contenido.Trim();
                if (limpio.StartsWith("{"))
                {
                    var obj = JsonSerializer.Deserialize<EvaluacionBiometricaBE>(limpio);
                    DatosBiometricos = obj ?? new EvaluacionBiometricaBE();
                }
                else
                {
                    DatosBiometricos = new EvaluacionBiometricaBE();
                }
            }
            catch
            {
                DatosBiometricos = new EvaluacionBiometricaBE();
            }
        }
    }
}