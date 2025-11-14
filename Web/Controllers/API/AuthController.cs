using Businness.Interface.API;
using Entities.DTO;
using Entities.RequestModel;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.API
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(new BaseResponse<IEnumerable<IdentityError>>() { Success = false, Data = result.Errors });
            }
            return Ok(new BaseResponse<string>() { Success = true, Response = "Kayıt işlemi başarıyla gerçekleştirildi." });

        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginModel model)
        {
            var token = await _authService.LoginAsync(model);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new BaseResponse<string>() { Success = false, Response = "Giriş başarısız oldu. Lütfen Bilgilerinizi Kontrol Edip Tekrar Deneyiniz." });
            }

            return Ok(new BaseResponse<string>() { Success = true, Response = "Giriş Başarılı", Data = token });

        }
        [HttpPost]
        public IActionResult Logout()
        {
            _authService.LogoutAsync();


            return Ok(new BaseResponse<string>() { Success = true, Response = "Çıkış Başarılı" });

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SetUserRole(string email, string role)
        {
            var resp = await _authService.AssignRoleAsync(email, role);
            if (!resp.Succeeded)
            {
                return BadRequest(resp.Errors);
            }
            return Ok($"Kullanıcı rolü başarıyla atandı. {email} kullanıcısı {role} rolüne atandı.");
        }
        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> Logout()
        //{
        //    var resp = await _authService.AssignRoleAsync(email, role);
        //    if (!resp.Succeeded)
        //    {
        //        return BadRequest(resp.Errors);
        //    }
        //    return Ok($"Kullanıcı rolü başarıyla atandı. {email} kullanıcısı {role} rolüne atandı.");
        //}
        [HttpGet]
        public IActionResult GetCurrentUserInfo()
        {
            var resp = GetCurrentUser();
            if (!resp.Success)
                return NotFound(new BaseResponse<string>() { Success = false, Response = resp.Response });

            return Ok(new BaseResponse<CurrentUserDTO>() { Success = true, Data = resp.Data });

        }

    }
}
