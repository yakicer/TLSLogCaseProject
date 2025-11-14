using Contracts.Common;
using Contracts.DTO.Auth;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Services.Api.Implementation
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _http;
        private const string _apiEndpoint = "api/auth";
        public AuthApiService(IHttpClientFactory f)
        {
            _http = f.CreateClient("ApiClient");
        }

        public async Task<BaseResponse<string>> LoginAsync(LoginRequestDto dto)
        {
            var resp = await _http.PostAsJsonAsync($"{_apiEndpoint}/Login", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<string>>()
                   ?? new BaseResponse<string> { Success = false, Response = "İstek başarısız." };
        }

        public async Task<BaseResponse<string>> LogoutAsync()
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/Logout");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<string>>()
                   ?? new BaseResponse<string> { Success = false, Response = "İstek başarısız." };
        }

        public Task<BaseResponse<UserProfileDto>> MeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
