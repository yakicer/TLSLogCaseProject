namespace Web.Blazor.Models
{
    public class LandingModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = "";
        public string SubTitle { get; set; } = "";
        public string Description { get; set; } = "";
        public string MainImagePath { get; set; } = "";
        public string ThumbImagePath { get; set; } = "";
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
