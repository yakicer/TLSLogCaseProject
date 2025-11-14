//using Newtonsoft.Json;
using RestSharp;
using System.Text.Json;
using Web.Blazor.Models.ResponseModel;
using Web.Blazor.Services.Interface;

namespace Web.Blazor.Services.Implementation
{
    public class RestClientService : IRestClientService
    {
        private readonly ITokenService _tokenService;
        private readonly RestClient _client;
        private IConfiguration _configuration;
        private readonly string baseApiUrl;
        public RestClientService(ITokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
            baseApiUrl = _configuration["App:baseApiUrl"]!;
            var options = new RestClientOptions(baseApiUrl)
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(10)
            };
            _client = new RestClient(options);

            //_client = new RestClient(httpClient, options, configureSerialization: s =>
            //{
            //    var jsonOptions = new JsonSerializerOptions
            //    {
            //        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //        PropertyNameCaseInsensitive = true,
            //        WriteIndented = false
            //    };
            //    s.UseSystemTextJson(jsonOptions);
            //});
        }
        public async Task<BaseResponse<T>> GetAsync<T>(string endpoint)
        {
            var request = await PrepareRequest(endpoint, Method.Get);
            var response = await _client.ExecuteAsync<BaseResponse<T>>(request);
            //var response = JsonSerializer.Deserialize<BaseResponse<T>>(responseContent.Content);
            var JsonConvertObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<T>>(response.Content);
            var JsonResponse = JsonSerializer.Deserialize<BaseResponse<T>>(response.Content);
            Console.WriteLine("---------------------------");
            Console.WriteLine("Response: " + response);
            Console.WriteLine("---------------------------");
            Console.WriteLine("Content : " + response.Content);
            Console.WriteLine("---------------------------");
            Console.WriteLine("JsonResponse from response.Content : " + JsonResponse);
            Console.WriteLine("---------------------------");
            Console.WriteLine("JsonConvertObject from response.Content : " + JsonConvertObject);
            Console.WriteLine("---------------------------");
            return HandleResponse(response);
        }

        public async Task<BaseResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var request = await PrepareRequest(endpoint, Method.Post);
            //var request = new RestRequest(endpoint, Method.Post);
            var jsonData = JsonSerializer.Serialize(data);
            //request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
            request.AddJsonBody(jsonData);
            var response = await _client.ExecuteAsync<BaseResponse<TResponse>>(request);
            return HandleResponse(response);
        }

        public async Task<BaseResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var request = await PrepareRequest(endpoint, Method.Put);
            var jsonData = JsonSerializer.Serialize(data);
            //request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
            request.AddJsonBody(jsonData);
            var response = await _client.ExecuteAsync<BaseResponse<TResponse>>(request);
            return HandleResponse(response);
        }

        public async Task<BaseResponse<bool>> DeleteAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            var response = await _client.ExecuteAsync<BaseResponse<bool>>(request);
            return HandleResponse(response);
        }

        private BaseResponse<T> HandleResponse<T>(RestResponse<BaseResponse<T>> response)
        {
            if (response.IsSuccessful && response.Data != null)
            {
                return response.Data;
            }

            // Response başarısızsa veya null geldiyse fallback
            return new BaseResponse<T>
            {
                Success = false,
                Response = response.ErrorMessage ?? "Sunucu hatası",
                Errors = new List<string> { response.ErrorMessage ?? "Sunucuya ulaşılamadı." }
            };
        }
        private BaseResponse<T> HandleResponseBase<T>(BaseResponse<T> response)
        {
            if (response.Success && response.Data != null)
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine("response from HandleResponseBase : " + response);
                Console.WriteLine("---------------------------");
                Console.WriteLine("---------------------------");
                Console.WriteLine("response.Data from HandleResponseBase : " + response.Data);
                Console.WriteLine("---------------------------");
                return response;

            }

            // Response başarısızsa veya null geldiyse fallback
            return new BaseResponse<T>
            {
                Success = false,
                Response = response.Response ?? "Sunucu hatası",
                Errors = new List<string> { response.Response ?? "Sunucuya ulaşılamadı." }
            };
        }
        private async Task<RestRequest> PrepareRequest(string endpoint, Method method)
        {
            var request = new RestRequest(endpoint, method);

            var token = await _tokenService.GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("Authorization", $"Bearer {token}");
            }

            request.AddHeader("Accept", "application/json");
            return request;
        }
    }
}
