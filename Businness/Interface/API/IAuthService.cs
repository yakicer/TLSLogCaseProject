using Businness.Application;
using Entities.RequestModel;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Identity;

namespace Businness.Interface.API
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(UserRegisterModel model);
        Task<string> LoginAsync(UserLoginModel model);
        Task LogoutAsync();
        Task<IdentityResult> AssignRoleAsync(string userEmail, string roleName);
        Task<IdentityResult> AddRoleAsync(string roleName);
        Task<BaseResponse<CurrentUserModel>> GetCurrentUserInfo(string userId);



    }
}
