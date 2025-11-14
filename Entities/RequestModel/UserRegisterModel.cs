using System.ComponentModel.DataAnnotations;

namespace Entities.RequestModel
{
    public class UserRegisterModel
    {
        /// <example>John</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Name { get; set; }
        /// <example>Doe</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Surname { get; set; }
        /// <example>05554443322</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        [MinLength(10, ErrorMessage = "Lütfen Başında Sıfır Olmadan 10 Haneli Şekilde Giriniz"), MaxLength(10, ErrorMessage = "Lütfen Başında Sıfır Olmadan 10 Haneli Şekilde Giriniz")]
        public string PhoneNumber { get; set; }
        /// <example>mail@example.com</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Lütfen Geçerli Email Giriniz")]
        public string Email { get; set; }
        /// <example>***Password***</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Password { get; set; }
        /// <example>***Password***(confirm)</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        [Compare("Password", ErrorMessage = "Şifreler birbiri ile eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}
