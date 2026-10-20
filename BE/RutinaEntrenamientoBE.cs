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
    public class RutinaEntrenamientoBE : IEntidadVerificable
    {
        private string _detalleEjercicios = string.Empty;

        [DigitoVerificador(1)]
        public int CodigoRutina { get; set; }

        [DigitoVerificador(2)]
        public DateTime FechaCreacion { get; set; }

        [DigitoVerificador(3)]
        public string ObjetivoFisico { get; set; } = string.Empty;

        [DigitoVerificador(4)]
        public int FrecuenciaSemanal { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public bool EsVigente { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public string TextoEstadoVigencia { get; set; } = string.Empty;

        public string EstadoVigencia
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TextoEstadoVigencia))
                    return TextoEstadoVigencia;
                return EsVigente ? "VIGENTE (Actual)" : "HISTÓRICA (Solo Lectura)";
            }
        }

        [DigitoVerificador(5)]
        public string DetalleEjercicios
        {
            get { return _detalleEjercicios; }
            set
            {
                _detalleEjercicios = value ?? string.Empty;
                DeserializarEjerciciosDesdeJson(_detalleEjercicios);
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public List<EjercicioBE> Ejercicios { get; set; } = new List<EjercicioBE>();

        public SocioBE Socio { get; set; }

        public UsuarioBE Entrenador { get; set; }

        [DigitoVerificador(6)]
        public int ID_SocioParaDVH { get { return Socio != null ? Socio.ID_Socio : 0; } }

        [DigitoVerificador(7)]
        public int ID_UsuarioParaDVH { get { return Entrenador != null ? Entrenador.ID : 0; } }

        public string DVH { get; set; } = string.Empty;

        public string ObtenerID()
        {
            return CodigoRutina.ToString();
        }

        public void SincronizarEjerciciosAJson()
        {
            if (Ejercicios != null && Ejercicios.Count > 0)
            {
                _detalleEjercicios = JsonSerializer.Serialize(Ejercicios);
            }
            else
            {
                _detalleEjercicios = string.Empty;
            }
        }

        private void DeserializarEjerciciosDesdeJson(string contenido)
        {
            if (string.IsNullOrWhiteSpace(contenido))
            {
                Ejercicios = new List<EjercicioBE>();
                return;
            }

            try
            {
                string limpio = contenido.Trim();
                if (limpio.StartsWith("["))
                {
                    var lista = JsonSerializer.Deserialize<List<EjercicioBE>>(limpio);
                    Ejercicios = lista ?? new List<EjercicioBE>();
                }
                else
                {
                    Ejercicios = new List<EjercicioBE>
                    {
                        new EjercicioBE
                        {
                            Nombre = limpio,
                            GrupoMuscular = "General",
                            Series = 3,
                            Repeticiones = "10-12",
                            TiempoDescansoSegundos = 60,
                            CargaSugeridaKg = 0
                        }
                    };
                }
            }
            catch
            {
                Ejercicios = new List<EjercicioBE>();
            }
        }
    }
}