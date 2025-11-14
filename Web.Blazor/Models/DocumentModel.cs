using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models
{
    public class DocumentModel
    {
        public Guid Id { get; set; }
        public string DocumentTitle { get; set; } = "";
        public string DocumentUrl { get; set; } = "";
        public string DocumentDescription { get; set; } = "";
        public string MainImagePath { get; set; } = "";
        public string ThumbImagePath { get; set; } = "";
        public DocumentTypes DocumentType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    }
}
