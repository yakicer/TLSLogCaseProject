using Refit;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models.RequestModel
{
    public class EmployeeRequestModel
    {
        [AliasAs("Name")]
        public string Name { get; set; }

        [AliasAs("SurName")]
        public string SurName { get; set; }

        [AliasAs("Title")]
        public string Title { get; set; }

        [AliasAs("Description")]
        public string? Description { get; set; } = string.Empty;

        [AliasAs("BirthDate")]
        public DateTime BirthDate { get; set; }

        [AliasAs("MainImage")]
        public StreamPart MainImage { get; set; }

        [AliasAs("DepartmentType")]
        public DepartmentTypes DepartmentType { get; set; }
    }
}
