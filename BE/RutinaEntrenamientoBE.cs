using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class RutinaEntrenamientoBE
    {
        [DigitoVerificador(1)]
        public int CodigoRutina { get; set; }

        [DigitoVerificador(2)]
        public DateTime FechaCreacion { get; set; }

        [DigitoVerificador(3)]
        public string ObjetivoFisico { get; set; }

        [DigitoVerificador(4)]
        public int FrecuenciaSemanal { get; set; }

        [DigitoVerificador(5)]
        public string DetalleEjercicios { get; set; }

        public SocioBE Socio { get; set; }

        public UsuarioBE Entrenador { get; set; }

        [DigitoVerificador(6)]
        public int ID_SocioParaDVH { get { return Socio != null ? Socio.ID_Socio : 0; } }

        [DigitoVerificador(7)]
        public int ID_UsuarioParaDVH { get { return Entrenador != null ? Entrenador.ID : 0; } }

        public string DVH { get; set; }
    }
}
