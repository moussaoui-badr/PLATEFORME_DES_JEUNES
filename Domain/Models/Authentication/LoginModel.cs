using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Authentication
{
#nullable disable
    public class LoginModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public bool KeepLogged { get; set; }
        [BindProperty(Name = "g-recaptcha-response")]
        public string CaptchaResponse { get; set; }
    }
}
