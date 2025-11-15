using Contracts.Common;
using Contracts.DTO.CustomerAddresses;
using Contracts.DTO.Customers;
using Contracts.DTO.Orders;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Services.Api.Implementation
{
    public class CustomerApiService : ICustomerApiService
    {
        private readonly HttpClient _http;
        private const string _apiEndpoint = "api/customer";
        public CustomerApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<BaseResponse<List<CustomerDto>>> GetAllAsync(bool onlyActives = true)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}?onlyActives={onlyActives.ToString().ToLower()}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerDto>>>()
                   ?? new BaseResponse<List<CustomerDto>> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<CustomerDetailDto>> GetByIdAsync(int id)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/{id}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<CustomerDetailDto>>()
                   ?? new BaseResponse<CustomerDetailDto> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<CustomerDetailDto>> CreateAsync(CustomerCreateRequestDto dto)
        {
            var resp = await _http.PostAsJsonAsync($"{_apiEndpoint}", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<CustomerDetailDto>>()
                   ?? new BaseResponse<CustomerDetailDto> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<bool>> UpdateAsync(CustomerUpdateRequestDto dto)
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

        public async Task<BaseResponse<CustomerAddressDto>> AddAddressAsync(CustomerAddressCreateRequestDto dto)
        {
            var resp = await _http.PostAsJsonAsync($"{_apiEndpoint}/{dto.CustomerId}/add-address", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<CustomerAddressDto>>()
                   ?? new BaseResponse<CustomerAddressDto> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<List<CustomerAddressDto>>> GetAddressesAsync(int customerId)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/{customerId}/getaddresses");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerAddressDto>>>()
                   ?? new BaseResponse<List<CustomerAddressDto>> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<List<CustomerDetailDto>>> GetCustomersWithDifferentAddressesAsync()
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/different-addresses");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerDetailDto>>>()
                   ?? new BaseResponse<List<CustomerDetailDto>> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<List<CustomerOrderDto>>> GetOrdersByCustomerNameAsync(string customerName)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/orders-by-name?name={customerName.ToString().ToLower()}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerOrderDto>>>()
                   ?? new BaseResponse<List<CustomerOrderDto>> { Success = false, Response = "Bağlantı hatası" };
        }
    }
}
