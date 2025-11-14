using Web.Blazor.Models;

namespace Web.Blazor.Services.ApiServices
{
    public interface IProjectImageService
    {
        Task AddRange(IEnumerable<ProjectImageModel> projectImages);
        Task<ProjectImageModel> GetByIdAsync(Guid projectImageId);
        ProjectImageModel GetById(Guid projectImageId);
        List<ProjectImageModel> GetAll();
        List<ProjectImageModel> GetAllByProjectId(Guid id);
        Task DeleteAllByProjectId(Guid projectId);

    }
}
