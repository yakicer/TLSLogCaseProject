using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.UI
{
    public class DashboardController : BaseController
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("test-token")]
        public IActionResult TestToken()
        {
            var user = HttpContext.User;
            if (user.Identity?.IsAuthenticated ?? false)
            {
                var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
                return Ok(claims);
            }
            return Unauthorized("Kullanıcı doğrulanamadı!");
        }
    }
}
