using Businness.Interface.API;
using Entities.DTO;
using Entities.Entity;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.API
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class SystemSettingsController : BaseController
    {
        private readonly ISystemManagementService _systemManagementService;

        public SystemSettingsController(ISystemManagementService systemManagementService)
        {
            _systemManagementService = systemManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var systemSettings = await _systemManagementService.GetAllAsync();
            return Ok(new BaseResponse<List<SystemSettings>>() { Success = true, Data = systemSettings.ToList() });
        }
        [HttpGet]
        public IActionResult GetDefaultSettings()
        {
            var systemSettings = _systemManagementService.GetDefaultSettings();
            if (systemSettings == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"Sistem ayarları bulunamadı. Lütfen kontrol edip tekrar deneyiniz." });
            return Ok(new BaseResponse<SystemSettings>() { Success = true, Data = systemSettings });
        }
        [HttpGet]
        public IActionResult GetContactInfo()
        {
            var systemSettings = _systemManagementService.GetDefaultSettings();
            if (systemSettings == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"Herhangi bir sistem ayarı bulunamadı. Lütfen sistem ayarlarınızı kontrol ediniz." });
            ContactDTO ContactInfo = new ContactDTO()
            {
                ContactAddress = systemSettings.ContactAddress,
                ContactEmail = systemSettings.ContactEmail,
                ContactPhone1 = systemSettings.ContactPhone1,
                ContactPhone2 = systemSettings.ContactPhone2,
                MapHtmlCode = systemSettings.MapHtmlCode,
            };
            return Ok(new BaseResponse<ContactDTO>() { Success = true, Data = ContactInfo });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var systemSettings = _systemManagementService.GetById(id);
            if (systemSettings == null)
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"Sistem ayarları bulunamadı. Lütfen kontrol edip tekrar deneyiniz." });
            return Ok(new BaseResponse<SystemSettings>() { Success = true, Data = systemSettings });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] SystemSettingsDTO model)
        {
            var systemSettings = _systemManagementService.GetById(id);
            if (systemSettings == null)
            {
                return NotFound(new BaseResponse<string>() { Success = false, Data = $"Sistem ayarları bulunamadı. Lütfen kontrol edip tekrar deneyiniz." });
            }
            try
            {
                if (ModelState.IsValid)
                {
                    systemSettings.LinkedInLink = model.LinkedInLink;
                    systemSettings.TwitterLink = model.TwitterLink;
                    systemSettings.InstagramLink = model.InstagramLink;
                    systemSettings.MailUrl = model.MailUrl;
                    systemSettings.ContactAddress = model.ContactAddress;
                    systemSettings.ContactEmail = model.ContactEmail;
                    systemSettings.ContactPhone1 = model.ContactPhone1;
                    systemSettings.ContactPhone2 = model.ContactPhone2;
                    systemSettings.AboutSection = model.AboutSection;
                    systemSettings.MapHtmlCode = model.MapHtmlCode;
                    systemSettings.LastUpdate = DateTime.Now;

                    await _systemManagementService.Update(systemSettings);
                    return Ok(new BaseResponse<string>() { Success = true, Data = $"Sistem ayarları başarıyla güncellendi." });
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
    }
}
