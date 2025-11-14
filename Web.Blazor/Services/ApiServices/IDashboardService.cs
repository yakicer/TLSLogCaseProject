using Refit;
using Web.Blazor.Helpers.Attributes;
using Web.Blazor.Models;

namespace Web.Blazor.Services.ApiServices
{
    [RefitService]
    public interface IDashboardService
    {
        [Get("/Dashboard/GetProjectCompletionStatus")]
        Task<List<ChartModel>> GetProjectCompletionStatus();

        [Get("/Dashboard/GetProjectCountByYear")]
        Task<List<ChartModel>> GetProjectCountByYear();

        [Get("/Dashboard/GetProjectGrowthOverTime")]
        Task<List<ChartModel>> GetProjectGrowthOverTime();

        [Get("/Dashboard/GetProjectCompletionRate")]
        Task<double> GetProjectCompletionRate();

        [Get("/Dashboard/GetSolutionCountByType")]
        Task<List<ChartModel>> GetSolutionCountByType();

        [Get("/Dashboard/GetSolutionGrowthOverTime")]
        Task<List<ChartModel>> GetSolutionGrowthOverTime();

        [Get("/Dashboard/GetEmployeeByDepartment")]
        Task<List<ChartModel>> GetEmployeeByDepartment();

        [Get("/Dashboard/GetEmployeeAgeDistribution")]
        Task<List<ChartModel>> GetEmployeeAgeDistribution();

        [Get("/Dashboard/GetCareerByDepartment")]
        Task<List<ChartModel>> GetCareerByDepartment();

        [Get("/Dashboard/GetOngoingProjectCount")]
        Task<int> GetOngoingProjectCount();

        [Get("/Dashboard/GetCompletedProjectCount")]
        Task<int> GetCompletedProjectCount();

        [Get("/Dashboard/GetTotalProjectCount")]
        Task<int> GetTotalProjectCount();

        [Get("/Dashboard/GetThisYearProjectCount")]
        Task<int> GetThisYearProjectCount();

        [Get("/Dashboard/GetTotalSolutionCount")]
        Task<int> GetTotalSolutionCount();

        [Get("/Dashboard/GetTotalEmployeeCount")]
        Task<int> GetTotalEmployeeCount();

        [Get("/Dashboard/GetMostCrowdedDepartment")]
        Task<ChartModel> GetMostCrowdedDepartment();

        [Get("/Dashboard/GetTotalCareerCount")]
        Task<int> GetTotalCareerCount();

        [Get("/Dashboard/RemoveCaches")]
        Task RemoveCaches();
    }
}
