using Contracts.Common;
using Contracts.DTO.Customers;
using Contracts.DTO.Orders;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Services.Api.Implementation
{
    public class OrderApiService : IOrderApiService
    {
        private readonly HttpClient _http;
        private const string _apiEndpoint = "api/order";
        public OrderApiService(HttpClient http)
        {
            _http = http;
        }
        public async Task<BaseResponse<List<OrderDto>>> GetAllAsync(bool onlyActives = true)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}?onlyActives={onlyActives.ToString().ToLower()}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<OrderDto>>>()
                   ?? new BaseResponse<List<OrderDto>> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<OrderDetailDto>> GetByIdAsync(int id)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/{id}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<OrderDetailDto>>()
                   ?? new BaseResponse<OrderDetailDto> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<int>> CreateAsync(OrderCreateRequestDto dto)
        {
            var resp = await _http.PostAsJsonAsync($"{_apiEndpoint}", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<int>>()
                   ?? new BaseResponse<int> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<int>> UpdateAsync(OrderUpdateRequestDto dto)
        {
            var resp = await _http.PutAsJsonAsync($"{_apiEndpoint}/{dto.Id}", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<int>>()
                   ?? new BaseResponse<int> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<int>> UpdateStatusAsync(OrderStatusUpdateRequestDto dto)
        {
            var resp = await _http.PatchAsJsonAsync($"{_apiEndpoint}/{dto.Id}/status", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<int>>()
                   ?? new BaseResponse<int> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            var resp = await _http.DeleteAsync($"api/order/{id}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<bool>>()
                   ?? new BaseResponse<bool> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<List<CustomerOrderDto>>> GetOrdersWithMinQuantityAsync(decimal minAmount)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/by-min-quantity?minAmount={minAmount}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerOrderDto>>>()
                   ?? new BaseResponse<List<CustomerOrderDto>> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<CityOrderCountDto>> GetOrderCountByCityAsync(string city)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/count-by-city?city={city}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<CityOrderCountDto>>()
                   ?? new BaseResponse<CityOrderCountDto> { Success = false, Response = "Bağlantı hatası" };
        }
    }
}
