using Microsoft.Ajax.Utilities;
using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class PedidoRepositorio
    {
        //Lista estatica mientras esperamos base de datos 
        private static readonly List<pedido> _pedidos = new List<pedido>();
        private static int _siguienteId = 1;
        private static readonly object _lock = new object();

        public static void Agregar(pedido pedido)
        {
            lock (_lock)
            {
                pedido.Id = _siguienteId++;
                pedido.Fecha = DateTime.Now;
                _pedidos.Add(pedido);
            }
        }
        public static List<pedido> ObtenerTodos()
        {
            lock (_lock) { return _pedidos.OrderByDescending(p => p.Fecha).ToList(); }  
        }
        public static pedido ObtenerPorId(int id)
        {
            lock (_lock) { return _pedidos.FirstOrDefault(p => p.Id == id); }
        }
        //Este hace que un perdido pendiente pueda pasar de entregado o cancelado
        ////Al cencelar, las unidades tienen que volver al stock

        public static bool CambiarEstado(int id, string nuevoEstado)
        {
            lock (_lock)
            {
                var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
                if (pedido == null || pedido.Estado != EstadoPedido.Pendiente) return false;
                if (nuevoEstado != EstadoPedido.Entregado && nuevoEstado != EstadoPedido.Cancelado) return false; 
                
                if (nuevoEstado ==EstadoPedido.Cancelado)
                {
                    foreach (var item in pedido.Items)
                        ProductoRepositorio.ReponerStock(item.ProductoId, item.Cantidad);
                }
                pedido.Estado = nuevoEstado;
                return true;
            }
        }
    }
}