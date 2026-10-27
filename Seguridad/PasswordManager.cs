using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Seguridad
{
    public static class PasswordManager
    {
        private const int Iteraciones = 600000;
        private const string Prefijo = "PBKDF2-SHA256";

        public static string GenerarHash(string clave)
        {
            ArgumentNullException.ThrowIfNull(clave);
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(clave, salt, Iteraciones, HashAlgorithmName.SHA256, 32);
            return $"{Prefijo}$1${Iteraciones.ToString(CultureInfo.InvariantCulture)}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool Verificar(string clave, string almacenado)
        {
            if (clave == null || string.IsNullOrWhiteSpace(almacenado)) return false;
            if (EsLegado(almacenado))
            {
                byte[] esperado = Convert.FromHexString(almacenado);
                return CryptographicOperations.FixedTimeEquals(SHA256.HashData(Encoding.UTF8.GetBytes(clave)), esperado);
            }
            if (!LeerFormato(almacenado, out int iteraciones, out byte[] salt, out byte[] esperadoPbkdf)) return false;
            byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(clave, salt, iteraciones, HashAlgorithmName.SHA256, esperadoPbkdf.Length);
            return CryptographicOperations.FixedTimeEquals(calculado, esperadoPbkdf);
        }

        public static bool RequiereActualizacion(string almacenado)
        {
            if (EsLegado(almacenado)) return true;
            return LeerFormato(almacenado, out int iteraciones, out _, out _) && iteraciones < Iteraciones;
        }

        private static bool EsLegado(string valor)
        {
            if (valor == null || valor.Length != 64) return false;
            foreach (char c in valor)
                if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))) return false;
            return true;
        }

        private static bool LeerFormato(string valor, out int iteraciones, out byte[] salt, out byte[] hash)
        {
            iteraciones = 0; salt = Array.Empty<byte>(); hash = Array.Empty<byte>();
            if (valor == null || valor.Length > 256) return false;
            string[] partes = valor.Split('$');
            if (partes.Length != 5 || partes[0] != Prefijo || partes[1] != "1" ||
                !int.TryParse(partes[2], NumberStyles.None, CultureInfo.InvariantCulture, out iteraciones) ||
                iteraciones < 1 || iteraciones > 2000000) return false;
            try
            {
                salt = Convert.FromBase64String(partes[3]);
                hash = Convert.FromBase64String(partes[4]);
                return salt.Length >= 16 && salt.Length <= 64 && hash.Length == 32;
            }
            catch (FormatException) { return false; }
        }
    }
}