using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DigitoVerificadorAttribute : Attribute
    {
        public int Orden { get; }
        public DigitoVerificadorAttribute(int orden)
        {
            Orden = orden;
        }
    }
}