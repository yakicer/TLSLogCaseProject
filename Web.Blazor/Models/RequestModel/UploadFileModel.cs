using Microsoft.AspNetCore.Components.Forms;

namespace Web.Blazor.Models.RequestModel
{
    public class UploadFileModel
    {
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string ContentType { get; set; }
        public FileContents LoadedFile { get; set; }
        public Guid FileId { get; set; }
    }

    public class FileContents
    {
        public IBrowserFile File { get; set; }
        public string Base64Content { get; set; }
        public Guid FileId { get; set; }

    }
}
