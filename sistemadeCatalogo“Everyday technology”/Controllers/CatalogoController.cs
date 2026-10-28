using sistemadeCatalogo_Everyday_technology_.Data;
using sistemadeCatalogo_Everyday_technology_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sistemadeCatalogo_Everyday_technology_.Controllers
{
    public class CatalogoController : Controller
    {
        // GET: Catalogo
        public ActionResult Index()
        {
            return View(SeccionRepositorio.ObtenerTodas());
        }
        //Prodcutos
        public ActionResult Seccion(int id)
        {
            var seccion = SeccionRepositorio.ObtenerPorId(id);
            if (seccion == null) return HttpNotFound();

            var modelo = new SeccionProductosViewModel
            {
                seccion = seccion,
                Productos = ProductoRepositorio.ObtenerPorSeccion(id)
            };
            return View(modelo);
        }
    }
}