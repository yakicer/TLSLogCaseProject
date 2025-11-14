using Microsoft.AspNetCore.Components.Forms;
using Web.Blazor.Models.RequestModel;

namespace Web.Blazor.Services.Interface
{
    public interface IFileUploadService
    {
        public Task<UploadFileModel> UploadFile(IBrowserFile file);
        public Task<UploadFileModel> UploadDocument(IBrowserFile file);
        public Task<List<UploadFileModel>> UploadMultipleFiles(IReadOnlyList<IBrowserFile> files);
        public void DownloadFile(string filePath);
        public void DeleteFile(string filePath);

    }
}
