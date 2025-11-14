using System.ComponentModel.DataAnnotations;
namespace Web.Blazor.Models.RequestModel
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        [MinLength(10, ErrorMessage = "Lütfen Başında Sıfır Olmadan 10 Haneli Şekilde Giriniz"), MaxLength(10, ErrorMessage = "Lütfen Başında Sıfır Olmadan 10 Haneli Şekilde Giriniz")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Lütfen Geçerli Email Giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Zorunlu Alan")]
        [Compare("Password", ErrorMessage = "Şifreler birbiri ile eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
