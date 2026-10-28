using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.ViewModels
{
    public class SeccionProductosViewModel
    {
        public Seccion seccion { get; set; }
        public List<Producto> Productos { get; set; }
    }
}