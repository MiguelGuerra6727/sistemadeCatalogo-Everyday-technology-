using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class ProductoRepositorio
    {
        // Lista estática mientras la wen hace la base 
        private static readonly List<Producto> _Productos = new List<Producto>
        {
            // 1- Memorias RAM
            new Producto { id = 1, SeccionId = 1, Codigo = "RAM-001", Nombre = "Memoria RAM DDR4 8GB",
                Descripcion = "Módulo DDR4 3200 MHz para equipos de escritorio.", Precio = 32.50m, Imagen = "sin-imagen.png" },
            new Producto { id = 4, SeccionId = 1, Codigo = "RAM-002", Nombre = "Memoria RAM DDR4 16GB",
                Descripcion = "Módulo DDR4 3200 MHz de 16 GB, ideal para multitarea y edición.", Precio = 58.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 5, SeccionId = 1, Codigo = "RAM-003", Nombre = "Memoria RAM DDR5 16GB",
                Descripcion = "Módulo DDR5 5600 MHz para plataformas de nueva generación.", Precio = 79.90m, Imagen = "sin-imagen.png" },

            // 2- Almacenamiento
            new Producto { id = 2, SeccionId = 2, Codigo = "SSD-001", Nombre = "Disco SSD 480GB",
                Descripcion = "Unidad de estado sólido SATA de 2.5 pulgadas.", Precio = 38.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 6, SeccionId = 2, Codigo = "SSD-002", Nombre = "SSD NVMe M.2 1TB",
                Descripcion = "Unidad NVMe de alta velocidad para arranque rápido y juegos.", Precio = 79.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 7, SeccionId = 2, Codigo = "HDD-001", Nombre = "Disco duro 2TB 7200 RPM",
                Descripcion = "Disco duro SATA de 3.5 pulgadas para almacenamiento masivo.", Precio = 64.50m, Imagen = "sin-imagen.png" },

            // 3- Fuentes de poder
            new Producto { id = 3, SeccionId = 3, Codigo = "FNT-001", Nombre = "Fuente de poder 550W",
                Descripcion = "Fuente ATX certificada 80 Plus Bronze.", Precio = 45.90m, Imagen = "sin-imagen.png" },
            new Producto { id = 8, SeccionId = 3, Codigo = "FNT-002", Nombre = "Fuente de poder 650W",
                Descripcion = "Fuente ATX certificada 80 Plus Gold para equipos de gama media.", Precio = 69.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 9, SeccionId = 3, Codigo = "FNT-003", Nombre = "Fuente de poder 750W modular",
                Descripcion = "Fuente modular 80 Plus Gold con cables planos para mejor flujo de aire.", Precio = 89.90m, Imagen = "sin-imagen.png" },

            // 4- Procesadores
            new Producto { id = 10, SeccionId = 4, Codigo = "CPU-001", Nombre = "Procesador AMD Ryzen 5 5600",
                Descripcion = "6 núcleos y 12 hilos, socket AM4, incluye disipador.", Precio = 129.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 11, SeccionId = 4, Codigo = "CPU-002", Nombre = "Procesador Intel Core i5-12400F",
                Descripcion = "6 núcleos, socket LGA1700, requiere tarjeta gráfica dedicada.", Precio = 139.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 12, SeccionId = 4, Codigo = "CPU-003", Nombre = "Procesador AMD Ryzen 7 5700X",
                Descripcion = "8 núcleos y 16 hilos, socket AM4, para trabajo y juegos exigentes.", Precio = 189.00m, Imagen = "sin-imagen.png" },

            // 5- Tarjetas gráficas
            new Producto { id = 13, SeccionId = 5, Codigo = "GPU-001", Nombre = "Tarjeta gráfica GTX 1650 4GB",
                Descripcion = "Tarjeta de entrada para juegos en 1080p y equipos de oficina potentes.", Precio = 149.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 14, SeccionId = 5, Codigo = "GPU-002", Nombre = "Tarjeta gráfica RTX 3050 8GB",
                Descripcion = "8 GB de memoria, trazado de rayos y DLSS para juegos en 1080p.", Precio = 219.00m, Imagen = "sin-imagen.png" },
            new Producto { id = 15, SeccionId = 5, Codigo = "GPU-003", Nombre = "Tarjeta gráfica RX 6600 8GB",
                Descripcion = "8 GB de memoria, excelente rendimiento por precio en 1080p.", Precio = 239.00m, Imagen = "sin-imagen.png" }
        };

        // El id siguientes para agregar 
        private static int _siguienteid = 16;
        private static readonly object _lock = new object();

        public static List<Producto> ObtenerTodos()
        {
            lock (_lock) { return _Productos.OrderBy(p => p.id).ToList(); }
        }

        public static List<Producto> ObtenerPorSeccion(int seccionId)
        {
            lock (_lock) { return _Productos.Where(p => p.SeccionId == seccionId).OrderBy(p => p.id).ToList(); }
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
                existente.SeccionId = producto.SeccionId;
            }
        }

        public static void Eliminar(int id)
        {
            lock (_lock) { _Productos.RemoveAll(p => p.id == id); }
        }
    }
}