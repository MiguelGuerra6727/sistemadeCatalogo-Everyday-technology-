using sistemadeCatalogo_Everyday_technology_.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace sistemadeCatalogo_Everyday_technology_.ViewModels
{
    public class CarritoLineaViewModels
    {
        public Producto Producto { get; set; }
        public  int Cantidad { get; set; }
        public decimal Subtotal { get { return Producto.Precio * Cantidad; } }
    }

    public class CarritoViewModels
    {
        public List<CarritoLineaViewModels> Lineas { get; set; }
        public decimal Total { get { return Lineas.Sum(l => l.Subtotal); } }
    }

    public class CheckoutViewModels
    {
        [Required(ErrorMessage ="Escribe tu nombre")]
        [StringLength(100)]
        [Display(Name ="Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Escriba su numero de telefono")]
        [RegularExpression(@"^[0-9+\-\s]{7,20}$", ErrorMessage ="Telefono no valido, vulva a colocarlo ")]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Escribe el lugar de envio")]
        [StringLength(500)]
        [Display(Name = "Escriba el lugar del envio")]
        public string DireccionEnvio { get; set; }
       
        public CarritoViewModels Carrito { get; set; }
    }

    public class PedidosIndexViewModels
    {
        public List<pedido> pedidos { get; set; }
        public decimal TotalVentas { get; set; } //LA SUMA DE LOS PEDIDOS QUE SE CANCELAN
    }

}