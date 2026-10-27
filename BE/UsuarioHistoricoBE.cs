using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class UsuarioHistoricoBE : UsuarioBE
    {
        public int ID_Cambio { get; set; }
        public int ID_Usuario_Autor { get; set; }
        public string NombreUsuarioAutor { get; set; }
        public DateTime FechaHora { get; set; }
        public string Accion { get; set; }
    }
}