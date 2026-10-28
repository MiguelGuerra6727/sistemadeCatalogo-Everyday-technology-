using sistemadeCatalogo_Everyday_technology_.Data;
using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace sistemadeCatalogo_Everyday_technology_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductosController : Controller
    {
        private void CargarSecciones(int? seleccionada = null)
        {
            ViewBag.Secciones = new SelectList(SeccionRepositorio.ObtenerTodas(), "id", "nombre", seleccionada);
        }
        private static readonly string[] ExtensionesPermitidas = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
      
        // GET: Productos
        public ActionResult Index()
        {
            return View(ProductoRepositorio.ObtenerTodos());
        }

        //Deytails del producto

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var producto = ProductoRepositorio.ObtnerPorId(id.Value);
            if (producto == null) return HttpNotFound();
            return View(producto);
        }

        // Crear producto Get
        public ActionResult Create()
        {
            CargarSecciones();
            return View(new Producto());
        }

        //  ProductosCreate post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producto producto, HttpPostedFileBase archivoImagen)
        {
            if (!string.IsNullOrWhiteSpace(producto.Codigo) && ProductoRepositorio.ExisteCodigo(producto.Codigo))
                ModelState.AddModelError("Codigo", "Ya existe un producto con este codigo, coloque otro porfavor");


            if (SeccionRepositorio.ObtenerPorId(producto.SeccionId) == null)
                ModelState.AddModelError("SeccionId", "Elige una seccion valida porfavor");

            string errorImagen;
            if (!ValidarImagen(archivoImagen, out errorImagen))
                ModelState.AddModelError("Imagen", errorImagen);

            if (!ModelState.IsValid)
            {
                CargarSecciones(producto.SeccionId);
                return View(producto);
            }
      

            producto.Imagen = GuardarImagen(archivoImagen) ?? "sin-imagen.png";
            ProductoRepositorio.Agregar(producto);
            TempData["Mensaje"] = "El producto creado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Productos Editar 
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var producto = ProductoRepositorio.ObtnerPorId(id.Value);
            if (producto == null) return HttpNotFound();

            CargarSecciones(producto.SeccionId);
            return View(producto);
        }

        // POST: Productos editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Producto producto, HttpPostedFileBase archivoImagen)
        {
            var original = ProductoRepositorio.ObtnerPorId(producto.id);
            if (original == null) return HttpNotFound();

            if (!string.IsNullOrWhiteSpace(producto.Codigo) && ProductoRepositorio.ExisteCodigo(producto.Codigo, producto.id))
                ModelState.AddModelError("Codigo", "Ya existe otro producto con ese código.");

            if (SeccionRepositorio.ObtenerPorId(producto.SeccionId) == null)
                ModelState.AddModelError("SeccionId", "Elige una seccion variada");

            string errorImagen;
            if (!ValidarImagen(archivoImagen, out errorImagen))
                ModelState.AddModelError("Imagen", errorImagen);

            if (!ModelState.IsValid)
            {
                producto.Imagen = original.Imagen;
                CargarSecciones(producto.SeccionId);
                return View(producto);
            }

            var nueva = GuardarImagen(archivoImagen);
            if (nueva != null)
            {
                EliminarImagenFisica(original.Imagen);
                producto.Imagen = nueva;
            }
            else
            {
                producto.Imagen = original.Imagen;
            }

            ProductoRepositorio.Actualizar(producto);
            TempData["Mensaje"] = "Producto actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Productos Delete
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var producto = ProductoRepositorio.ObtnerPorId(id.Value);
            if (producto == null) return HttpNotFound();
            return View(producto);
        }

        // POST: Productos Eliminar
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var producto = ProductoRepositorio.ObtnerPorId(id);
            if (producto != null)
            {
                EliminarImagenFisica(producto.Imagen);
                ProductoRepositorio.Eliminar(id);
                TempData["Mensaje"] = "Producto eliminado correctamente.";
            }
            return RedirectToAction("Index");
        }

        // Auxiliares para imagen (Recomendacion de la ia)

        private bool ValidarImagen(HttpPostedFileBase archivo, out string error)
        {
            error = null;
            if (archivo == null || archivo.ContentLength == 0) return true; // opcional

            var ext = Path.GetExtension(archivo.FileName).ToLower();
            if (Array.IndexOf(ExtensionesPermitidas, ext) < 0)
            {
                error = "Formato no permitido. Usa JPG, PNG, GIF o WEBP.";
                return false;
            }
            if (archivo.ContentLength > 2 * 1024 * 1024)
            {
                error = "La imagen no debe superar los 2 MB.";
                return false;
            }
            return true;
        }

        private string GuardarImagen(HttpPostedFileBase archivo)
        {
            if (archivo == null || archivo.ContentLength == 0) return null;

            var ext = Path.GetExtension(archivo.FileName).ToLower();
            var nombre = Guid.NewGuid().ToString("N") + ext;
            var carpeta = Server.MapPath("~/Content/Imagenes");
            Directory.CreateDirectory(carpeta);
            archivo.SaveAs(Path.Combine(carpeta, nombre));
            return nombre;
        }

        private void EliminarImagenFisica(string nombre)
        {
            if (string.IsNullOrEmpty(nombre) || nombre == "sin-imagen.png") return;
            var ruta = Path.Combine(Server.MapPath("~/Content/Imagenes"), nombre);
            if (System.IO.File.Exists(ruta)) System.IO.File.Delete(ruta);
        }
    }
}