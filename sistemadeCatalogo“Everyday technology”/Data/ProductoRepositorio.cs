using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class ProductoRepositorio
    {
        //Lista estatica mientras la wen hace la base de datos 
        private static readonly List<Producto> _Productos = new List<Producto>
        {
             new Producto { id = 1, Codigo = "RAM-001", Nombre = "Memoria RAM DDR4 8GB",
                Descripcion = "Módulo DDR4 3200 MHz para equipos de escritorio.", Precio = 32.50m, Imagen = "sin-imagen.png" },
            new Producto { id = 2, Codigo = "SSD-001", Nombre = "Disco SSD 480GB",
                Descripcion = "Unidad de estado sólido SATA de 2.5 pulgadas.", Precio = 38.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 3, Codigo = "FNT-001", Nombre = "Fuente de poder 550W",
                Descripcion = "Fuente ATX certificada 80 Plus Bronze.", Precio = 45.90m, Imagen = "sin-imagen.png" }
        };

        //orden del ide del producto que agreguemos al probar 
        private static int _siguienteid = 4;
        private static readonly object _lock = new object();

        //Acciones de la lista 
        public static List<Producto> ObtenerTodos()
        {
            lock (_lock) { return _Productos.ToList(); }

        }
        public static Producto ObtnerPorId(int id)
        {
            lock (_lock) { return _Productos.FirstOrDefault(p => p.id == id); }
        }

        public static bool ExisteCodigo(string Codigo, int? idExcluir = null)
        {
            lock (_lock)
            {
                return _Productos.Any(p => p.Codigo.ToLower() == Codigo.ToLower() && p.id != idExcluir);
            }
        }

        public static void Agregar(Producto producto)
        {
            lock (_lock)
            {
                producto.id = _siguienteid++;
                _Productos.Add(producto);
            }
        }

        public static void Actualizar(Producto producto)
        {
            lock (_lock)
            {
                var existente = _Productos.FirstOrDefault(p => p.id == producto.id);
                if (existente == null) return;
                existente.Codigo = producto.Codigo;
                existente.Nombre = producto.Nombre;
                existente.Descripcion = producto.Descripcion;
                existente.Precio = producto.Precio;
                existente.Imagen = producto.Imagen;
            }
        }

        public static void Eliminar(int id)
        {
            lock (_lock) { _Productos.RemoveAll(p => p.id == id); }
        }
    }
}