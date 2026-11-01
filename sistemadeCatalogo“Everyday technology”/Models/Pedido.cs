using System;
using System.Collections.Generic;
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
        public int ProductoId { get; set;}
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; } 
        public int Cantidad { get; set; }
        public decimal SubTotal { get { return PrecioUnitario * Cantidad; } }
    }

    public class pedido
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Telefono { get; set; }
        public string DireccionEnvio { get; set; }
        public string Estado { get; set; }

        public List<PedidoItem>Items { get; set; }
        public decimal Total { get { return Items == null ? 0m : Items.Sum(i => i.SubTotal); } }
    }
}