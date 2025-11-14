using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Web.Blazor.Models.RequestModel;
using Web.Blazor.Models.ResponseModel;
using Web.Blazor.Services.ApiServices;
using Web.Blazor.Services.Interface;

namespace Web.Blazor.Services.Implementation
{
    public class AuthStateService : IAuthStateService
    {
        private readonly IAuthService _authService;
        private readonly IToastService _toastService;
        private readonly ITokenService _tokenService;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly NavigationManager _navigationManager;

        public AuthStateService(IAuthService authService,
            IToastService toastService,
            ITokenService tokenService,
            AuthenticationStateProvider authProvider,
            NavigationManager navigationManager)
        {
            _authService = authService;
            _toastService = toastService;
            _tokenService = tokenService;
            _authStateProvider = authProvider;
            _navigationManager = navigationManager;
        }

        public async Task<AuthenticationState> GetCurrentAuthState()
        {
            return await ((CustomAuthStateProvider)_authStateProvider).GetAuthenticationStateAsync();
        }

        public async Task Login(UserLoginModel model)
        {
            var resp = await _authService.Login(model);
            if (resp == null)
            {
                _toastService.ShowError("Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz");
                return;
            }
            if (!resp.IsSuccessStatusCode)
            {
                if (resp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    _toastService.ShowError("Kullanıcı adı veya şifre yanlış! Lütfen kontrol edip tekrar deneyiniz.", settings => { settings.Timeout = 4; settings.ShowProgressBar = true; });
                else
                {
                    _toastService.ShowError(resp.Error.Message);
                }
                return;
            }
            var baseResp = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseResponse<string>>(resp.Content!);

            var token = baseResp!.Data;
            if (string.IsNullOrEmpty(token))
            {
                _toastService.ShowError("Oturum süresi dolmuş. Lütfen tekrar giriş yapınız.");
                return;
            }
            await _tokenService.SetAccessTokenAsync(token);
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);

            _toastService.ShowSuccess("Giriş başarılı !", settings => { settings.Timeout = 3; settings.ShowProgressBar = false; });
            _navigationManager.NavigateTo("/");

        }

        public async Task Logout()
        {
            await _tokenService.RemoveAccessTokenAsync();
            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }

        public Task<string> Register(UserRegisterModel model)
        {
            throw new NotImplementedException();
        }
        public async Task ForgotPassword(ForgotPasswordRequestModel model)
        {
            try
            {
                var response = await _authService.ForgotPassword(model);
                if (response == null)
                {
                    _toastService.ShowError("Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz");
                    return;
                }

                //var baseResp = new BaseResponse<string> { Success = false, Response = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.", Data = "Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz." };

                if (!response.Success)
                {
                    _toastService.ShowError(response!.Response, settings => { settings.Timeout = 5; settings.ShowProgressBar = true; });
                    return;
                }
                _toastService.ShowSuccess(response!.Data, settings => { settings.Timeout = 5; settings.ShowProgressBar = false; });
                _navigationManager.NavigateTo("/login");

            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz. Hata Mesajı: {ex.Message}");
            }
        }

        public async Task ResetPassword(ResetPasswordRequestModel model)
        {
            try
            {
                var response = await _authService.ResetPassword(model);
                if (response == null)
                {
                    _toastService.ShowError("Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz");
                    return;
                }

                if (!response.Success)
                {
                    _toastService.ShowError(response!.Response, settings => { settings.Timeout = 5; settings.ShowProgressBar = true; });
                    return;
                }
                _toastService.ShowSuccess(response!.Data, settings => { settings.Timeout = 5; settings.ShowProgressBar = false; });
                _navigationManager.NavigateTo("/login");

            }
            catch (Exception ex)
            {
                _toastService.ShowError($"Beklenmeyen bir hata oluştu. Lütfen daha sonra tekrar deneyiniz. Hata Mesajı: {ex.Message}");
            }
        }

        public Task<string> SetUserRole(string userEmail, string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
