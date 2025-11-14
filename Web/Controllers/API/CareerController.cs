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
    public class CareerController : BaseController
    {
        private readonly ICareerService _careerService;

        public CareerController(ICareerService careerService)
        {
            _careerService = careerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var careers = await _careerService.GetAllAsync();
            return Ok(new BaseResponse<List<Career>>() { Success = true, Data = careers.ToList() });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var career = _careerService.GetById(id);
            if (career == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID'ye ait herhangi bir başvuru bulunamadı. ID: {id}" });
            return Ok(new BaseResponse<Career>() { Success = true, Data = career });
        }

        [HttpGet]
        public IActionResult GetByDepartmentType(DepartmentTypes departmentType)
        {
            var careers = _careerService.GetByDepartment(departmentType).ToList();
            if (careers.Count == 0)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"{departmentType} departmanına ait herhangi bir proje bulunamadı." });
            return Ok(new BaseResponse<List<Career>>() { Success = true, Data = careers });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CareerDTO model)
        {

            if (model.CVFile == null || model.CVFile.Length == 0)
            {
                return BadRequest(new BaseResponse<string>() { Success = false, Data = $"CV Yüklenmesi Zorunludur." });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var career = new Career
                    {
                        Name = model.Name,
                        SurName = model.SurName,
                        Email = model.Email,
                        Phone = model.Phone,
                        Department = model.Department,
                        Description = model.Description,
                    };
                    var resp = await SaveFileAsync(model.CVFile, UploadRoutes.AdditionalFiles, DocumentTypes.Document);
                    if (!resp.Success)
                    {
                        return StatusCode(500, resp);

                    }
                    career.CVFilePath = resp.Data;

                    await _careerService.CreateAsync(career);

                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Başvuru başarıyla eklendi. Career_Id: {career.Id}" });
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
        public async Task<IActionResult> Update(Guid id, [FromForm] CareerDTO model)
        {
            var career = _careerService.GetById(id);
            if (career == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile başvuru bulunamadı. ID: {id}" });
            }
            try
            {

                if (ModelState.IsValid)
                {
                    career.Name = model.Name;
                    career.SurName = model.SurName;
                    career.Email = model.Email;
                    career.Phone = model.Phone;
                    career.Department = model.Department;
                    career.Description = model.Description;

                    if (model.CVFile != null)
                    {
                        if (!string.IsNullOrEmpty(career.CVFilePath))
                        {
                            DeleteFile(career.CVFilePath);
                        }

                        var resp = await SaveFileAsync(model.CVFile, UploadRoutes.AdditionalFiles, DocumentTypes.Document);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);

                        }
                        career.CVFilePath = resp.Data;
                    }

                    await _careerService.Update(career);
                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Başvuru başarıyla güncellendi. Project_Id: {career.Id}" });
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
            var career = _careerService.GetById(id);
            if (career == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile başvuru bulunamadı. ID: {id}" });
            }
            try
            {
                if (!string.IsNullOrEmpty(career.CVFilePath))
                {
                    DeleteFile(career.CVFilePath);
                }
                await _careerService.DeleteAsync(id);
                return Ok(new BaseResponse<string>() { Success = true, Data = $"Başvuru ve başvuruya ait dosyalar başarıyla silindi." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });

            }
        }
    }
}
