using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace sistemadeCatalogo_Everyday_technology_.Models
{
    public class Producto
    {
        public int id { get; set; }

        [Required(ErrorMessage= "Coloca el codigo del producto")]
        [StringLength(20)]
        [Display(Name ="Codigo")]

        public string Codigo {  get; set; }

        [Required(ErrorMessage ="El producto tiene que llevar u nombre")]
        [StringLength(1000)]
        public string Nombre { get; set; }

        [Required(ErrorMessage ="El producto tiene que llevar descripcion")]
        [StringLength(1200)]
        [Display(Name ="Descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage ="El producto tiene que llevar un precio!")]
        [Range(0.1, 100000, ErrorMessage ="El precio tiene que ser mayor que $0.00")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }
        [Display(Name ="Imagen")]
        public string Imagen { get; set; }

        }
    }
