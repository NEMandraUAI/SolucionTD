using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class TraduccionBE
    {
        public int ID { get; set; }
        public IdiomaBE Idioma { get; set; }
        public string NombreControl { get; set; }
        public string Formulario { get; set; }
        public string Texto { get; set; }
    }
}