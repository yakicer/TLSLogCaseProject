using Refit;
using Web.Blazor.Models;
using Web.Blazor.Models.RequestModel;
using Web.Blazor.Models.ResponseModel;

namespace Web.Blazor.Services.ApiServices
{
    //[RefitService]
    public interface IAuthService
    {
        [Post("/Auth/Register")]
        Task<string> Register(UserRegisterModel model);

        [Post("/Auth/Login")]
        Task<ApiResponse<string>> Login(UserLoginModel model);

        [Get("/Auth/Logout")]
        Task Logout();

        [Post("/Auth/SetUserRole")]
        Task<string> SetUserRole(string userEmail, string roleName);

        [Get("/Auth/GetCurrentUserInfo")]
        Task<UserModel> GetCurrentUserInfo(string userId);

        [Post("/Auth/ForgotPassword")]
        Task<BaseResponse<string>> ForgotPassword(ForgotPasswordRequestModel model);

        [Post("/Auth/ResetPassword")]
        Task<BaseResponse<string>> ResetPassword(ResetPasswordRequestModel model);
    }
}
