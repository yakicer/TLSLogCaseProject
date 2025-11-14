using Contracts.Common;
using Contracts.DTO.Customers;
using Contracts.DTO.Stocks;

namespace Web.UI.Services.Api.Interface
{
    public interface IStockApiService
    {
        Task<BaseResponse<List<StockDto>>> GetAllAsync(bool onlyActives = true);
        Task<BaseResponse<List<CustomerDetailDto>>> GetCustomersByStock(int stockId);
        Task<BaseResponse<StockDetailDto>> GetByIdAsync(int id);
        Task<BaseResponse<StockDetailDto>> CreateAsync(StockCreateDto dto);
        Task<BaseResponse<bool>> UpdateAsync(StockUpdateDto dto);
        Task<BaseResponse<bool>> DeleteAsync(int id);
    }
}
