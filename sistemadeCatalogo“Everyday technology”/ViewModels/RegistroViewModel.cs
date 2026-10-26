using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Timers;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.ViewModels
{
    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(1300)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "CORREO NO VALIDO")]
        [Display(Name ="Correo Electronico")]
        public string Correo { get; set; }

        [Required(ErrorMessage ="La contraseña es obligatoria")]
        [StringLength(50, MinimumLength =6, ErrorMessage ="Minimo 6 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name ="Contraseña")]
        public string password { get; set; }
        [Required(ErrorMessage ="Confirma la contraseña")]
        [DataType(DataType.Password)]
        [Display(Name ="Confirmar contraseña")]
        public string Confirmarpassword { get; set; }
    }
}