using Businness.Interface.API;
using Entities.DTO;
using Entities.Entity;
using Entities.Enums;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.API
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class DocumentController : BaseController
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var documents = await _documentService.GetAllAsync();
            return Ok(new BaseResponse<List<Document>>() { Success = true, Data = documents.ToList() });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var document = _documentService.GetById(id);
            if (document == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID'ye ait herhangi bir dosya bulunamadı. ID: {id}" });
            return Ok(new BaseResponse<Document>() { Success = true, Data = document });
        }

        [HttpGet]
        public IActionResult GetByDocumentType(DocumentTypes documentType)
        {
            var documents = _documentService.GetByDocumentType(documentType).ToList();
            if (documents.Count == 0)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"{documentType} tipine ait herhangi bir dosya bulunamadı." });
            return Ok(new BaseResponse<List<Document>>() { Success = true, Data = documents });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DocumentDTO model)
        {
            if (model.Document == null || model.Document.Length == 0)
            {
                return BadRequest(new BaseResponse<string>() { Success = false, Data = $"Döküman yüklenmesi zorunludur." });
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var document = new Document
                    {
                        DocumentTitle = model.DocumentTitle,
                        DocumentType = model.DocumentType,
                        DocumentDescription = model.DocumentDescription,

                    };

                    if (model.MainImage != null && model.MainImage.Length != 0)
                    {
                        var imageResp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                        if (!imageResp.Success)
                        {
                            return StatusCode(500, imageResp);
                        }
                        document.MainImagePath = imageResp.Data;
                    }

                    var resp = await SaveFileAsync(model.Document, UploadRoutes.Documents, DocumentTypes.Document);
                    if (!resp.Success)
                    {
                        return StatusCode(500, resp);
                    }
                    document.DocumentUrl = resp.Data;

                    await _documentService.CreateAsync(document);

                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Döküman başarıyla eklendi. Document_Id: {document.Id}" });
                }
                else
                {
                    return BadRequest(new BaseResponse<string>() { Success = false, Data = $"Model Hatası: {ModelState}" });
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] DocumentDTO model)
        {
            var document = _documentService.GetById(id);
            if (document == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile döküman bulunamadı. ID: {id}" });
            }
            try
            {

                if (ModelState.IsValid)
                {
                    document.DocumentTitle = model.DocumentTitle;
                    document.DocumentDescription = model.DocumentDescription;
                    document.DocumentType = model.DocumentType;

                    if (model.MainImage != null)
                    {
                        if (!string.IsNullOrEmpty(document.MainImagePath))
                        {
                            DeleteFile(document.MainImagePath);
                        }

                        var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);

                        }
                        document.MainImagePath = resp.Data;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(document.MainImagePath))
                        {
                            DeleteFile(document.MainImagePath);
                            document.MainImagePath = "";
                        }
                    }

                    if (model.Document != null)
                    {
                        if (!string.IsNullOrEmpty(document.DocumentUrl))
                        {
                            DeleteFile(document.DocumentUrl);
                        }

                        var resp = await SaveFileAsync(model.Document, UploadRoutes.Documents, DocumentTypes.Document);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);

                        }
                        document.DocumentUrl = resp.Data;
                    }

                    await _documentService.Update(document);
                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Döküman başarıyla güncellendi. Döküman_Id: {document.Id}" });
                }
                else
                {
                    return BadRequest(new BaseResponse<string>() { Success = true, Data = $"Model Hatası: {ModelState}" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });
            }

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var document = _documentService.GetById(id);
            if (document == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile döküman bulunamadı. ID: {id}" });
            }
            try
            {
                if (!string.IsNullOrEmpty(document.MainImagePath))
                {
                    DeleteFile(document.MainImagePath);
                }
                if (!string.IsNullOrEmpty(document.DocumentUrl))
                {
                    DeleteFile(document.DocumentUrl);
                }
                await _documentService.DeleteAsync(id);
                return Ok(new BaseResponse<string>() { Success = true, Data = $"Döküman başarıyla silindi." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });

            }
        }
    }
}
