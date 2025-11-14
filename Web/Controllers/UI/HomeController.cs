using Entities.DTO;
using Entities.RequestModel;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers.UI
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _baseUrl = _configuration["AppSettings:BaseApiUrl"]!;
        }

        public async Task<IActionResult> Index()
        {
            var user = HttpContext.User;
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
                return Ok(claims);
            }

            var token = Request.Cookies["AuthToken"];
            ViewBag.UserInfo = null;
            if (!string.IsNullOrEmpty(token))
            {
                ViewBag.Token = token;
                string apiUrl = "https://localhost:7182/api/auth/GetCurrentUserInfo";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var contentData = JsonConvert.DeserializeObject<BaseResponse<CurrentUserDTO>>(content);
                    ViewBag.UserInfo = contentData!.Data;
                }
            }

            return View();
        }

        public async Task<IActionResult> Privacy()
        {
            var loginInfo = new UserLoginModel
            {
                Email = "yakupicer@gmail.com",
                Password = "123456",
                ReturnUrl = ""
            };

            string apiUrl = $"{_baseUrl}/api/auth/login";

            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginInfo), System.Text.Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(apiUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contentData = JsonConvert.DeserializeObject<BaseResponse<string>>(content);
                ViewBag.IsLoginSuccess = true;
                ViewBag.Content = content;
                var token = contentData!.Data;
                ViewBag.Token = token;
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                ViewBag.IsLoginSuccess = true;
                ViewBag.Content = null;
            }

            return View();
        }

        public IActionResult Test()
        {
            var token = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token Header'a eklenmedi!");
            }

            var user = User.Identity;
            if (user == null || !user.IsAuthenticated)
            {
                return Unauthorized("Kullanýcý Authenticate deðil!");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            var user = HttpContext.User;
            var token = HttpContext.Session.GetString("AuthToken");
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
                var claim = new { Type = "token", Value = token };
                claims.Add(claim!);
                return Ok(claims);
            }

            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult TestUser()
        {
            var user = HttpContext.User;
            var token = HttpContext.Session.GetString("AuthToken");
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
                var claim = new { Type = "token", Value = token };
                claims.Add(claim!);
                return Ok(claims);
            }

            return View();
        }

        [Authorize(Roles = "Moderator")]
        public IActionResult TestModerator()
        {
            var user = HttpContext.User;
            var token = HttpContext.Session.GetString("AuthToken");
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
                var claim = new { Type = "token", Value = token };
                claims.Add(claim!);
                return Ok(claims);
            }

            return View();
        }


        //public async Task<IActionResult> Logout()
        //{

        //    string apiUrl = "https://localhost:7182/api/auth/login";

        //    var jsonContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(loginInfo), System.Text.Encoding.UTF8, "application/json");
        //    var client = _httpClientFactory.CreateClient();
        //    var response = await client.PostAsync(apiUrl, jsonContent);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();
        //        var contentData = JsonConvert.DeserializeObject<BaseResponse<string>>(content);
        //        ViewBag.IsLoginSuccess = true;
        //        ViewBag.Content = content;
        //        var token = contentData!.Data;
        //        ViewBag.Token = token;
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        //    }
        //    else
        //    {
        //        ViewBag.IsLoginSuccess = true;
        //        ViewBag.Content = null;
        //    }

        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
