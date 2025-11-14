using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface ISystemManagementService
    {
        [Get("/SystemSettings/GetAll")]
        Task<List<SystemSettingsModel>> GetAllAsync();

        [Get("/SystemSettings/GetById/{id}")]
        Task<SystemSettingsModel?> GetByIdAsync(Guid id);

        [Get("/SystemSettings/GetDefaultSettings")]
        Task<SystemSettingsModel> GetDefaultSettings();

        [Multipart]
        [Put("/SystemSettings/Update/{id}")]
        Task<ApiResponse<string>> UpdateSystemSettingsAsync(Guid id, MultipartFormDataContent form);
    }
}
