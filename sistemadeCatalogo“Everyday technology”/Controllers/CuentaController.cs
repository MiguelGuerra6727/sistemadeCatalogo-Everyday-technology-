using sistemadeCatalogo_Everyday_technology_.Data;
using sistemadeCatalogo_Everyday_technology_.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace sistemadeCatalogo_Everyday_technology_.Controllers
{
    public class CuentaController : Controller
    {

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Productos");
            ViewBag.ReturnUr1 = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login (LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUr1 = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var usuario = UsuarioRepositorio.Validar(model.Correo, model.Password);
            if (usuario == null)
            {
                ModelState.AddModelError("", " El correo o la contraseña son incorrectos");
                return View(model);
            }
            //Ticket con el reol 

            var ticket = new FormsAuthenticationTicket(
                1, usuario.Correo, DateTime.Now, DateTime.Now.AddMinutes(60), false, usuario.Rol);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL
            };
            Response.Cookies.Add(cookie);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

                return RedirectToAction("Index", "Productos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Catalogo");
        }


    }
}