using sistemadeCatalogo_Everyday_technology_.Data;
using sistemadeCatalogo_Everyday_technology_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace sistemadeCatalogo_Everyday_technology_.Controllers
{
    public class CuentaController : Controller
    {

        // GET: Cuenta El login
        [AllowAnonymous]
        public ActionResult Login(string returnUr1)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Idex", "Catalogo");
            ViewBag.ReturnUrl = returnUr1;
            return View();
        }
        //Post de la cuenta login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginView, string returnUr1)
        {
            if (!ModelState.IsValid) return View(loginView);

            var usuario = UsuarioRepositorio.Validar(loginView.Correo, loginView.Password);
            if (usuario == null)
            {
                ModelState.AddModelError("", "El correo o la contraseña son incorrectas");
                return View(loginView);
            }
            FormsAuthentication.SetAuthCookie(usuario.Correo, loginView.Recordarme);

            if (!string.IsNullOrEmpty(returnUr1) && Url.IsLocalUrl(returnUr1))
                return Redirect(returnUr1);

            return RedirectToAction("Index", "Catalogo");
        }
        //Cuenta del registro
        [AllowAnonymous]
        public ActionResult Registro()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Catalogo");
            return View();
        }
        //Post registro de la cuenta 
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Registro(RegistroViewModel viewModel)
        {
            if (UsuarioRepositorio.ExisteCorreo(viewModel.Correo ?? ""))
                ModelState.AddModelError("Correo", "Este correo ya esta reistrado");

            if (!ModelState.IsValid) return View(viewModel);
            UsuarioRepositorio.Registrar(viewModel.Nombre, viewModel.Correo, viewModel.password);
            TempData["Mensaje"] = "Cuenta creada correctamente";
            return RedirectToAction("Login");
        }
        //Post Cuenta logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }

}