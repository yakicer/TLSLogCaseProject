using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models
{
    public class EmployeeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public string SurName { get; set; } = "";
        public string Title { get; set; } = "";
        public string MainImagePath { get; set; } = "";
        public string ThumbImagePath { get; set; } = "";
        public DateTime BirthDate { get; set; } = DateTime.Now;
        public string Description { get; set; } = "";
        public DepartmentTypes DepartmentType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
