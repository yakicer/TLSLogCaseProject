using System.ComponentModel.DataAnnotations;

namespace Web.Blazor.Models.RequestModel
{
    public class ResetPasswordRequestModel
    {
        [Required(ErrorMessage = "Şifre alanı zorunludur!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifre doğrulama alanı zorunludur!")]
        [Compare("Password", ErrorMessage = "Şifreler birbiri ile eşleşmiyor!")]
        public string ConfirmPassword { get; set; }

        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
