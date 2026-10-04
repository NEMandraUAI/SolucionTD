using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PlanNutricionalBE : IEntidadVerificable
    {
        [DigitoVerificador(1)]
        public int CodigoPlan { get; set; }

        [DigitoVerificador(2)]
        public DateTime FechaCreacion { get; set; }

        [DigitoVerificador(3)]
        public string EvaluacionAntropometrica { get; set; }

        [DigitoVerificador(4)]
        public string DetalleHabitos { get; set; }

        [DigitoVerificador(5)]
        public int ID_Socio { get; set; }

        [DigitoVerificador(6)]
        public int ID_Nutricionista { get; set; }

        public string DVH { get; set; }

        public SocioBE Socio { get; set; }
        public UsuarioBE Nutricionista { get; set; }
        public string NombreSocio { get { return Socio != null ? Socio.Nombre + " " + Socio.Apellido : ""; } }
        public string NombreNutricionista { get { return Nutricionista != null ? Nutricionista.Nombre : ""; } }

        public string ObtenerID()
        {
            return CodigoPlan.ToString();
        }
    }
}
