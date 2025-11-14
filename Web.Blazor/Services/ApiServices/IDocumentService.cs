using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;
using Web.Blazor.Models.Enums;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface IDocumentService
    {
        [Get("/Document/GetAll")]
        Task<List<DocumentModel>> GetAllAsync();

        [Get("/Document/GetById/{id}")]
        Task<DocumentModel?> GetByIdAsync(Guid id);

        [Get("/Document/GetByDocumentType")]
        Task<List<DocumentModel>> GetByDocumentType(DocumentTypes documentType);

        [Delete("/Document/Delete/{id}")]
        Task<ApiResponse<string>> DeleteDocumentAsync(Guid id);

        [Multipart]
        [Post("/Document/Create")]
        Task<ApiResponse<string>> CreateDocumentAsync(MultipartFormDataContent form);

        [Multipart]
        [Put("/Document/Update/{id}")]
        Task<ApiResponse<string>> UpdateDocumentAsync(Guid id, MultipartFormDataContent form);
    }
}
