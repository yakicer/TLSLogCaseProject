using Refit;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models.RequestModel
{
    public class ProjectRequestMultipartModel
    {
        [AliasAs("Name")]
        public string Name { get; set; }

        [AliasAs("ProjectType")]
        public SolutionTypes ProjectType { get; set; }

        [AliasAs("IsCompleted")]
        public bool IsCompleted { get; set; }

        [AliasAs("ClientName")]
        public string ClientName { get; set; }

        [AliasAs("Year")]
        public string Year { get; set; }

        [AliasAs("ArchitectName")]
        public string ArchitectName { get; set; }

        [AliasAs("Location")]
        public string Location { get; set; }

        [AliasAs("MainImage")]
        public StreamPart MainImage { get; set; }

        [AliasAs("AdditionalImages")]
        public List<StreamPart>? AdditionalImages { get; set; } = new();

        [AliasAs("HasAdditionalImages")]
        public bool HasAdditionalImages { get; set; } = true;

        [AliasAs("RemovedAdditionalImages")]
        public List<Guid>? RemovedAdditionalImagesIds { get; set; } = new();

    }
}
