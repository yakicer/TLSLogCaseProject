using Businness.Application;
using Entities.ResponseModel;

namespace Businness.Interface.API
{

    public interface ICustomerAddressService
    {
        Task<BaseResponse<List<CustomerAddressListItem>>> GetAllAddressesAsync(bool onlyActives);
        Task<BaseResponse<List<CustomerAddressDetailItem>>> GetAllAddressesDetailedAsync(bool onlyActives);
        Task<BaseResponse<List<CustomerAddressListItem>>> GetByCustomerAsync(int customerId, bool onlyActives);
        Task<BaseResponse<CustomerAddressDetailItem>> GetByIdAsync(int id);
        Task<BaseResponse<CustomerAddressDetailItem>> CreateAsync(CustomerAddressCreateRequest dto);
        Task<BaseResponse<bool>> UpdateAsync(CustomerAddressUpdateRequest dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
