using Refit;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models.RequestModel
{
    public class SolutionRequestModel
    {
        [AliasAs("Title")]
        public string Title { get; set; }

        [AliasAs("SubTitle")]
        public string SubTitle { get; set; }

        [AliasAs("SolutionType")]
        public SolutionTypes SolutionType { get; set; }

        [AliasAs("MainImage")]
        public StreamPart MainImage { get; set; }

        [AliasAs("IsImageDeleted")]
        public bool IsImageDeleted { get; set; } = false;
    }
}
