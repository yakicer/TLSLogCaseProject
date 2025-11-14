using System.ComponentModel.DataAnnotations;
namespace Web.Blazor.Models.RequestModel
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
