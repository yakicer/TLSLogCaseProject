using System.ComponentModel.DataAnnotations;

namespace Web.Blazor.Models.RequestModel
{
    public class ForgotPasswordRequestModel
    {
        [Required(ErrorMessage = "Mail adresi alanı zorunludur!")]
        public string Email { get; set; }
    }
}
