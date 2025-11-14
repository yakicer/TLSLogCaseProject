namespace Businness.Interface.Base
{


    //public interface IApiClientService
    //{
    //    Task<T> GetAsync<T>(HttpRequestMessage requestMessage, object? queryParams = null);
    //    Task<T> PostAsync<T, U>(string endpoint, U data);
    //    Task<List<T>> GetAllAsync<T>(HttpRequestMessage requestMessage);
    //    Task<T> PutAsync<T, U>(string endpoint, U data);
    //    Task<bool> DeleteAsync(string endpoint);
    //}

    public interface IApiClientService
    {
        Task<T> GetAsync<T>(string endpoint, object? queryParams = null, Dictionary<string, string>? headers = null);
        //Task<List<T>> GetListAsync<T>(string endpoint, object? queryParams = null, Dictionary<string, string>? headers = null);
        Task<T> PostAsync<T, U>(string endpoint, U data, Dictionary<string, string>? headers = null);
        Task<T> PutAsync<T, U>(string endpoint, U data, Dictionary<string, string>? headers = null);
        Task<bool> DeleteAsync(string endpoint, Dictionary<string, string>? headers = null);
    }

}
