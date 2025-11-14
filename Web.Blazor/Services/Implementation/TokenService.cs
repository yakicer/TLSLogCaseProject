using Web.Blazor.Services.Interface;

namespace Web.Blazor.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly LocalStorageService _localStorageService;
        private const string TokenKey = "Auth-Token";

        public TokenService(LocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetAccessTokenAsync(string token)
        {
            await _localStorageService.SetItemAsync(TokenKey, token);
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await _localStorageService.GetItemAsync(TokenKey);
        }

        public async Task RemoveAccessTokenAsync()
        {
            await _localStorageService.RemoveItemAsync(TokenKey);
        }
    }
}
