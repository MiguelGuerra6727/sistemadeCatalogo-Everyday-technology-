namespace sistemadeCatalogo_Everyday_technology_.Migrations
{
    using sistemadeCatalogo_Everyday_technology_.Data;
    using sistemadeCatalogo_Everyday_technology_.Models;
    using System.Data.Entity.Migrations;
    using System.Linq;

    //Este lo que hace es que si quiero atualizar o agregar alguna seccion me lo cambiara en la base de datos tambien
    //para no reacerla de nuevo :o
    //NO TOCAR EL DISIGN!!!!!!!!!!!!EASRFSSZTGZBTRZDT
    internal sealed class Configuration : DbMigrationsConfiguration<sistemadeCatalogo_Everyday_technology_.Data.InventarioContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.InventarioContext context)
        {
            //Roles
            if (!context.Roles.Any(r => r.Nombre == "Admin"))
                context.Roles.Add(new Rol { Nombre = "Admin" });
            if(!context.Roles.Any(r => r.Nombre == "Cliente"))
                context.Roles.Add(new Rol { Nombre = "Cliente" });
            context.SaveChanges();
            //fin roles
          if (!context.Seccions.Any())
            {
                context.Seccions.AddRange(new[]
                {
                    new Seccion { Nombre = "Memorias RAM", Descripcion = "Módulos de memoria RAM para equipos de escritorio y portátiles.", Imagen = "sin-imagen.png" },
                    new Seccion { Nombre = "Almacenamiento", Descripcion = "Discos SSD y discos duros para guardar tu información.", Imagen = "sin-imagen.png" },
                    new Seccion { Nombre = "Fuentes de poder", Descripcion = "Fuentes de alimentación para tu computadora.", Imagen = "sin-imagen.png" },
                    new Seccion { Nombre = "Procesadores", Descripcion = "Procesadores para equipos de oficina, estudio y juegos.", Imagen = "sin-imagen.png" },
                    new Seccion { Nombre = "Tarjetas gráficas", Descripcion = "Tarjetas de video para juegos y diseño.", Imagen = "sin-imagen.png" }
                });
                context.SaveChanges();
            }// Selcciones contet
             //Para el administrador 
            string correoAdmin = "lasd9@gmail.com";
            string passwordAdmin = "Admin67";

            if (!context.Usuarios.Any(u => u.Correo == correoAdmin))
            {
                var rolAdmin = context.Roles.First(r => r.Nombre == "Admin");
                string sal = PasswordHelper.GenerarSal();
                context.Usuarios.Add(new Usuario
                {
                    Nombre = "Administrador",
                    Correo = correoAdmin,
                    Salt = sal,
                    PasswordHash = PasswordHelper.Hash(passwordAdmin, sal),
                    RolId = rolAdmin.Id,
                    Activo = true
                });
            }

        }// fin del context
    }
}
