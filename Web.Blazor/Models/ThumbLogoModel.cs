namespace Web.Blazor.Models
{
    public class ThumbLogoModel
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; } = "";
        public string AltName { get; set; } = "";
        public int Order { get; set; }
    }
}
