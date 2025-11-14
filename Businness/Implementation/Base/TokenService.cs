using Businness.Interface.Base;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Businness.Implementation.Base
{

    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return string.Empty;
            string token;
            try
            {
                token = context.Session.GetString("AuthToken")!;
                if (!string.IsNullOrEmpty(token))
                    return token;
            }
            catch (Exception)
            {
                token = context.Request.Cookies["AuthToken"]!;
                if (!string.IsNullOrEmpty(token))
                    return token;
            }

            token = context.User.Claims.FirstOrDefault(c => c.Type == "AuthToken")?.Value!;
            return token ?? string.Empty;
        }

        public void SetToken(string token)
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;

            try
            {
                context.Session.SetString("AuthToken", token);
            }
            catch (Exception)
            {

            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(8)
            };
            context.Response.Cookies.Append("AuthToken", token, cookieOptions);
        }

        public void ClearToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return;

            try
            {
                context.Session.Remove("AuthToken");
            }
            catch (Exception)
            {

            }

            context.Response.Cookies.Delete("AuthToken");
        }
    }
}
