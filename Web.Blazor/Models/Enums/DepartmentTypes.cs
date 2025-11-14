using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Blazor.Models.Enums
{
    public enum DepartmentTypes
    {
        [Display(Description = "Diğer")]
        [Description("Others")]
        Others,
        [Display(Description = "Ofis")]
        [Description("Office")]
        Office,
        [Display(Description = "Pazarlama")]
        [Description("Marketing")]
        Marketing,
        [Display(Description = "Satış")]
        [Description("Sales")]
        Sales,
        [Display(Description = "Mimarlık")]
        [Description("Architectural")]
        Architectural,
        [Display(Description = "Dizayn")]
        [Description("Design")]
        Design,
        [Display(Description = "Operasyon")]
        [Description("Operations")]
        Operations,
        [Display(Description = "Mühendislik")]
        [Description("Engineering")]
        Engineering,
        [Display(Description = "Finans")]
        [Description("Finance")]
        Finance,
        [Display(Description = "Teknoloji")]
        [Description("Technology")]
        Technology
    }
}
