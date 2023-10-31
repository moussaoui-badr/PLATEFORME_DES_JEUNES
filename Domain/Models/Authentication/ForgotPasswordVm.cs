using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Authentication
{
    public class ForgotPasswordVm
    {
        [Required]
        public string Email { get; set; }
        public bool NewPasswordSent { get; set; }
        public ForgotPasswordEmailVm ForgotPasswordEmailVm { get; set; }
    }
}
