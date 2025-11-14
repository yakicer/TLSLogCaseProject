using System.ComponentModel.DataAnnotations;

namespace Web.UI.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required, EmailAddress] public string Email { get; set; } = default!;
        [Required] public string Password { get; set; } = default!;
    }
}
