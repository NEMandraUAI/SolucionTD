using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class RegistroBE
    {
        public int ID_Registro { get; set; }
        public int ID_Usuario { get; set; }
        public DateTime FechaHora { get; set; }
        public string Evento { get; set; }
        public string Criticidad { get; set; }
        public string NombreUsuario { get; set; }
    }
}