using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Models
{
    public class EstadoPedido
    {
        public const string Pendiente = "Pendiente de entrega";
        public const string Entregado = "Entregado";
        public const string Cancelado = "Cancelado";
    }

    public class PedidoItem
    {
        [Key]
        public int Id { get; set; }
        public int PedidoId { get; set; }
        [ForeignKey("PedidoId")]
        public virtual pedido Pedido { get; set; }
    
        public int ProductoId { get; set; }
        [ForeignKey("pedidoId")] //Este le pertenece al  public virtual
        public virtual Producto Producto { get; set; }

        //Este es una copia en el historial de la compra por si se cambia en un futuro el precio de algo
        [StringLength(20)]
        public string Codigo { get; set; }
        //Fin de la copia xd
        [StringLength(10000)]
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; } 
        public int Cantidad { get; set; }
        public decimal SubTotal { get { return PrecioUnitario * Cantidad; } }
    }

    public class pedido
    {
        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        [Required, StringLength(100)]
        public string Cliente { get; set; }

        [Required, StringLength(20)]
        public string Telefono { get; set; }
        [Required, StringLength(500)]
        public string DireccionEnvio { get; set; }
        [Required, StringLength(30)]
        public string Estado { get; set; }

        public virtual ICollection<PedidoItem> Items { get; set; }
        public decimal Total { get { return Items == null ? 0m : Items.Sum(i => i.SubTotal); } }
    }
}
