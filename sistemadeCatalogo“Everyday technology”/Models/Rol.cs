using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(30)]
        public string Nombre { get; set; }

        //Este hace que el rol tenga usuarios 
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}