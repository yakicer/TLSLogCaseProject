using Businness.Application;
using Entities.ResponseModel;

namespace Businness.Interface.API
{
    public interface IStockService
    {
        Task<BaseResponse<List<StockListItem>>> GetAllAsync(bool onlyActives);
        Task<BaseResponse<StockDetailItem>> GetByIdAsync(int id);
        Task<BaseResponse<StockDetailItem>> CreateAsync(StockCreateRequest dto);
        Task<BaseResponse<bool>> UpdateAsync(StockUpdateRequest dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);
        Task<BaseResponse<List<CustomerDetailItem>>> GetCustomersWhoBoughtStockAsync(int stockId);

    }

}
