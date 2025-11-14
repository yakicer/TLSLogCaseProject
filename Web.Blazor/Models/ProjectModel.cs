using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models
{
    public class ProjectModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public SolutionTypes ProjectType { get; set; }
        public bool IsCompleted { get; set; }
        public string MainImagePath { get; set; } = "";
        public string ThumbImagePath { get; set; } = "";
        public virtual ICollection<ProjectImageModel> ProjectImage { get; set; }
        public string ClientName { get; set; } = "";
        public string Year { get; set; } = "";
        public string ArchitectName { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
