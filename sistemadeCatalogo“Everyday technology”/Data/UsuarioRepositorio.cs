using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class UsuarioRepositorio
    {
        private static readonly List<Usuarios> _usuarios = new List<Usuarios>();
        private static int _siguienteId = 1;
        private static readonly object _lock = new object();
        public static bool ExisteCorreo(string Correo)
        {
            lock (_lock)
            {
                return _usuarios.Any(u => u.Correo.ToLower() == Correo.ToLower());
            }
        }
        // Para el registro
        public static void Registrar(string Nombre, string Correo, string password)
        {
            var salt = GeneraSalt();
            lock (_lock)
            {
                _usuarios.Add(new Usuarios
                {
                    Id = _siguienteId++,
                    Nombre = Nombre,
                    Correo = Correo.Trim(),
                    Salt = Convert.ToBase64String(salt),
                    PasswordHash = Hash(password, salt),
                    Rol = "Cliente"
                });
            }
        }
        //Validar usuarios
        public static Usuarios Validar(string Correo, string password)
        {
            Usuarios usuarios;
            lock (_lock)
            {
                usuarios = _usuarios.FirstOrDefault(u => u.Correo.ToLower() == Correo.Trim().ToLower());
            }
            if (usuarios == null) return null;

            var hash = Hash(password, Convert.FromBase64String(usuarios.Salt));
            return hash == usuarios.PasswordHash ? usuarios : null;
        }
        private static byte[] GeneraSalt()
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider()) { rng.GetBytes(salt); }
            return salt;
        }
        // la cosa del hash

        private static string Hash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32));

            }
        }
    }
}