using Microsoft.AspNetCore.Components.Forms;
using Web.Blazor.Models.RequestModel;
using Web.Blazor.Services.Interface;

namespace Web.Blazor.Services.ApiServices
{
    public class FileUploadService : IFileUploadService
    {
        private List<string> imagefiletypes = new List<string>() {
                    ".png",".jpg",".jpeg",".webp"
                };
        private List<string> documentFileTypes = new List<string>() {
                    ".pdf"
                };
        private int maxAllowedFiles = 10;
        const int maxFileSize = 20 * 1024 * 1024; // 20 Mb
        public void DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void DownloadFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public async Task<UploadFileModel> UploadFile(IBrowserFile file)
        {
            var extensionname = Path.GetExtension(file.Name);
            if (imagefiletypes.Contains(extensionname.ToLower()))
            {
                var fileGuid = Guid.NewGuid();
                var resizedFile = await file.RequestImageFileAsync(file.ContentType, 640, 480); // resize the image file
                var buf = new byte[resizedFile.Size]; // allocate a buffer to fill with the file's data
                using (var stream = resizedFile.OpenReadStream(maxFileSize))
                {
                    await stream.ReadAsync(buf); // copy the stream to the buffer
                }
                string base64data = "data:image/png;base64," + Convert.ToBase64String(buf);
                using var fileStream = file.OpenReadStream(maxFileSize);
                using var ms = new MemoryStream();
                await fileStream.CopyToAsync(ms);

                FileContents loadedFile = new()
                {
                    File = file,
                    Base64Content = base64data,
                    FileId = fileGuid
                };

                UploadFileModel uploadedFile = new UploadFileModel
                {
                    FileName = file.Name,
                    FileContent = ms.ToArray(), // byte[] olarak sakla (stream yerine)
                    ContentType = file.ContentType,
                    FileId = fileGuid,
                    LoadedFile = loadedFile

                };

                return uploadedFile;
            }
            else
            {
                throw new Exception($"{file.Name} dosyası geçersiz dosya tipinde olduğu için yüklenemedi !");
            };
        }
        public async Task<UploadFileModel> UploadDocument(IBrowserFile file)
        {
            var extensionname = Path.GetExtension(file.Name);
            if (documentFileTypes.Contains(extensionname.ToLower()))
            {
                var fileGuid = Guid.NewGuid();
                var buf = new byte[file.Size];
                using (var stream = file.OpenReadStream(maxFileSize))
                {
                    await stream.ReadAsync(buf);
                }
                using var fileStream = file.OpenReadStream(maxFileSize);
                using var ms = new MemoryStream();
                await fileStream.CopyToAsync(ms);

                FileContents loadedFile = new()
                {
                    File = file,
                    Base64Content = "",
                    FileId = fileGuid
                };

                UploadFileModel uploadedFile = new UploadFileModel
                {
                    FileName = file.Name,
                    FileContent = ms.ToArray(),
                    ContentType = file.ContentType,
                    FileId = fileGuid,
                    LoadedFile = loadedFile
                };

                return uploadedFile;
            }
            else
            {
                throw new Exception($"{file.Name} dosyası geçersiz dosya tipinde olduğu için yüklenemedi !");
            };
        }

        public async Task<List<UploadFileModel>> UploadMultipleFiles(IReadOnlyList<IBrowserFile> files)
        {
            List<UploadFileModel> model = new List<UploadFileModel>();

            foreach (var file in files)
            {
                var extensionname = Path.GetExtension(file.Name);

                if (imagefiletypes.Contains(extensionname.ToLower()))
                {
                    var fileGuid = Guid.NewGuid();
                    var resizedFile = await file.RequestImageFileAsync(file.ContentType, 640, 480); // resize the image file
                    var buf = new byte[resizedFile.Size]; // allocate a buffer to fill with the file's data
                    using (var stream = resizedFile.OpenReadStream(maxFileSize))
                    {
                        await stream.ReadAsync(buf); // copy the stream to the buffer
                    }
                    string base64data = "data:image/png;base64," + Convert.ToBase64String(buf);
                    using var fileStream = file.OpenReadStream(maxFileSize);
                    using var ms = new MemoryStream();
                    await fileStream.CopyToAsync(ms);


                    FileContents loadedFile = new()
                    {
                        File = file,
                        Base64Content = base64data,
                        FileId = fileGuid
                    };

                    model.Add(new UploadFileModel
                    {
                        FileName = file.Name,
                        FileContent = ms.ToArray(), // byte[] olarak sakla (stream yerine)
                        ContentType = file.ContentType,
                        FileId = fileGuid,
                        LoadedFile = loadedFile
                    });
                }
            }
            return model;
        }
    }
}
