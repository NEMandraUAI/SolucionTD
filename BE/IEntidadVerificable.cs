using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface IEntidadVerificable
    {
        string DVH { get; set; }
        string ObtenerID();
    }
}
