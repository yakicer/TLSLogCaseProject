using Contracts.Common;
using Contracts.DTO.Dashboard;
using System.Text.Json;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Services.Api.Implementation
{
    public class DashboardApiService : IDashboardApiService
    {
        private readonly HttpClient _http;

        public DashboardApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<BaseResponse<DashboardResponse>> GetAsync()
        {
            var resp = await _http.GetAsync("api/dashboard");
            if (!resp.IsSuccessStatusCode)
            {
                return new BaseResponse<DashboardResponse>
                {
                    Success = false,
                    Response = "Dashboard verileri alınamadı."
                };
            }

            var obj = await resp.Content.ReadFromJsonAsync<BaseResponse<DashboardResponse>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return obj ?? new BaseResponse<DashboardResponse> { Success = false, Response = "Beklenmeyen cevap." };
        }
    }
}
