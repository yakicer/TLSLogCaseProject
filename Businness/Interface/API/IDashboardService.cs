using Businness.Application;
using Entities.ResponseModel;

namespace Businness.Interface.API
{
    public interface IDashboardService
    {
        Task<BaseResponse<DashboardResponse>> GetDashboardAsync();
    }
}
