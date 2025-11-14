using Refit;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models.RequestModel
{
    public class DocumentRequestModel
    {
        [AliasAs("DocumentTitle")]
        public string DocumentTitle { get; set; }

        [AliasAs("DocumentDescription")]
        public string? DocumentDescription { get; set; }

        [AliasAs("DocumentType")]
        public DocumentTypes DocumentType { get; set; }

        [AliasAs("Document")]
        public StreamPart Document { get; set; }

        [AliasAs("MainImage")]
        public StreamPart MainImage { get; set; }
    }
}
