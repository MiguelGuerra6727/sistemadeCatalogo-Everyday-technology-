using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace sistemadeCatalogo_Everyday_technology_
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        { 
    AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // Lee el rol del ticket y lo pone en User para que funcione [Authorize(Roles="Admin")]
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value)) return;

            FormsAuthenticationTicket ticket;
            try { ticket = FormsAuthentication.Decrypt(cookie.Value); }
            catch { return; }

            if (ticket == null || ticket.Expired) return;

            string[] roles = (ticket.UserData ?? string.Empty).Split(',');
            Context.User = new GenericPrincipal(new FormsIdentity(ticket), roles);
        }
    }
}
