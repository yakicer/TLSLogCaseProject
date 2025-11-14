using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models.RequestModel
{
    public class ProjectRequestModel
    {
        public string Name { get; set; }
        public SolutionTypes ProjectType { get; set; }
        public bool IsCompleted { get; set; }
        public string ClientName { get; set; }
        public string Year { get; set; }
        public string ArchitectName { get; set; }
        public string Location { get; set; }
        public Refit.StreamPart? MainImage { get; set; }
        public IEnumerable<Refit.StreamPart>? AdditionalImages { get; set; }
    }
}
