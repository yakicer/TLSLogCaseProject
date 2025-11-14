using System.ComponentModel.DataAnnotations;

namespace Entities.RequestModel
{
    public class UserLoginModel
    {
        /// <example>mail@example.com</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Email { get; set; }
        /// <example>***Password***</example> //
        [Required(ErrorMessage = "Zorunlu Alan")]
        public string Password { get; set; }
    }
}
