using Contracts.DTO.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IDashboardApiService _api;

        public HomeController(IDashboardApiService api)
        {
            _api = api;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var resp = await _api.GetAsync();
            if (!resp.Success || resp.Data == null)
            {
                TempData["Error"] = resp.Response ?? "Dashboard yüklenemedi.";
                return View(new DashboardResponse());
            }
            return View(resp.Data);
        }
    }
}

