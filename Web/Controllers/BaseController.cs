using Entities.DTO;
using Entities.Enums;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        protected async Task<BaseResponse<string>> SaveFileAsync(IFormFile file, UploadRoutes route, DocumentTypes fileType)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", route.ToString());

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            List<string> validTypes = SetValidTypeFormats(fileType);

            var isFileTypeValid = CheckValidTypes(file.FileName, validTypes);
            if (!isFileTypeValid)
            {
                return new BaseResponse<string>() { Success = false, Data = $"Dosya formatı yanlış. Lütfen yüklenen dosyaların dosya formatlarını kontrol ediniz." };

            }
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            var relateivePath = $"/uploads/{route.ToString()}/{uniqueFileName}";
            var isFileUploaded = CheckIfFileSaved(relateivePath);
            if (!isFileUploaded)
            {
                return new BaseResponse<string>() { Success = true, Data = relateivePath };

            }
            return new BaseResponse<string>() { Success = false, Data = $"Dosya yüklenirken bir hata oluştu. Lütfen tekrar deneyiniz." };
        }

        protected void DeleteFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.TrimStart('/'));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        protected bool CheckIfFileSaved(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                return true;
            }
            return false;
        }
        protected bool CheckValidTypes(string fileName, List<string> ValidTypes)
        {
            string extension = Path.GetExtension(fileName);

            if (ValidTypes.Count > 0)
            {
                foreach (string type in ValidTypes)
                {
                    if (extension.Equals(type, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        protected List<string> SetValidTypeFormats(DocumentTypes fileType)
        {
            List<string> validTypes = new List<string>();
            switch (fileType)
            {
                case DocumentTypes.Document:
                    validTypes = new List<string>()
                    {
                        ".pdf",".docx",".doc"
                    };
                    break;
                case DocumentTypes.Image:
                    validTypes = new List<string>()
                    {
                        ".jpg",".jpeg",".png",".webp",".avif"
                    };
                    break;
                default:
                    break;
            }
            return validTypes;
        }

        protected BaseResponse<CurrentUserDTO> GetCurrentUser()
        {
            var isAuthenticated = User.Identity!.IsAuthenticated;

            if (!isAuthenticated)
                return new BaseResponse<CurrentUserDTO> { Success = false, Response = "Kullanıcı bilgileri bulunamadı!" };

            var user = new CurrentUserDTO
            {
                UserName = User.FindFirst(ClaimTypes.Name)?.Value ?? "",
                Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
            };

            return new BaseResponse<CurrentUserDTO> { Success = true, Data = user };
        }


        //protected BaseResponse<FileContentResult> DownloadFile(string fileName)
        //{
        //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads", route.ToString());

        //    var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
        //    if (!System.IO.File.Exists(filePath))
        //    {
        //        return new BaseResponse<FileContentResult> { Success = false, Response = "Dosya Bulunamadı" };
        //    }

        //    var bytes = System.IO.File.ReadAllBytes(filePath);
        //    return new BaseResponse<FileContentResult> { Success = true, Data = File(bytes, "application/octet-stream", fileName) };
        //}

    }
}
