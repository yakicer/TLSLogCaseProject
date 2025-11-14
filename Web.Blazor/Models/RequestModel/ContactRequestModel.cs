using Refit;

namespace Web.Blazor.Models.RequestModel
{
    public class ContactRequestModel
    {
        [AliasAs("MapHtmlCode")]
        public string MapHtmlCode { get; set; }

        [AliasAs("ContactAddress")]
        public string ContactAddress { get; set; }

        [AliasAs("ContactPhone1")]
        public string ContactPhone1 { get; set; }

        [AliasAs("ContactPhone2")]
        public string ContactPhone2 { get; set; }

        [AliasAs("ContactEmail")]
        public string ContactEmail { get; set; }

    }
}
