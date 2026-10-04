using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class TurnoBE : IEntidadVerificable
    {
        [DigitoVerificador(1)]
        public int NroTurno { get; set; }

        [DigitoVerificador(2)]
        public DateTime FechaHora { get; set; }

        [DigitoVerificador(3)]
        public string Estado { get; set; }

        [DigitoVerificador(4)]
        public int ID_Socio { get; set; }

        [DigitoVerificador(5)]
        public int ID_Nutricionista { get; set; }

        public string DVH { get; set; }

        public SocioBE Socio { get; set; }
        public UsuarioBE Nutricionista { get; set; }
        public string DNISocio { get { return Socio != null ? Socio.DNI : ""; } }
        public string NombreSocio { get { return Socio != null ? Socio.Nombre + " " + Socio.Apellido : ""; } }
        public string NombreNutricionista { get { return Nutricionista != null ? Nutricionista.Nombre : ""; } }

        public string ObtenerID()
        {
            return NroTurno.ToString();
        }
    }
}
