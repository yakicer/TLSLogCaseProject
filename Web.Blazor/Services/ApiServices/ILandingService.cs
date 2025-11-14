using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface ILandingService
    {
        [Get("/Home/GetAll")]
        Task<List<LandingModel>> GetAllAsync();

        [Get("/Home/GetById/{id}")]
        Task<LandingModel?> GetByIdAsync(Guid id);

        [Delete("/Home/Delete/{id}")]
        Task<ApiResponse<string>> DeleteLandingAsync(Guid id);

        [Multipart]
        [Post("/Home/Create")]
        Task<ApiResponse<string>> CreateLandingAsync(MultipartFormDataContent form);

        [Multipart]
        [Put("/Home/Update/{id}")]
        Task<ApiResponse<string>> UpdateLandingAsync(Guid id, MultipartFormDataContent form);

        [Put("/Home/UpdateHomeOrder")]
        Task<ApiResponse<string>> UpdateLandingOrderAsync(Guid id, int order);
    }
}
