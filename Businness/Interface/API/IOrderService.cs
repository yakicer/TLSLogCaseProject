using Businness.Application;
using Entities.ResponseModel;

namespace Businness.Interface.API
{
    public interface IOrderService
    {
        Task<BaseResponse<List<OrderListItem>>> GetAllAsync(bool onlyActives);
        Task<BaseResponse<OrderDetailItem>> GetByIdAsync(int id);
        Task<BaseResponse<int>> CreateAsync(OrderCreateRequest dto);
        Task<BaseResponse<int>> UpdateAsync(OrderUpdateRequest dto);
        Task<BaseResponse<int>> UpdateStatusAsync(OrderStatusUpdateRequest dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);

        //buralara da onlyACtives yapisi ekleyebilirdim ancak istenen mantik tam net belli olmadigi icin eklemedim
        Task<BaseResponse<List<CustomerOrderModel>>> GetOrdersWithMinQuantityAsync(decimal minAmount);
        Task<BaseResponse<CityOrderCountModel>> GetOrderCountByCityAsync(string city);

    }
}
