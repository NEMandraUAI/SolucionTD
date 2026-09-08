using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PlanSuscripcionBE
    {
        [DigitoVerificador(1)]
        public int CodigoPlan { get; set; }

        [DigitoVerificador(2)]
        public string Nombre { get; set; }

        [DigitoVerificador(3)]
        public decimal Precio { get; set; }

        [DigitoVerificador(4)]
        public int DuracionDias { get; set; }

        public string DVH { get; set; }
    }
}
