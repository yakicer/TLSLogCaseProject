using Web.Blazor.Models.ResponseModel;

namespace Web.Blazor.Services.Interface
{
    public interface IRestClientService
    {
        Task<BaseResponse<T>> GetAsync<T>(string endpoint);
        Task<BaseResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<BaseResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<BaseResponse<bool>> DeleteAsync(string endpoint);
    }

}
