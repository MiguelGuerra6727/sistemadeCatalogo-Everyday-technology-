using System;
using System.Security.Cryptography;


namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class PasswordHelper
    {
        private const int Iteraciones = 10000;

        public static string GenerarSal()
        {
            var sal = new byte[16];
            using(var rng = new RNGCryptoServiceProvider()) { rng.GetBytes(sal); }
            return Convert.ToBase64String(sal);
        }

        public static string Hash(string password, string salBase64)
        {
            byte[] sal = Convert.FromBase64String(salBase64);
            using (var pbkdf2 =new Rfc2898DeriveBytes(password, sal, Iteraciones))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }
        
        public static bool Verificar(string password, string salBase64, string hasGuardado)
        {
            byte[] calculado = Convert.FromBase64String(Hash(password, salBase64));
            byte[] guardado = Convert.FromBase64String(hasGuardado);

            int diferencia = 0;
            for (int i = 0; i < calculado.Length; i++) diferencia |= calculado[i] ^ guardado[i];
            return diferencia == 0;
        }

    }//Public class
}