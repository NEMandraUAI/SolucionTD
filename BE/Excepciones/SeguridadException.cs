using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Excepciones
{
    public class SeguridadException : Exception
    {
        public TipoErrorSeguridad TipoError { get; set; }
        public SeguridadException(TipoErrorSeguridad tipo, string mensaje) : base(mensaje)
        {
            TipoError = tipo;
        }
    }

    public enum TipoErrorSeguridad
    {
        CredencialesInvalidas,
        CuentaBloqueada,
        SesionYaActiva,
        MultiplesIntentosFallidos
    }
}
