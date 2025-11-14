using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Blazor.Models.Enums
{
    public enum DocumentTypes
    {
        [Display(Description = "Dosya")]
        [Description("File")]
        File,
        [Display(Description = "Döküman")]
        [Description("Document")]
        Document,
        [Display(Description = "Görsel")]
        [Description("Image")]
        Image
    }
}
