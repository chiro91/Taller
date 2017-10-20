using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TF_Base.Models
{
    public class Registro
    {
        [StringLength(20, ErrorMessage = "La cantaidad maxima es 20 caracteres")]
        [Display(Name = "Nombre de usuario")]
        [Required]
        public string UserName { get; set; }

        [StringLength(30, ErrorMessage = "La cantaidad maxima es 30 caracteres")]
        [Display(Name = "Nombre")]
        [Required]
        public string Nombre { get; set; }

        [StringLength(20, ErrorMessage = "La cantaidad maxima es 20 caracteres")]
        [Display(Name = "Apellido")]
        [Required]
        public string Apellido { get; set; }

        [Display(Name = "Dni")]
        [Required]
        public string Dni { get; set; }
        
        
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "La cantaidad maxima es 50 caracteres")]
        [EmailAddress(ErrorMessage = "Direccion de mail no valida")]
        [Display(Name = "Email")]
        [Required]
        public string mail { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(25, ErrorMessage = "La contraseña es muy corta o muy larga", MinimumLength = 6)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Repita su contraseña")]
        [Required]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string PasswordConfirmation { get; set; }
    }

    public class Login
    {
        [Display(Name = "Nombre de usuario")]
        [Required]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Mantener sesion iniciada")]
        public bool RememberMe { get; set; }
    }
}