using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sistemadeCatalogo_Everyday_technology_.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage ="Correo no valido")]
        [Display(Name= "Correo electronico")]
        public string Correo { get; set; }

        [Required(ErrorMessage ="La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name= "Password")]
        public string Password { get; set; }

        [Display(Name ="Recordarme")]
        public bool Recordarme { get; set; }
    }
}