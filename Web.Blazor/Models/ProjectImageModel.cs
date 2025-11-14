namespace Web.Blazor.Models
{
    public class ProjectImageModel
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public virtual ProjectModel Project { get; set; }
        public string ImagePath { get; set; } = "";
        public string ThumbImagePath { get; set; } = "";
        public string AltName { get; set; } = "";
        public bool Status { get; set; }
        public int Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
