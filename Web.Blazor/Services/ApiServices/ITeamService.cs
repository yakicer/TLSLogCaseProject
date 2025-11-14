using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface ITeamService
    {
        [Get("/Team/GetAll")]
        Task<List<EmployeeModel>> GetAllAsync();

        [Get("/Team/GetByDepartmentType")]
        Task<List<EmployeeModel>> GetByDepartmentType(DepartmentTypes departmentType);

        [Get("/Team/GetById/{id}")]
        Task<EmployeeModel?> GetByIdAsync(Guid id);

        [Delete("/Team/Delete/{id}")]
        Task<ApiResponse<string>> DeleteEmployeeAsync(Guid id);

        [Multipart]
        [Post("/Team/Create")]
        Task<ApiResponse<string>> CreateEmployeeAsync(MultipartFormDataContent form);

        [Multipart]
        [Put("/Team/Update/{id}")]
        Task<ApiResponse<string>> UpdateEmployeeAsync(Guid id, MultipartFormDataContent form);
    }
}
