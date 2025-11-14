using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Web.Blazor.Services.Interface;

namespace Web.Blazor.Services.Implementation
{
    /// <summary>
    /// version of Claim controls with jwt
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenService _tokenService;
        private readonly IToastService _toastService;

        public CustomAuthStateProvider(ITokenService tokenService, IToastService toastService)
        {
            _tokenService = tokenService;
            _toastService = toastService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenService.GetAccessTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var jwt = handler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                //var claimName = identity.Claims.FirstOrDefault(c => c.Type == "Name");
                //var claimSurName = identity.Claims.FirstOrDefault(c => c.Type == "SurName")?.Value;
                //var claimEmail = identity.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
                //identity.AddClaim(claimName);
                var expiry = identity.Claims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
                if (expiry == null)
                {
                    _toastService.ShowWarning("Oturum zaman aşımına uğradı. Lütfen tekrar giriş yapınız.");
                    return await LogUserOutForce();
                }
                var datetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
                if (datetime.UtcDateTime <= DateTime.UtcNow)
                {
                    _toastService.ShowWarning("Oturum zaman aşımına uğradı. Lütfen tekrar giriş yapınız.");
                    return await LogUserOutForce();
                }

                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception)
            {
                _toastService.ShowError("Token manipülasyonu tespit edildi. Lütfen tekrar giriş yapınız.");
                return await LogUserOutForce();
            }
        }

        public void NotifyUserAuthentication(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var jwt = handler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(jwt.Claims, "jwt");
                var user = new ClaimsPrincipal(identity);
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            }
            catch (Exception)
            {
                _toastService.ShowError("Geçersiz token tespit edildi. Lütfen tekrar giriş yapınız. Sorun çözülmezse lütfen sistem yöneticinizle iletişime geçiniz.");
                NotifyUserLogout();
            }
        }

        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }

        public async Task<AuthenticationState> LogUserOutForce()
        {
            await _tokenService.RemoveAccessTokenAsync();
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
            var state = Task.FromResult(new AuthenticationState(anonymous));
            NotifyAuthenticationStateChanged(state);
            return await state;
        }
    }
}
