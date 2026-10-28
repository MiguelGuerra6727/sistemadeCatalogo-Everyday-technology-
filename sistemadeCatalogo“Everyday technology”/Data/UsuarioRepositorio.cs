using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class UsuarioRepositorio
    {
        private static readonly List<Usuario> _Usuarios = new List<Usuario>();
        private static readonly object _lock = new object();

        // Creaccion del administrador 
        static UsuarioRepositorio()
        {
            byte[] sal = GenerarSal();
            _Usuarios.Add(new Usuario
            {
                Id = 1,
                Nombre ="Administrador", //para mientras 
                Correo = "lasd9@gmail.com",
                Salt = Convert.ToBase64String(sal),
                PasswordHash = Hash("Admin67", sal),
                Rol = "Admin"
            });
        }
        //Fin de la creacion del admin 

        public static Usuario Validar(string correo, string password) 
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrEmpty(password)) return null;


            Usuario usuario;
            lock (_lock)
            {
                usuario = _Usuarios.FirstOrDefault(u =>
                string.Equals(u.Correo, correo.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            if (usuario == null) return null;

            byte[] sal = Convert.FromBase64String(usuario.Salt);
            byte[] Calculado = Convert.FromBase64String(Hash(password,sal));
            byte[] Guardado = Convert.FromBase64String(usuario.PasswordHash);

            return ComparacionSegura(Calculado, Guardado) ? usuario : null;
        }

        private static byte[] GenerarSal()
        {
            var sal = new byte[16];
            using (var rng = new RNGCryptoServiceProvider()) { rng.GetBytes(sal); }
            return sal;
        }

        private static string Hash(string password, byte[] sal)
        {
            using(var pbkdf2 = new Rfc2898DeriveBytes(password, sal, 100000))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));
            }
        }
        private static bool ComparacionSegura(byte[] a, byte[]b )
        {
            if (a.Length != b.Length) return false;
            int diferencia = 0;
            for (int i = 0; i < a.Length; i++) diferencia |= a[i] ^ b[i];
            return diferencia == 0;
        }

    }
}
