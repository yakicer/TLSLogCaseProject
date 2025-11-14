using Contracts.DTO.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.UI.Helpers;
using Web.UI.Models.ViewModels;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiService _auth;
        private readonly IConfiguration _cfg;

        public AccountController(IAuthApiService auth, IConfiguration cfg)
        {
            _auth = auth; _cfg = cfg;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);

            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [AllowAnonymous]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(vm);

            var resp = await _auth.LoginAsync(new LoginRequestDto { Email = vm.Email, Password = vm.Password });

            if (!resp.Success || string.IsNullOrEmpty(resp.Data))
            {
                ModelState.AddModelError(string.Empty, resp.Response ?? "Giriş başarısız.");
                return View(vm);
            }

            var jwt = resp.Data;
            var cookieName = _cfg["Auth:CookieName"] ?? "AuthToken";
            var hours = int.TryParse(_cfg["Auth:CookieHours"], out var h) ? h : 8;

            Response.Cookies.Append(cookieName, jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(hours)
            });

            var principal = JwtToClaimsHelper.CreatePrincipalFromJwt(jwt);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _auth.LogoutAsync();
            var cookieName = _cfg["Auth:CookieName"] ?? "AuthToken";
            Response.Cookies.Delete(cookieName);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }


}
