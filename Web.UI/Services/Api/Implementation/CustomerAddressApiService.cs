using Contracts.Common;
using Contracts.DTO.CustomerAddresses;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Services.Api.Implementation
{
    public class CustomerAddressApiService : ICustomerAddressApiService
    {
        private readonly HttpClient _http;
        private const string _apiEndpoint = "api/customeraddress";
        public CustomerAddressApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<BaseResponse<List<CustomerAddressDto>>> GetAllAsync(bool onlyActives)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/?onlyActives={onlyActives.ToString().ToLower()}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerAddressDto>>>()
                   ?? new BaseResponse<List<CustomerAddressDto>> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<List<CustomerAddressDetailDto>>> GetAllDetailedAsync(bool onlyActives)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/getalldetailed?onlyActives={onlyActives.ToString().ToLower()}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerAddressDetailDto>>>()
                   ?? new BaseResponse<List<CustomerAddressDetailDto>> { Success = false, Response = "Bağlantı hatası" };
        }
        public async Task<BaseResponse<CustomerAddressDetailDto>> CreateAsync(CustomerAddressCreateRequestDto dto)
        {
            var resp = await _http.PostAsJsonAsync($"{_apiEndpoint}", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<CustomerAddressDetailDto>>()
                   ?? new BaseResponse<CustomerAddressDetailDto> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(int id)
        {
            var resp = await _http.DeleteAsync($"{_apiEndpoint}/{id}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<bool>>()
                   ?? new BaseResponse<bool> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<CustomerAddressDetailDto>> GetAsync(int id)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/{id}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<CustomerAddressDetailDto>>()
                   ?? new BaseResponse<CustomerAddressDetailDto> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<List<CustomerAddressDto>>> GetByCustomerAsync(int customerId, bool onlyActives)
        {
            var resp = await _http.GetAsync($"{_apiEndpoint}/customer/{customerId}?onlyActives={onlyActives.ToString().ToLower()}");
            return await resp.Content.ReadFromJsonAsync<BaseResponse<List<CustomerAddressDto>>>()
                   ?? new BaseResponse<List<CustomerAddressDto>> { Success = false, Response = "Bağlantı hatası" };
        }

        public async Task<BaseResponse<bool>> UpdateAsync(CustomerAddressUpdateRequestDto dto)
        {
            var resp = await _http.PutAsJsonAsync($"{_apiEndpoint}/{dto.Id}", dto);
            return await resp.Content.ReadFromJsonAsync<BaseResponse<bool>>()
                   ?? new BaseResponse<bool> { Success = false, Response = "Bağlantı hatası" };
        }
    }
}
