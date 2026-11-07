using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class ProductoRepositorio
    {
        // Esta parte me avisa del stock cuando es insuficiente (Bajo pues)
        public const int StockMinimo = 5;

        //Este solo hace que los productos activos (Los otros los ocultara)
        public static List<Producto> ObtenerTodos()
        {
            using (var db = new InventarioContext())
            {
                return db.Productos.AsNoTracking()
                    .Where(p => p.Activo)
                    .OrderBy(p => p.id)
                    .ToList();
            }
        }//Fin de List Producto
        //Obetner por seccion inicio 
        public static List<Producto> ObtenerPorSeccion(int seccionId)
        {
            using (var db = new InventarioContext())
            {
                return db.Productos.AsNoTracking()
                    .Where(p => p.Activo && p.SeccionId == seccionId)
                    .OrderBy(p => p.id)
                    .ToList();
            }
        } // Obtener seccion final 
        public static Producto ObtenerPorId(int id)
        {
            using (var db = new InventarioContext())
            {
                return db.Productos.AsNoTracking()
                    .FirstOrDefault(p => p.id == id && p.Activo);
            }
        }

        public static bool ExisteCodigo(string codigo, int? idExcluiir = null)
        {
            string c = (codigo ?? "").Trim().ToLower();
            int excluir = idExcluiir ?? 0;
            using (var db = new InventarioContext())
            {
                return db.Productos.Any(p => p.Codigo.ToLower() == c && p.id != excluir);
            }
        }
        //Accion de agregar producto 
        public static void Agregar (Producto producto)
        {
            using(var db= new InventarioContext())
            {
                producto.Activo = true;
                db.Productos.Add(producto);
                db.SaveChanges();
            }
        }// fin de agregar
        // Actualizar producto 
        public static void Actualizar(Producto producto)
        {
            using (var db= new InventarioContext())
            {
                var existente = db.Productos.FirstOrDefault(p => p.id == producto.id);
                if (existente == null) return;

                existente.Codigo = producto.Codigo;
                existente.Nombre = producto.Nombre;
                existente.Descripcion = producto.Descripcion;
                existente.Precio = producto.Precio;
                existente.Imagen = producto.Imagen;
                existente.SeccionId = producto.SeccionId;
                existente.Stock = producto.Stock;
                db.SaveChanges();
            }
        } //Fin de actualizar 
        
        //Desactivar linea 
        public static void Elminar(int id)
        {
            using (var db = new InventarioContext())
            {
                var producto = db.Productos.FirstOrDefault(p => p.id == id);
                if (producto == null) return;

                producto.Activo = false;
                db.SaveChanges();
            }
        }

        //despuesntos en el estock y devolver null si se vende y error si no hay
        public static string DescontarStockLote(Dictionary<int, int> cantidades)
        {
            using (var db = new InventarioContext())
            {
                var ids = cantidades.Keys.ToList();
                var productos = db.Productos.Where( p=> ids.Contains(p.id) && p.Activo).ToList();

                foreach (var par in cantidades)
                {
                    var p = productos.FirstOrDefault(x => x.id == par.Key);
                    if (p == null) return " Este producto no esta disponible ";
                    if (par.Value > p.Stock)
                        return p.Stock == 0
                            ? p.Nombre + " esta agotado "
                            : "De " + p.Nombre + "solo quedan " + p.Stock + " unidades ";
                }

                foreach (var par in cantidades)
                    productos.First(x => x.id == par.Key).Stock -= par.Value;
                db.SaveChanges();
                return null;
            }// fin de usin
        } //fin de DescontarStock

        //Regresara el stock almomento que se cancele un pedido
        public static void ReponerStock(int id, int cantidad)
        {
            using ( var db = new InventarioContext())
            {
                var p = db.Productos.FirstOrDefault(x => x.id == id);
                if (p == null) return;
                p.Stock += cantidad;
                db.SaveChanges();

            } // fin usiing
        } // fin reponer stock

    }//Fin de la clase 


}//FinNameSpace