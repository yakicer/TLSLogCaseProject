using Businness.Interface.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace Businness.Implementation.Base
{
    public class ApiClientService : IApiClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly ILogger<ApiClientService> _logger;

        public ApiClientService(IHttpClientFactory httpClientFactory, ILogger<ApiClientService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private async Task<T> SendRequestAsync<T>(HttpRequestMessage request)
        {
            using var client = _httpClientFactory.CreateClient("ApiClient");

            try
            {
                _logger.LogInformation($"Sending {request.Method} request to {request.RequestUri}");

                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return default;
                }

                var contentString = await response.Content.ReadAsStringAsync();
                var contentData = JsonConvert.DeserializeObject<T>(contentString);

                //aşağıdaki yapı response doğru ve düzgün gelmesine rağmen response u doğru iletmiyor.
                //var contentStream = await response.Content.ReadAsStreamAsync();
                //var content = await JsonSerializer.DeserializeAsync<T>(contentStream);

                return contentData!;

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"Request {request.Method} to {request.RequestUri} failed");
                throw;
            }
        }

        //private async Task<List<T>> SendListRequestAsync<T>(HttpRequestMessage request)
        //{
        //    using var client = _httpClientFactory.CreateClient("ApiClient");

        //    try
        //    {
        //        _logger.LogInformation($"Sending {request.Method} request to {request.RequestUri}");

        //        var response = await client.SendAsync(request);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return default;
        //        }

        //        var contentString = await response.Content.ReadAsStringAsync();
        //        var contentData = JsonConvert.DeserializeObject<List<T>>(contentString);

        //        //aşağıdaki yapı response doğru ve düzgün gelmesine rağmen response u doğru iletmiyor.
        //        //var contentStream = await response.Content.ReadAsStreamAsync();
        //        //var content = await JsonSerializer.DeserializeAsync<T>(contentStream);

        //        return contentData!;

        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        _logger.LogError(ex, $"Request {request.Method} to {request.RequestUri} failed");
        //        throw;
        //    }
        //}

        public async Task<T> GetAsync<T>(string endpoint, object? queryParams = null, Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, BuildUrl(endpoint, queryParams));
            AddHeaders(request, headers);
            return await SendRequestAsync<T>(request);
        }
        //public async Task<List<T>> GetListAsync<T>(string endpoint, object? queryParams = null, Dictionary<string, string>? headers = null)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Get, BuildUrl(endpoint, queryParams));
        //    AddHeaders(request, headers);
        //    return await SendListRequestAsync<T>(request);
        //}

        public async Task<T> PostAsync<T, U>(string endpoint, U data, Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            AddHeaders(request, headers);
            return await SendRequestAsync<T>(request);
        }

        public async Task<T> PutAsync<T, U>(string endpoint, U data, Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, endpoint)
            {
                Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
            };
            AddHeaders(request, headers);
            return await SendRequestAsync<T>(request);
        }

        public async Task<bool> DeleteAsync(string endpoint, Dictionary<string, string>? headers = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
            AddHeaders(request, headers);
            var response = await SendRequestAsync<HttpResponseMessage>(request);
            return response.IsSuccessStatusCode;
        }
        private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
        {
            if (headers == null) return;
            foreach (var header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        private static string BuildUrl(string endpoint, object? queryParams)
        {
            if (queryParams == null) return endpoint;
            var queryString = string.Join("&", queryParams.GetType().GetProperties()
                .Select(p => $"{p.Name}={Uri.EscapeDataString(p.GetValue(queryParams)?.ToString() ?? "")}"));
            return $"{endpoint}?{queryString}";
        }
        #region Basit Api Yontemi
        //public async Task<List<T>> GetAllAsync<T>(string endpoint)
        //{
        //    var client = _httpClientFactory.CreateClient("ApiClient");


        //    var response = await client.GetAsync(endpoint);
        //    response.EnsureSuccessStatusCode();

        //    var content = await response.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<List<T>>(content)!;
        //}

        //public async Task<T> GetAsync<T>(string endpoint, object? queryParams = null)
        //{
        //    try
        //    {
        //        HttpRequestMessage sss;
        //        using (var client = _httpClientFactory.CreateClient())
        //        {
        //            if (queryParams != null)
        //            {
        //                var queryString = QueryStringBuilder(queryParams);
        //                endpoint += $"?{queryString}";
        //            }

        //            var response = await client.GetAsync(endpoint);
        //            response.EnsureSuccessStatusCode();

        //            var content = await response.Content.ReadAsStringAsync();

        //            return JsonConvert.DeserializeObject<T>(content)!;

        //        }

        //    }
        //    catch (HttpRequestException ex)
        //    {

        //        _logger.LogError(ex, $"Request failed");
        //        throw;
        //    }

        //}

        //private static string QueryStringBuilder(object queryParams)
        //{
        //    var properties = from p in queryParams.GetType().GetProperties()
        //                     where p.GetValue(queryParams, null) != null
        //                     select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(queryParams, null).ToString())}";

        //    return string.Join("&", properties);
        //}
        #endregion
    }
}
