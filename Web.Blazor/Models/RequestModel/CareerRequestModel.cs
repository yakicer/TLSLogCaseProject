using Refit;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Models.RequestModel
{
    public class CareerRequestModel
    {
        [AliasAs("Name")]
        public string Name { get; set; }

        [AliasAs("SurName")]
        public string SurName { get; set; }

        [AliasAs("Email")]
        public string Email { get; set; }

        [AliasAs("Phone")]
        public string Phone { get; set; }

        [AliasAs("Description")]
        public string Description { get; set; }

        [AliasAs("CVFile")]
        public StreamPart CVFile { get; set; }

        [AliasAs("Department")]
        public DepartmentTypes Department { get; set; }

    }
}
