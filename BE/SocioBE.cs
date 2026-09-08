using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class SocioBE
    {
        [DigitoVerificador(1)]
        public int ID_Socio { get; set; }

        [DigitoVerificador(2)]
        public string DNI { get; set; }

        [DigitoVerificador(3)]
        public string Nombre { get; set; }

        [DigitoVerificador(4)]
        public string Apellido { get; set; }

        [DigitoVerificador(5)]
        public string Telefono { get; set; }

        [DigitoVerificador(6)]
        public string Email { get; set; }

        [DigitoVerificador(7)]
        public bool EstadoActivo { get; set; }

        public PlanSuscripcionBE PlanAsociado { get; set; }

        [DigitoVerificador(8)]
        public int CodigoPlanParaDVH
        {
            get { return PlanAsociado != null ? PlanAsociado.CodigoPlan : 0; }
        }

        public string DVH { get; set; }
    }
}
