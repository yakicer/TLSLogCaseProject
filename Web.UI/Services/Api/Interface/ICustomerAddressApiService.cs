using Contracts.Common;
using Contracts.DTO.CustomerAddresses;

namespace Web.UI.Services.Api.Interface
{
    public interface ICustomerAddressApiService
    {
        Task<BaseResponse<List<CustomerAddressDto>>> GetAllAsync(bool onlyActives);
        Task<BaseResponse<List<CustomerAddressDetailDto>>> GetAllDetailedAsync(bool onlyActives);
        Task<BaseResponse<List<CustomerAddressDto>>> GetByCustomerAsync(int customerId, bool onlyActives);
        Task<BaseResponse<CustomerAddressDetailDto>> GetAsync(int id);
        Task<BaseResponse<CustomerAddressDetailDto>> CreateAsync(CustomerAddressCreateRequestDto dto);
        Task<BaseResponse<bool>> UpdateAsync(CustomerAddressUpdateRequestDto dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
