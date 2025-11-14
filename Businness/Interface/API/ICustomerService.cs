using Businness.Application;
using Entities.ResponseModel;

namespace Businness.Interface.API
{
    public interface ICustomerService
    {
        Task<BaseResponse<List<CustomerListItem>>> GetAllAsync(bool onlyActives);
        Task<BaseResponse<CustomerDetailItem>> GetByIdAsync(int id);
        Task<BaseResponse<CustomerAddressItem>> AddAddressAsync(CustomerAddressCreateRequest dto);
        Task<BaseResponse<List<CustomerAddressItem>>> GetAddressesAsync(int customerId);
        Task<BaseResponse<CustomerDetailItem>> CreateAsync(CustomerCreateRequest model);
        Task<BaseResponse<bool>> UpdateAsync(CustomerUpdateRequest model);
        Task<BaseResponse<bool>> DeleteAsync(int id);

        //buralara da onlyACtives yapisi ekleyebilirdim ancak istenen mantik tam net belli olmadigi icin eklemedim
        Task<BaseResponse<List<CustomerDetailItem>>> GetCustomersWithDifferentAddressesAsync();
        Task<BaseResponse<List<CustomerOrderModel>>> GetOrdersByCustomerNameAsync(string customerName);

    }
}
