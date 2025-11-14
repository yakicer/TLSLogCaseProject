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
    public class SolutionController : BaseController
    {
        private readonly ISolutionService _solutionService;

        public SolutionController(ISolutionService solutionService)
        {
            _solutionService = solutionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var solutions = await _solutionService.GetAllAsync();
            return Ok(new BaseResponse<List<Solution>>() { Success = true, Data = solutions.ToList() });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var solution = _solutionService.GetById(id);
            if (solution == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID'ye ait herhangi bir data bulunamadı. ID: {id}" });
            return Ok(new BaseResponse<Solution>() { Success = true, Data = solution });
        }
        [HttpGet]
        public IActionResult GetBySolutionType(SolutionTypes solutionType)
        {
            var solutions = _solutionService.GetBySolutionType(solutionType).ToList();
            if (solutions.Count == 0)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"{solutionType} çözümüne ait herhangi bir data bulunamadı." });
            return Ok(new BaseResponse<List<Solution>>() { Success = true, Data = solutions });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SolutionDTO model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var solution = new Solution
                    {
                        Title = model.Title,
                        SubTitle = model.SubTitle,
                        SolutionType = model.SolutionType
                    };

                    if (model.MainImage != null && model.MainImage.Length != 0)
                    {
                        var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);

                        }
                        solution.ImagePath = resp.Data;
                    }

                    await _solutionService.CreateAsync(solution);

                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Çözüm başarıyla eklendi. Solution_Id: {solution.Id}" });
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
        public async Task<IActionResult> Update(Guid id, [FromForm] SolutionDTO model)
        {
            var solution = _solutionService.GetById(id);
            if (solution == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile data bulunamadı. ID: {id}" });
            }
            try
            {

                if (ModelState.IsValid)
                {
                    solution.Title = model.Title;
                    solution.SubTitle = model.SubTitle;
                    solution.SolutionType = model.SolutionType;

                    if (model.MainImage != null)
                    {
                        if (!string.IsNullOrEmpty(solution.ImagePath))
                        {
                            DeleteFile(solution.ImagePath);
                        }

                        var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);

                        }
                        solution.ImagePath = resp.Data;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(solution.ImagePath))
                        {
                            DeleteFile(solution.ImagePath);
                            solution.ImagePath = "";
                        }
                    }

                    await _solutionService.Update(solution);
                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Çözüm başarıyla güncellendi. Solution_Id: {solution.Id}" });
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
            var solution = _solutionService.GetById(id);
            if (solution == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile data bulunamadı. ID: {id}" });
            }
            try
            {
                if (!string.IsNullOrEmpty(solution.ImagePath))
                {
                    DeleteFile(solution.ImagePath);
                }
                await _solutionService.DeleteAsync(id);
                return Ok(new BaseResponse<string>() { Success = true, Data = $"Çözüm ve çözüme ait dosyalar başarıyla silindi." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });

            }
        }

    }
}
