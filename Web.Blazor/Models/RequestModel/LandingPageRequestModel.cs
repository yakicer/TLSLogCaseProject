using Refit;

namespace Web.Blazor.Models.RequestModel
{
    public class LandingPageRequestModel
    {
        [AliasAs("Title")]
        public string Title { get; set; }

        [AliasAs("SubTitle")]
        public string SubTitle { get; set; }

        [AliasAs("Description")]
        public string Description { get; set; }

        [AliasAs("MainImage")]
        public StreamPart MainImage { get; set; }

        [AliasAs("Order")]
        public int Order { get; set; }
    }
}
