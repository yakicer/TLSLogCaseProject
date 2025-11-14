using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Blazor.Models.Enums
{
    public enum SolutionTypes
    {
        [Description("Exterior")]
        Exterior,
        [Description("Interior")]
        Interior,
        [Display(Description = "Proje Yönetimi")]
        [Description("ProjectManagement")]
        ProjectManagement
    }
}
