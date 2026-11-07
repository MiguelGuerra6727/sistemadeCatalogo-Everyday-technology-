using sistemadeCatalogo_Everyday_technology_.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sistemadeCatalogo_Everyday_technology_.Data
{
    public class SeccionRepositorio
    {
       public static List<Seccion> ObtenerTodas()
        {
            using (var db = new InventarioContext())
            {
                return db.Seccions.AsNoTracking()
                    .Where(s => s.Activo)
                    .OrderBy(s => s.Id)
                    .ToList();
            }
        }

        public static Seccion ObtenerPorId (int id)
        {
         using (var db = new InventarioContext())
            {
                return db.Seccions.AsNoTracking()
                    .FirstOrDefault(s => s.Id == id && s.Activo);
            }
        }
     } // Class Selccion
}//Fin del namepce
