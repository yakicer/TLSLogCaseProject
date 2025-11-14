
using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface ICareerService
    {
        [Get("/Career/GetAll")]
        Task<List<CareerModel>> GetAllAsync();

        [Get("/Career/GetById/{id}")]
        Task<CareerModel?> GetByIdAsync(Guid id);

        [Get("/Career/GetByDepartmentType")]
        Task<List<CareerModel>> GetByDepartment(CareerModel departmentType);

        [Delete("/Career/Delete/{id}")]
        Task<ApiResponse<string>> DeleteCareerAsync(Guid id);

        [Multipart]
        [Post("/Career/Create")]
        Task<ApiResponse<string>> CreateCareerAsync(MultipartFormDataContent form);

        [Multipart]
        [Put("/Career/Update/{id}")]
        Task<ApiResponse<string>> UpdateCareerAsync(Guid id, MultipartFormDataContent form);
    }
}
