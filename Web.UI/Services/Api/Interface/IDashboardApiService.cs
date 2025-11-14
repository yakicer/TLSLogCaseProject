using Contracts.Common;
using Contracts.DTO.Dashboard;

namespace Web.UI.Services.Api.Interface
{
    public interface IDashboardApiService
    {
        Task<BaseResponse<DashboardResponse>> GetAsync();
    }
}
