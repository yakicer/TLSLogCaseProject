using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Refit;
using System.Net.Http.Json;
using Web.Blazor.Models.ResponseModel;
using Web.Blazor.Services.Interface;

namespace Web.Blazor.Helpers
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;
        private readonly NavigationManager _navigationManager;
        private readonly IAuthStateService _authStateService;

        public AuthHeaderHandler(
            ITokenService tokenService,
            NavigationManager navigationManager,
            IAuthStateService authStateService,
            IToastService toastService)
        {
            _tokenService = tokenService;
            _navigationManager = navigationManager;
            _authStateService = authStateService;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetAccessTokenAsync();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var authState = await _authStateService.GetCurrentAuthState();
                var user = authState.User;
                if (user != null && user.Identity!.IsAuthenticated)
                {
                    await _authStateService.Logout();
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                var message = await response.Content.ReadFromJsonAsync<BaseResponse<string>>().ConfigureAwait(false);
                throw new Exception(message!.Data);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                if (response.Content.Headers.ContentType?.MediaType == "application/problem+json")
                {
                    var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>().ConfigureAwait(false);
                    if (problemDetails != null)
                    {
                        if (problemDetails.Errors.Count > 0)
                        {
                            string[] errorMessageValue = problemDetails.Errors.FirstOrDefault().Value;
                            string message = errorMessageValue[0];
                            if (errorMessageValue[0].Contains("max request body size"))
                                message = "Maksimum toplam dosya boyutunu (100 MB) aştınız ! Lütfen yüklenen dosyaların toplam boyutunu kontrol ederek tekrar deneyiniz.";
                            throw new Exception(message);
                        }
                    }
                }
                else
                {
                    var message = await response.Content.ReadFromJsonAsync<BaseResponse<string>>().ConfigureAwait(false);
                    throw new Exception(message!.Data);
                }
            }
            return response;
        }

    }
}
