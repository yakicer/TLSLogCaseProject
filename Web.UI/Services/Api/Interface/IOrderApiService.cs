using Contracts.Common;
using Contracts.DTO.Customers;
using Contracts.DTO.Orders;

namespace Web.UI.Services.Api.Interface
{
    public interface IOrderApiService
    {
        Task<BaseResponse<List<OrderDto>>> GetAllAsync(bool onlyActives = true);
        Task<BaseResponse<List<CustomerOrderDto>>> GetOrdersWithMinQuantityAsync(decimal minAmount);
        Task<BaseResponse<CityOrderCountDto>> GetOrderCountByCityAsync(string city);
        Task<BaseResponse<OrderDetailDto>> GetByIdAsync(int id);
        Task<BaseResponse<int>> CreateAsync(OrderCreateRequestDto dto);
        Task<BaseResponse<int>> UpdateAsync(OrderUpdateRequestDto dto);
        Task<BaseResponse<int>> UpdateStatusAsync(OrderStatusUpdateRequestDto dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);

    }
}
