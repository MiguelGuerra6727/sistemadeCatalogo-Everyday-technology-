using sistemadeCatalogo_Everyday_technology_.Models;
using sistemadeCatalogo_Everyday_technology_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Routing;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class SeccionRepositorio
    {
        //Catalogo publico de los activos 
        public static List<Seccion> ObtenerTodas()
        {
            using (var db = new InventarioContext())
            {
                return db.Seccions.AsNoTracking()
                    .Where(s => s.Activo)
                    .OrderBy(s => s.Id)
                    .ToList();
            }
        }

        public static Seccion ObtenerPorId(int id)
        {
            using (var db = new InventarioContext())
            {
                return db.Seccions.AsNoTracking()
                    .FirstOrDefault(s => s.Id == id && s.Activo);
            }
        }
        //Fin de el catalogo de activos 

        //Para el Admin
        public static List<SeccionAdminViewModel> ObtenerTodasAdmin()
        {
            using (var db = new InventarioContext())
            {
                return db.Seccions.AsNoTracking()
                    .OrderBy(s => s.Id)
                    .Select(s => new SeccionAdminViewModel
                    {
                        Seccion = s,
                        ProductosActivos = s.Productos.Count(p => p.Activo)
                    })
                    .ToList();
            }
        } // fin de List

        public static Seccion ObtenerPorIdAdmin(int id)
        {
            using (var db = new InventarioContext())
            {
                return db.Seccions.AsNoTracking().FirstOrDefault(s => s.Id == id);
            }
        } //fin de seccion

        public static bool ExisteNombre(string nombre, int? idExcluir = null)
        {
            string n = (nombre ?? "").Trim().ToLower();
            int excluir = idExcluir ?? 0;
            using (var db = new InventarioContext())
            {
                return db.Seccions.Any(s => s.Nombre.ToLower() == n && s.Id != excluir);
            }
        } //Fin de existirNombre

        public static int ContarProductosActivos(int id)
        {
            using (var db = new InventarioContext())
            {
                return db.Productos.Count(p => p.SeccionId == id && p.Activo);
            }
        } //Fin ContarProductos 

        public static void Agregar(Seccion seccion)
        {
            using (var db = new InventarioContext())
            {
                seccion.Nombre = seccion.Nombre.Trim();
                seccion.Activo = true;
                db.Seccions.Add(seccion);
                db.SaveChanges();
            }
        }//Fin agregar 

        public static void Actualizar(Seccion seccion)
        {
            using (var db = new InventarioContext())
            {
                var existente = db.Seccions.FirstOrDefault(s => s.Id == seccion.Id);
                if (existente == null) return;

                existente.Nombre = seccion.Nombre.Trim();
                existente.Descripcion = seccion.Descripcion;
                existente.Imagen = seccion.Imagen;
                db.SaveChanges();
            }
        } //actualiar seccion
        public static void CambiarEstado(int id, bool activo)
        {
            using (var db = new InventarioContext())
            {
                var existente = db.Seccions.FirstOrDefault(s => s.Id == id);
                if (existente == null) return;

                existente.Activo = activo;
                db.SaveChanges();
            }
        } // Class Selccion
    }
}//Fin del namepce
