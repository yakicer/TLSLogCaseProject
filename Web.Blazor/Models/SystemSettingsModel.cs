namespace Web.Blazor.Models
{
    public class SystemSettingsModel
    {
        public Guid Id { get; set; }
        public string AboutSection { get; set; } = "";
        public string TwitterLink { get; set; } = "";
        public string InstagramLink { get; set; } = "";
        public string LinkedInLink { get; set; } = "";
        public string MailUrl { get; set; } = "";
        public string MapHtmlCode { get; set; } = "";
        public string ContactAddress { get; set; } = "";
        public string ContactPhone1 { get; set; } = "";
        public string ContactPhone2 { get; set; } = "";
        public string ContactEmail { get; set; } = "";
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
