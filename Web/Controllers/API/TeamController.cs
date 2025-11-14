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
    public class TeamController : BaseController
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await _teamService.GetAllAsync();
            return Ok(new BaseResponse<List<Employee>>() { Success = true, Data = employees.ToList() });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var employee = _teamService.GetById(id);
            if (employee == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID'ye ait herhangi bir çalışan bulunamadı. ID: {id}" });
            return Ok(new BaseResponse<Employee>() { Success = true, Data = employee });
        }

        [HttpGet]
        public IActionResult GetByDepartmentType(DepartmentTypes departmentType)
        {
            var employees = _teamService.GetByDepartment(departmentType).ToList();
            if (employees.Count == 0)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"{departmentType} departmanına ait herhangi bir çalışan bulunamadı." });
            return Ok(new BaseResponse<List<Employee>>() { Success = true, Data = employees });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeDTO model)
        {

            if (model.MainImage == null || model.MainImage.Length == 0)
            {
                return BadRequest(new BaseResponse<string>() { Success = false, Data = $"Ana Görsel Zorunludur." });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var employee = new Employee
                    {
                        Name = model.Name,
                        SurName = model.SurName,
                        BirthDate = model.BirthDate,
                        Description = model.Description,
                        Title = model.Title,
                        DepartmentType = model.DepartmentType,
                    };
                    var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                    if (!resp.Success)
                    {
                        return StatusCode(500, resp);

                    }
                    employee.MainImagePath = resp.Data;

                    await _teamService.CreateAsync(employee);

                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Çalışan başarıyla eklendi. Employee_Id: {employee.Id}" });
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
        public async Task<IActionResult> Update(Guid id, [FromForm] EmployeeDTO model)
        {
            var employee = _teamService.GetById(id);
            if (employee == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile çalışan bulunamadı. ID: {id}" });
            }
            try
            {

                if (ModelState.IsValid)
                {
                    employee.Name = model.Name;
                    employee.SurName = model.SurName;
                    employee.Title = model.Title;
                    employee.Description = model.Description;
                    employee.BirthDate = model.BirthDate;
                    employee.DepartmentType = model.DepartmentType;

                    if (model.MainImage != null)
                    {
                        if (!string.IsNullOrEmpty(employee.MainImagePath))
                        {
                            DeleteFile(employee.MainImagePath);
                        }

                        var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);

                        }
                        employee.MainImagePath = resp.Data;
                    }

                    await _teamService.Update(employee);
                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Çalışan bilgileri başarıyla güncellendi. Employee_Id: {employee.Id}" });
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
            var employee = _teamService.GetById(id);
            if (employee == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile çalışan bulunamadı. ID: {id}" });
            }
            try
            {
                if (!string.IsNullOrEmpty(employee.MainImagePath))
                {
                    DeleteFile(employee.MainImagePath);
                }
                await _teamService.DeleteAsync(id);
                return Ok(new BaseResponse<string>() { Success = true, Data = $"Çalışan bilgileri ve çalışana ait dosyalar başarıyla silindi." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });

            }
        }
    }
}
