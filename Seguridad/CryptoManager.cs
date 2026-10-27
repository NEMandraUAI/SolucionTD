using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Seguridad
{
    public static class CryptoManager
    {
        public static bool Comparar(string claveIngresada, string claveBD) => claveBD == GenerarHash(claveIngresada) ? true : false;
        public static string GenerarHash(string textoPlano)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] arregloBytes = Encoding.UTF8.GetBytes(textoPlano);
                byte[] hashBytes = sha256.ComputeHash(arregloBytes);
                StringBuilder constructorTexto = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    constructorTexto.Append(b.ToString("x2"));
                }
                return constructorTexto.ToString();
            }
        }
    }
}