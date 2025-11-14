using Contracts.Common;
using Contracts.DTO.CustomerAddresses;
using Contracts.DTO.Customers;
using Contracts.DTO.Orders;

namespace Web.UI.Services.Api.Interface
{
    public interface ICustomerApiService
    {
        Task<BaseResponse<List<CustomerDto>>> GetAllAsync(bool onlyActives = true);
        Task<BaseResponse<CustomerDetailDto>> GetByIdAsync(int id);
        Task<BaseResponse<CustomerAddressDto>> AddAddressAsync(CustomerAddressCreateRequestDto dto);
        Task<BaseResponse<List<CustomerAddressDto>>> GetAddressesAsync(int customerId);
        Task<BaseResponse<CustomerDetailDto>> CreateAsync(CustomerCreateRequestDto dto);
        Task<BaseResponse<bool>> UpdateAsync(CustomerUpdateRequestDto dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);
        Task<BaseResponse<List<CustomerDetailDto>>> GetCustomersWithDifferentAddressesAsync();
        Task<BaseResponse<List<CustomerOrderDto>>> GetOrdersByCustomerNameAsync(string customerName);

    }
}
