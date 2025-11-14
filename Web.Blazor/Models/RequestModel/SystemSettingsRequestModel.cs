using Refit;

namespace Web.Blazor.Models.RequestModel
{
    public class SystemSettingsRequestModel
    {
        [AliasAs("AboutSection")]
        public string AboutSection { get; set; }

        [AliasAs("TwitterLink")]
        public string TwitterLink { get; set; }

        [AliasAs("InstagramLink")]
        public string InstagramLink { get; set; }

        [AliasAs("LinkedInLink")]
        public string LinkedInLink { get; set; }

        [AliasAs("MailUrl")]
        public string MailUrl { get; set; }

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
