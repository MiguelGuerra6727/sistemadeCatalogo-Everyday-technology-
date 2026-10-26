using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class SeccionRepositorio
    {
        private static readonly List<Seccion> _Secciones = new List<Seccion>
        {
            new Seccion { Id = 1, Nombre = "Memorias RAM",       Descripcion = "Módulos DDR3, DDR4 y DDR5 para PC y laptop." },
            new Seccion { Id = 2, Nombre = "Almacenamiento",     Descripcion = "Discos SSD, HDD y unidades M.2." },
            new Seccion { Id = 3, Nombre = "Fuentes de poder",   Descripcion = "Fuentes ATX de distintas potencias." },
            new Seccion { Id = 4, Nombre = "Procesadores",       Descripcion = "CPU Intel y AMD para diferentes gamas." },
            new Seccion { Id = 5, Nombre = "Tarjetas gráficas",  Descripcion = "GPU para trabajo, estudio y juegos." }
        };

        public static List<Seccion> ObtenerTodas() { return _Secciones.ToList(); }
        public static Seccion ObtenerPorId(int id)
        {
            return _Secciones.FirstOrDefault(s => s.Id ==id);
        }
     }
}
