using sistemadeCatalogo_Everyday_technology_.Data;
using sistemadeCatalogo_Everyday_technology_.Models;
using sistemadeCatalogo_Everyday_technology_.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace sistemadeCatalogo_Everyday_technology_.Controllers
{
    public class CarritoController : Controller
    {
        // Esta parte hace Producyo id va ir a cantidad 

        private Dictionary<int, int> ObtenerCarrito()
        {
            var carrito = Session["Carrito"] as Dictionary<int, int>;
            if (carrito == null)
            {
                carrito = new Dictionary<int, int>();
                Session["Carrito"] = carrito;
            }
            return carrito;
        } // fin de obtenercarrito

        private CarritoViewModels ConstruirCarrito()
        {
            var carrito = ObtenerCarrito();
            var vm = new CarritoViewModels { Lineas = new List<CarritoLineaViewModels>() };
            foreach (var par in carrito.ToList())
            {
                var producto = ProductoRepositorio.ObtenerPorId(par.Key);
                if (producto == null) { carrito.Remove(par.Key); continue; }
                vm.Lineas.Add(new CarritoLineaViewModels { Producto = producto, Cantidad = par.Value });
            }
            return vm;
        } //Fin del Construir carrito

        private ActionResult Volver(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index");
        }// fin de accion de volver a Index

        [ChildActionOnly] //Se supone que creara el icono con la cantidad de productos
        public ActionResult Contador()
        {
            int total = ObtenerCarrito().Values.Sum();
            return PartialView("_CarritoIcono", total);
        }

        public ActionResult Index()
        {
            return View(ConstruirCarrito());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(int id, int cantidad = 1, string returnUrl = null)
        { //Agregar inicio 

            var producto = ProductoRepositorio.ObtenerPorId(id);
            if (producto == null) return HttpNotFound();

            if (cantidad < 1) cantidad = 1;
            if (cantidad > 10000) cantidad = 10000;

            if (producto.Stock <= 0)
            {
                TempData["carritoError"] = producto.Nombre + " este producto esta agotado ";
                return Volver(returnUrl);
            }
            var carrito = ObtenerCarrito();
            int actual = carrito.ContainsKey(id) ? carrito[id] : 0;
            int nueva = actual + cantidad;

            if (nueva > producto.Stock)
            {
                nueva = producto.Stock;
                TempData["CarritoError"] = " solo hay " + producto.Stock + " unidades disponible de " + producto.Nombre + "";
            }
            else
            {
                TempData["CarritoMensaje"] = producto.Nombre + " producto agregado al carrito ";
            }
            carrito[id] = nueva;
            return Volver(returnUrl);
        }//  cierre para agregar Agregar

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(int id, int cantidad)
        { // inicio actualizar 

            var carrito = ObtenerCarrito();
            var producto = ProductoRepositorio.ObtenerPorId(id);

            if (producto == null || producto.Stock <= 0)
            {
                carrito.Remove(id);
                TempData["CarritoError"] = " El prodicto no esta disponible y se quito del carrito ";
                return RedirectToAction("Index");
            }
            if (cantidad < 1) cantidad = 1;
            if (cantidad > producto.Stock)
            {
                cantidad = producto.Stock;
                TempData["CarritoError"] = " Solo hay " + producto.Stock + " Unidades disponible de " + producto.Nombre + "";
            }
            if (carrito.ContainsKey(id)) carrito[id] = cantidad;
            return RedirectToAction("Index");
        }//fin actualizar 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quitar(int id)
        {//Inicio quiar 
            ObtenerCarrito().Remove(id);
            return RedirectToAction("Index");
        }//Fin quitar 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Vaciar()
        {//Inicio limpiar 
            ObtenerCarrito().Clear();
            return RedirectToAction("Index");
        }//Fin limpiar

        [HttpGet]
        public ActionResult Checkout()
        {
            var carrito = ConstruirCarrito();
            if (carrito.Lineas.Count == 0) return RedirectToAction("Index");
            return View(new CheckoutViewModels { Carrito = carrito });
            
        }
        //Simulacion del pago

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutViewModels model)
        {
            var carrito = ConstruirCarrito();
            if (carrito.Lineas.Count == 0) return RedirectToAction("Index");

            model.Carrito = carrito;
            if (!ModelState.IsValid) return View(model);

            var cantidades = carrito.Lineas.ToDictionary(l => l.Producto.id, l => l.Cantidad);

            var pedido = new pedido
            {
                Cliente = model.Nombre.Trim(),
                Telefono = model.Telefono.Trim(),
                DireccionEnvio = model.DireccionEnvio.Trim(),
                Estado = EstadoPedido.Pendiente,
                Items = carrito.Lineas.Select(l => new PedidoItem
                {
                    ProductoId = l.Producto.id,
                    Codigo = l.Producto.Codigo,
                    Nombre = l.Producto.Nombre,
                    PrecioUnitario = l.Producto.Precio,
                    Cantidad = l.Cantidad
                }).ToList()
            };

            string error = PedidoRepositorio.crear(pedido, cantidades);
            if (error != null)
            {
                TempData["CarritoError"] = error + " Adjunta tu carrito e intenta de nuevo ";
                return RedirectToAction("Index");
            }
            ObtenerCarrito().Clear();
            TempData["PedidoNumero"] = pedido.Id;
            TempData["PedidoTotal"] = pedido.Total;
            return RedirectToAction("Gracias");
        }
        public ActionResult Gracias()
        {
            if (TempData["PedidoNumero"] == null) return RedirectToAction("Index", "Catalogo");
            ViewBag.Numero = TempData["PedidoNumero"];
            ViewBag.Total = TempData["PedidoTotal"];
            return View();
        }
    } //Fin cheakOut
}
