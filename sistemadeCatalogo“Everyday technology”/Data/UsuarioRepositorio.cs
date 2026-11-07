using sistemadeCatalogo_Everyday_technology_.Models;
using System.Data.Entity;
using System.Linq;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class UsuarioRepositorio
    {
        //se usa cuando el mismo correo no eciste  
        public static readonly string _salFalsa = PasswordHelper.GenerarSal();

        public static Usuario Validar(string correo, string password)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrEmpty(password)) return null;

            string c = correo.Trim().ToLower();

            using (var db = new InventarioContext())
            {
                var usuario = db.Usuarios.AsNoTracking()
                    .Include(u => u.Rol)
                    .FirstOrDefault(u => u.Correo.ToLower() == c && u.Activo);

                if (usuario == null)
                {
                    PasswordHelper.Hash(password, _salFalsa);
                    return null;
                }

                return PasswordHelper.Verificar(password, usuario.Salt, usuario.PasswordHash)
                ? usuario
                : null;
            }//Fin del using
        } //Fin de la validacion de usuario
    }//Fin nameespace
}//fin de clase
