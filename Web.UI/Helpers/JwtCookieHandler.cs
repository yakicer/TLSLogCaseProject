using System.Net.Http.Headers;

namespace Web.UI.Helpers
{
    public class JwtCookieHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _ctx;
        private readonly IConfiguration _cfg;
        public JwtCookieHandler(IHttpContextAccessor ctx, IConfiguration cfg)
        {
            _ctx = ctx;
            _cfg = cfg;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var http = _ctx.HttpContext;
            var cookieName = _cfg["Auth:CookieName"] ?? "AuthToken";
            var token = http?.Request.Cookies[cookieName];
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
