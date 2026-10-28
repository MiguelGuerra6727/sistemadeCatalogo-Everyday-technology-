using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string PasswordHash { get; set; }
        public string  Salt { get; set; }
        public string Rol { get; set; }
    }
}