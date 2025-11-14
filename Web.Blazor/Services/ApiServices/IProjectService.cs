using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;
using Web.Blazor.Models.Enums;
using Web.Blazor.Models.RequestModel;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface IProjectService
    {
        [Get("/Project/getall")]
        Task<List<ProjectModel>> GetAll();

        [Get("/Project/GetProjectsForSolution")]
        Task<List<ProjectSolutionModel>> GetProjectsForSolution();

        [Get("/Project/GetById")]
        Task<ProjectModel?> GetById(Guid id);

        [Get("/Project/GetByCompletedStatus")]
        Task<List<ProjectModel>> GetByCompleteStatus(bool isCompleted);

        [Get("/Project/GetByProjectType")]
        Task<List<ProjectModel>> GetBySolutionType(SolutionTypes type);

        [Delete("/Project/Delete/{id}")]
        Task<ApiResponse<string>> Delete(Guid id);

        [Multipart]
        [Post("/Project/Create")]
        Task<ApiResponse<string>> CreateProjectAsync(MultipartFormDataContent form);

        [Multipart]
        [Put("/Project/Update/{id}")]
        Task<ApiResponse<string>> Update(Guid id, MultipartFormDataContent form);

    }
}
