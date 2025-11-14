using Businness.Interface.Base;
using System.Net.Http.Headers;

namespace Web.Helpers
{
    public class JwtAuthorizationHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        private readonly List<string> _excludedRoutes = new List<string>
        {
            "/api/auth/login",
            "/api/auth/register"
        };
        public JwtAuthorizationHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenService.GetToken();
            //var requestPath = request.RequestUri?.AbsolutePath.ToLower();
            //if (_excludedRoutes.Any(route => requestPath!.Contains(route)))
            //{
            //    return await base.SendAsync(request, cancellationToken);
            //}
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
