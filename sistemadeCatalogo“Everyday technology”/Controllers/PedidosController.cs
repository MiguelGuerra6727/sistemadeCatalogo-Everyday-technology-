
using sistemadeCatalogo_Everyday_technology_.Models;
using sistemadeCatalogo_Everyday_technology_.Data;
using sistemadeCatalogo_Everyday_technology_.ViewModels;

using System.Linq;
using System.Web.Mvc;


namespace sistemadeCatalogo_Everyday_technology_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PedidosController : Controller
    {
        // GET: Pedidos
        public ActionResult Index()
        {
            var pedidos = PedidoRepositorio.ObtenerTodos();
            var vm = new PedidosIndexViewModels
            {
                pedidos = pedidos,
                TotalVentas = pedidos.Where(p => p.Estado != EstadoPedido.Cancelado).Sum(p => p.Total)
            };
            return View(vm);
        }
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(400);
            var pedido = PedidoRepositorio.ObtenerPorId(id.Value);
            if (pedido == null) return HttpNotFound();
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CambiarEstado(int id, string estado, string returnUrl)
        { //El cabmbio de estado
            var pedido = PedidoRepositorio.ObtenerPorId(id);
            if (pedido == null) return HttpNotFound();
        
            if(PedidoRepositorio.CambiarEstado(id, estado))
            {
                TempData["PedidoMensaje"] = estado == EstadoPedido.Entregado
                    ?"pedido #" + id + " Entregado correctamente "
                    : "pedido #" + id + " Cancelado. los productos volvieron al stock ";
            }
            else
            {
                TempData["pedidoError"] = " No se pudo cambiar el estado: el pedido ya fue entregado o cancelado ";
            }
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index");
        }// fin del cambio de estado 
       
        
    } // fin de administrador
}