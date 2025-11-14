using Contracts.Common;
using Contracts.DTO.Customers;
using Contracts.DTO.Stocks;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Services.Api.Implementation
{
    public class StockApiService : IStockApiService
    {
        private readonly HttpClient _http;
        private const string _apiEndpoint = "api/stock";
        public StockApiService(HttpClient http)
        {
            _http = http;
        }
        public async Task<BaseResponse<List<StockDto>>> GetAllAsync(bool onlyActives = true)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}?onlyActives={onlyActives.ToString().ToLower()}");
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"API çağrısı başarısız oldu. Durum kodu: {resp.StatusCode}");
            }
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<StockDto>>>()
                   ?? new BaseResponse<List<StockDto>> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<StockDetailDto>> GetByIdAsync(int id)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/{id}");
            if (!resp.IsSuccessStatusCode)
            {
                throw new Exception($"API çağrısı başarısız oldu. Durum kodu: {resp.StatusCode}");
            }
            return await resp.Content.ReadFromJsonAsync<BaseResponse<StockDetailDto>>()
                   ?? new BaseResponse<StockDetailDto> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<List<CustomerDetailDto>>> GetCustomersByStock(int stockId)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/{stockId}/customers");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerDetailDto>>>()
                   ?? new BaseResponse<List<CustomerDetailDto>> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<StockDetailDto>> CreateAsync(StockCreateDto dto)
        {
            var resp = await _http.PostAsJsonAsync($"{_apiEndpoint}", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<StockDetailDto>>()
                   ?? new BaseResponse<StockDetailDto> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<bool>> UpdateAsync(StockUpdateDto dto)
        {
            var resp = await _http.PutAsJsonAsync($"{_apiEndpoint}/{dto.Id}", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<bool>>()
                   ?? new BaseResponse<bool> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            var resp = await _http.DeleteAsync($"{_apiEndpoint}/{id}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<bool>>()
                   ?? new BaseResponse<bool> { Success = false, Response = "Bağlantı hatası" };
        }

    }
}
