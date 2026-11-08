using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.ViewModels
{
    public class SeccionAdminViewModel
    {
        public Seccion Seccion { get; set; }
        public int ProductosActivos { get; set; }
    }
}