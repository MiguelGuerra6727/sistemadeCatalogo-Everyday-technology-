using Microsoft.Ajax.Utilities;
using System;
using System.IO;
using System.Security.Policy;
using System.Web;
using System.Web.UI.WebControls;

namespace sistemadeCatalogo_Everyday_technology_.Helpers
{
    public class ImagenHelper
    {
        public const string ImagenPordefecto = "sin-imagen-png";
        private static readonly string[] Extensiones = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const int MaxBytes = 2 * 1024 * 1024;

        public static bool Validar(HttpPostedFileBase archivo, out string error)
        {
            error = null;
            if (archivo == null || archivo.ContentLength == 0) return true;

            string ext = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (Array.IndexOf(Extensiones, ext) < 0)
            {
                error = "Formato no permitido (detecté: '" + ext + "'). Usa JPG, PNG, GIF o WEBP.";
                return false;
            }
            if (archivo.ContentLength > MaxBytes)
            {
                error = "La imaagen no debe superar los 2 MB ";
                return false;
            }
            return true;
        } // fin de validar

        public static string Guardar(HttpPostedFileBase archivo, HttpServerUtilityBase server)
        {
            if (archivo == null || archivo.ContentLength == 0) return null;

            string ext = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            string nombre = Guid.NewGuid().ToString("N") + ext;
            string carpeta = server.MapPath("~/Content/Imagenes");
            Directory.CreateDirectory(carpeta);
            archivo.SaveAs(Path.Combine(carpeta, nombre));
            return nombre;
        
        } //Fin de guardar
        public static void Eliminar(string nombre, HttpServerUtilityBase server)
        {
            if (string.IsNullOrEmpty(nombre) || nombre == ImagenPordefecto) return;

            string soloNombre = Path.GetFileName(nombre);
            string ruta = Path.Combine(server.MapPath("~/Content/Imagenes"), soloNombre);
            if (File.Exists(ruta)) File.Delete(ruta);
        }
    }// fin de la clase 
} //fin de namepace