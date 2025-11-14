using System.ComponentModel.DataAnnotations;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models
{
    public class CareerModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string SurName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        [Required]
        public string Phone { get; set; } = "";
        public DepartmentTypes Department { get; set; }
        public string Description { get; set; } = "";
        public string CVFilePath { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    }
}
