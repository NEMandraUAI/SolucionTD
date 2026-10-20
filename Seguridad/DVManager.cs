using BE;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var propiedadesOrdenadas = tipo.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(DigitoVerificadorAttribute)))
                .OrderBy(p => ((DigitoVerificadorAttribute)Attribute.GetCustomAttribute(p, typeof(DigitoVerificadorAttribute))).Orden)
                .ToList();
            foreach (var prop in propiedadesOrdenadas)
            {
                var valor = prop.GetValue(entidad);
                if (valor != null)
                {
                    sb.Append(FormatearValorInvariante(valor));
                }
            }
            return CryptoManager.GenerarHash(sb.ToString());
        }

        private static string FormatearValorInvariante(object valor)
        {
            if (valor is DateTime dt)
            {
                return dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }
            if (valor is decimal dec)
            {
                return dec.ToString("0.00", CultureInfo.InvariantCulture);
            }
            if (valor is double dbl)
            {
                return dbl.ToString("0.##", CultureInfo.InvariantCulture);
            }
            if (valor is float flt)
            {
                return flt.ToString("0.##", CultureInfo.InvariantCulture);
            }
            if (valor is bool b)
            {
                return b ? "True" : "False";
            }
            if (valor is string str)
            {
                return str.Replace("\r\n", "\n").Replace("\r", "\n");
            }
            if (valor is IFormattable formattable)
            {
                return formattable.ToString(null, CultureInfo.InvariantCulture);
            }
            return valor.ToString() ?? string.Empty;
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