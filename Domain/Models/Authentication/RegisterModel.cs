using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Authentication
{
#nullable disable
    public class RegisterModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Societe is required")]
        public int SocieteID { get; set; }

        [Required(ErrorMessage = "Tableau de Bord is required")]
        public int? TableauBordID { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string RoleName { get; set; }
        [BindProperty(Name = "g-recaptcha-response")]
        public string CaptchaResponse { get; set; }

    }
}
