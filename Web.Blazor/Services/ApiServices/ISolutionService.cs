using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;
using Web.Blazor.Models.Enums;
using Web.Blazor.Models.RequestModel;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface ISolutionService
    {
        [Get("/Solution/GetAll")]
        Task<List<SolutionModel>> GetAllAsync();

        [Get("/Solution/GetSolutionsForProject")]
        Task<List<SolutionProjectModel>> GetSolutionsForProject();

        [Get("/Solution/GetBySolutionType")]
        Task<List<SolutionModel>> GetBySolutionType(SolutionTypes solutionType);

        [Get("/Solution/GetById/{id}")]
        Task<SolutionModel?> GetByIdAsync(Guid id);

        [Delete("/Solution/Delete/{id}")]
        Task<ApiResponse<string>> DeleteSolutionAsync(Guid id);

        [Multipart]
        [Post("/Solution/Create")]
        Task<ApiResponse<string>> CreateSolutionAsync(MultipartFormDataContent form);

        [Multipart]
        [Put("/Solution/Update/{id}")]
        Task<ApiResponse<string>> UpdateSolutionAsync(Guid id, MultipartFormDataContent form);
    }
}
