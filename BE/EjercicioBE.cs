using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class EjercicioBE
    {
        public string Nombre { get; set; } = string.Empty;
        public string GrupoMuscular { get; set; } = string.Empty;
        public int Series { get; set; }
        public string Repeticiones { get; set; } = string.Empty;
        public int TiempoDescansoSegundos { get; set; }
        public decimal CargaSugeridaKg { get; set; }
    }
}