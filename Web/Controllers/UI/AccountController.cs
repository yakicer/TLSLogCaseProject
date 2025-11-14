using Businness.Interface.Base;
using Entities.DTO;
using Entities.Entity;
using Entities.RequestModel;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.UI
{
    public class AccountController : BaseController
    {
        private readonly IApiClientService _apiClientService;
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IApiClientService apiClientService, ITokenService tokenService, IHttpClientFactory httpClientFactory, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _apiClientService = apiClientService;
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var response = await _apiClientService.PostAsync<BaseResponse<string>, UserLoginModel>("api/auth/login", model);
            // usermanager ve signinmanagerin ihtiyac duydugu bilgileri response icerisinde donmeyi dene
            if (response != null && !string.IsNullOrEmpty(response.Data))
            {
                _tokenService.SetToken(response.Data);
                //var user = await _userManager.FindByEmailAsync(model.Email);
                //if (user == null)
                //{
                //    ModelState.AddModelError("Error", response?.Response ?? "Kullanıcı bulunamadı. Lütfen bilgilerinizi kontrol ederek tekrar deneyiniz.");
                //    return View();
                //}

                //var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                //if (!result.Succeeded)
                //{
                //    ModelState.AddModelError("Error", response?.Response ?? "Giriş Başarısız. Lütfen bilgilerinizi kontrol ederek tekrar deneyiniz.");
                //    return View();
                //}
                return LocalRedirect("/Dashboard");
            }
            ModelState.AddModelError("Error", response?.Response ?? "Giriş Başarısız.");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var response = await _apiClientService.GetAsync<BaseResponse<List<Project>>>("api/project/getall");
            if (response != null && response.Data != null)
            {
                return Ok(response.Data);
            }
            return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> GetAdminTest()
        {
            var queryParams = new { isCompleted = true }
;

            var response = await _apiClientService.GetAsync<BaseResponse<ProjectDTO>>("api/project/GetByCompletedStatus", queryParams);
            if (response != null && response.Data != null)
            {
                return Ok(response.Data);
            }
            return NotFound();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _tokenService.ClearToken();

            return LocalRedirect("/Dashboard");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            var resp = GetCurrentUser();
            var roles = resp.Data.Roles;
            ViewBag.Roles = roles;
            return View();
        }
    }
}
