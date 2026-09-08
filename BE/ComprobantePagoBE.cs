using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ComprobantePagoBE
    {
        [DigitoVerificador(1)]
        public int NroComprobante { get; set; }

        [DigitoVerificador(2)]
        public DateTime Fecha { get; set; }

        [DigitoVerificador(3)]
        public decimal MontoTotal { get; set; }

        [DigitoVerificador(4)]
        public string MetodoPago { get; set; }

        public SocioBE Socio { get; set; }

        public UsuarioBE EmpleadoCobrador { get; set; }

        [DigitoVerificador(5)]
        public int ID_SocioParaDVH { get { return Socio != null ? Socio.ID_Socio : 0; } }

        [DigitoVerificador(6)]
        public int ID_UsuarioParaDVH { get { return EmpleadoCobrador != null ? EmpleadoCobrador.ID : 0; } }

        public string DVH { get; set; }
    }
}
