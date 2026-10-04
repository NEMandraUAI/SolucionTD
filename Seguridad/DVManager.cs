using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Seguridad
{
    public class DVManager
    {
        public static string CalcularDVH(object entidad)
        {
            StringBuilder sb = new StringBuilder();
            Type tipo = entidad.GetType();
            PropertyInfo[] propiedades = tipo.GetProperties();
            foreach (var prop in propiedades)
            {
                if (Attribute.IsDefined(prop, typeof(DigitoVerificadorAttribute)))
                {
                    var valor = prop.GetValue(entidad);
                    if (valor != null)
                    {
                        sb.Append(valor.ToString());
                    }
                }
            }
            return CryptoManager.GenerarHash(sb.ToString());
        }
        public static string CalcularDVV(List<string> listaDVH)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var dvh in listaDVH)
            {
                sb.Append(dvh);
            }
            return CryptoManager.GenerarHash(sb.ToString());
        }
    }
}
