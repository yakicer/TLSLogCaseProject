using Microsoft.AspNetCore.Components.Authorization;
using Web.Blazor.Models.RequestModel;

namespace Web.Blazor.Services.Interface
{
    public interface IAuthStateService
    {
        Task<string> Register(UserRegisterModel model);
        Task Login(UserLoginModel model);
        Task Logout();
        Task<string> SetUserRole(string userEmail, string roleName);
        Task<AuthenticationState> GetCurrentAuthState();
        Task ForgotPassword(ForgotPasswordRequestModel model);
        Task ResetPassword(ResetPasswordRequestModel model);

    }
}
