using sistemadeCatalogo_Everyday_technology_.Data;
using sistemadeCatalogo_Everyday_technology_.Helpers;
using sistemadeCatalogo_Everyday_technology_.Models;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace sistemadeCatalogo_Everyday_technology_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SeccionesController : Controller
    {
        // Lista de todas las secciones (activas y desactivadas)
        public ActionResult Index()
        {
            return View(SeccionRepositorio.ObtenerTodasAdmin());
        }

        // Crear (GET)
        public ActionResult Create()
        {
            return View(new Seccion());
        }

        // Crear (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nombre,Descripcion")] Seccion seccion, HttpPostedFileBase archivoImagen)
        {
            if (!string.IsNullOrWhiteSpace(seccion.Nombre) && SeccionRepositorio.ExisteNombre(seccion.Nombre))
                ModelState.AddModelError("Nombre", "Ya existe una sección con ese nombre.");

            string errorImagen;
            if (!ImagenHelper.Validar(archivoImagen, out errorImagen))
                ModelState.AddModelError("Imagen", errorImagen);

            if (!ModelState.IsValid) return View(seccion);

            seccion.Imagen = ImagenHelper.Guardar(archivoImagen, Server) ?? ImagenHelper.ImagenPordefecto;
            SeccionRepositorio.Agregar(seccion);

            TempData["SeccionMensaje"] = "Sección creada correctamente.";
            return RedirectToAction("Index");
        }

        // Editar (GET)
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var seccion = SeccionRepositorio.ObtenerPorIdAdmin(id.Value);
            if (seccion == null) return HttpNotFound();
            return View(seccion);
        }

        // Editar (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Descripcion")] Seccion seccion, HttpPostedFileBase archivoImagen)
        {
            var original = SeccionRepositorio.ObtenerPorIdAdmin(seccion.Id);
            if (original == null) return HttpNotFound();

            if (!string.IsNullOrWhiteSpace(seccion.Nombre) && SeccionRepositorio.ExisteNombre(seccion.Nombre, seccion.Id))
                ModelState.AddModelError("Nombre", "Ya existe otra sección con ese nombre.");

            string errorImagen;
            if (!ImagenHelper.Validar(archivoImagen, out errorImagen))
                ModelState.AddModelError("Imagen", errorImagen);

            if (!ModelState.IsValid)
            {
                seccion.Imagen = original.Imagen;   // para que la vista siga mostrando la imagen actual
                return View(seccion);
            }
            string nueva = ImagenHelper.Guardar(archivoImagen, Server);
            if (nueva != null)
            {
                ImagenHelper.Eliminar(original.Imagen, Server);   
                seccion.Imagen = nueva;
            }
            else
            {
                seccion.Imagen = original.Imagen;
            }

            SeccionRepositorio.Actualizar(seccion);
            TempData["SeccionMensaje"] = "Sección actualizada correctamente.";
            return RedirectToAction("Index");
        }

        // Activar o desactivar (baja lógica)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, bool activar)
        {
            var seccion = SeccionRepositorio.ObtenerPorIdAdmin(id);
            if (seccion == null) return HttpNotFound();

            if (!activar && SeccionRepositorio.ContarProductosActivos(id) > 0)
            {
                TempData["SeccionError"] = "No puedes desactivar \"" + seccion.Nombre +
                    "\" porque tiene productos activos. Muévelos a otra sección o elimínalos primero.";
                return RedirectToAction("Index");
            }

            SeccionRepositorio.CambiarEstado(id, activar);
            TempData["SeccionMensaje"] = activar
                ? "Sección \"" + seccion.Nombre + "\" activada."
                : "Sección \"" + seccion.Nombre + "\" desactivada.";
            return RedirectToAction("Index");
        }
    }
}