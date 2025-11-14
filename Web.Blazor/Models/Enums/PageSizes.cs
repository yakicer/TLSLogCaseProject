using System.ComponentModel.DataAnnotations;

namespace Web.Blazor.Models.Enums
{
    public enum PageSizes
    {
        [Display(Description = "1 kayıt gösteriliyor")]
        One = 1,
        [Display(Description = "5 kayıt gösteriliyor")]
        Five = 5,
        [Display(Description = "10 kayıt gösteriliyor")]
        Ten = 10,
        [Display(Description = "20 kayıt gösteriliyor")]
        Twenty = 20,
        [Display(Description = "50 kayıt gösteriliyor")]
        Fifty = 50,
        [Display(Description = "100 kayıt gösteriliyor")]
        Hundred = 100,
    }
}
