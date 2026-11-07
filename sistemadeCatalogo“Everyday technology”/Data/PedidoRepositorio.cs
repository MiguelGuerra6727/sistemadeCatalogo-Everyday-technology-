using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class PedidoRepositorio
    {
        public static string crear(pedido pedido, Dictionary<int, int> cantidades)
        {
            using (var db = new InventarioContext())
            using (var tx = db.Database.BeginTransaction())
            {
                foreach (var par in cantidades)
                {
                    int filas = db.Database.ExecuteSqlCommand(
                      "UPDATE Productos SET Stock = Stock - @p0 WHERE id = @p1 AND Activo = 1 AND Stock >= @p0",
                       par.Value, par.Key);

                    if (filas == 0)
                    {
                        var p = db.Productos.AsNoTracking().FirstOrDefault(x => x.id == par.Key);
                        tx.Rollback();

                        if (p == null || !p.Activo)
                            return " El prodducto ya no esta disponible ";
                        return p.Stock == 0
                            ? p.Nombre + " esta agotado "
                            : "De " + p.Nombre + " solo quedan " + p.Stock + " unidades ";

                    } // no alcanza los stocks
                } //FIN resta solo si el stocj es suficiente
                 
                pedido.Fecha = DateTime.Now;
                pedido.Estado = EstadoPedido.Pendiente;
                db.pedidos.Add(pedido);
                db.SaveChanges();

                tx.Commit();
                return null;
            }
        } //fin del crear 
        
        //lISTA DE HISTORIAL DE ITEMS
        public static List<pedido> ObtenerTodos()
        {
            using (var db = new InventarioContext())
            {
                return db.pedidos.AsNoTracking()
                    .Include(p => p.Items)
                    .OrderByDescending(p => p.Fecha)
                    .ToList();
            }
        } //Lista de oBtener todos 

         public static pedido ObtenerPorId(int id)
         {
             using (var db = new InventarioContext())
              {

                return db.pedidos.AsNoTracking()
                    .Include(p => p.Items)
                    .FirstOrDefault(p => p.Id == id);
              }
          }//Fin de pedidos
    
        //Pedidos pendientes (Cncelar o etregado)
        public static bool CambiarEstado( int id, string nuevoEstado)
        {
            if (nuevoEstado != EstadoPedido.Entregado && nuevoEstado != EstadoPedido.Cancelado)
            return false;

            using (var db = new InventarioContext())
            using (var tx = db.Database.BeginTransaction())
            {
                int filas = db.Database.ExecuteSqlCommand(
                "UPDATE Pedidos SET Estado = @p0 WHERE Id = @p1 AND Estado = @p2",
                 nuevoEstado, id, EstadoPedido.Pendiente);

                if (filas == 0)
                {
                    tx.Rollback();
                    return false;
                }

              if (nuevoEstado == EstadoPedido.Cancelado)
                {
                    var items = db.PedidoItems.AsNoTracking().Where(i => i.PedidoId == id).ToList();
                    foreach (var item in items)
                    {
                        db.Database.ExecuteSqlCommand(
                           "UPDATE Productos SET Stock = Stock + @p0 WHERE id = @p1",
                           item.Cantidad,item.ProductoId);
                    }
                }
                tx.Commit();
                return true;
            }
        } //fin de cambiar estado
    } //fin Crear
} // fin namespace