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
    public class HomeController : BaseController
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var home = await _homeService.GetAllAsync();
            return Ok(new BaseResponse<List<Home>>() { Success = true, Data = home.ToList() });
        }
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var home = _homeService.GetById(id);
            if (home == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID'ye ait herhangi bir içerik bulunamadı. ID: {id}" });
            return Ok(new BaseResponse<Home>() { Success = true, Data = home });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] HomeDTO model)
        {

            if (model.MainImage == null || model.MainImage.Length == 0)
            {
                return BadRequest(new BaseResponse<string>() { Success = false, Data = $"Ana görsel zorunludur." });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var home = new Home
                    {
                        Title = model.Title,
                        SubTitle = model.SubTitle,
                        Description = model.Description,
                        Order = model.Order,
                    };
                    var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                    if (!resp.Success)
                    {
                        return StatusCode(500, resp);

                    }
                    home.MainImagePath = resp.Data;

                    await _homeService.CreateAsync(home);

                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Anasayfa içeriği başarıyla eklendi. Home_Id: {home.Id}" });
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
        public async Task<IActionResult> Update(Guid id, [FromForm] HomeDTO model)
        {
            var home = _homeService.GetById(id);
            if (home == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile içerik bulunamadı. ID: {id}" });
            }
            try
            {

                if (ModelState.IsValid)
                {
                    home.Title = model.Title;
                    home.SubTitle = model.SubTitle;
                    home.Description = model.Description;
                    home.Order = model.Order;


                    if (model.MainImage != null)
                    {
                        if (!string.IsNullOrEmpty(home.MainImagePath))
                        {
                            DeleteFile(home.MainImagePath);
                        }

                        var resp = await SaveFileAsync(model.MainImage, UploadRoutes.Images, DocumentTypes.Image);
                        if (!resp.Success)
                        {
                            return StatusCode(500, resp);

                        }
                        home.MainImagePath = resp.Data;
                    }

                    await _homeService.Update(home);
                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Başvuru başarıyla güncellendi. Home_Id: {home.Id}" });
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
            var home = _homeService.GetById(id);
            if (home == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"İlgili ID ile başvuru bulunamadı. ID: {id}" });
            }
            try
            {
                if (!string.IsNullOrEmpty(home.MainImagePath))
                {
                    DeleteFile(home.MainImagePath);
                }
                await _homeService.DeleteAsync(id);
                return Ok(new BaseResponse<string>() { Success = true, Data = $"Başvuru ve başvuruya ait dosyalar başarıyla silindi." });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>() { Success = false, Data = $"Sistemsel bir hata oluştu: {ex.Message}" });

            }
        }
    }
}
