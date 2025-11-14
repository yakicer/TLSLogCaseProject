using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models
{
    public class SolutionModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string SubTitle { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public string ThumbImagePath { get; set; } = "";
        public bool IsImageDeleted { get; set; } = false;
        public SolutionTypes SolutionType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
