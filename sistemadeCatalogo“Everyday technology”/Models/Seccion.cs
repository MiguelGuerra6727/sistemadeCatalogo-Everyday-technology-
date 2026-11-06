using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Models
{
    public class Seccion
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="La seccion necesita un nombre")]
        [StringLength(100)]
        public string Nombre { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; }
        [StringLength(200)]
        public string Imagen { get; set; }

        public bool Activo { get; set; } = true; 

        //Este hace que en vez de borrar ek producto lo oculta
        public virtual ICollection<Producto> Productos { get; set; }

    }
}