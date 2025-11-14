using Contracts.Common;
using Contracts.DTO.Auth;

namespace Web.UI.Services.Api.Interface
{
    public interface IAuthApiService
    {
        Task<BaseResponse<string>> LoginAsync(LoginRequestDto dto);
        Task<BaseResponse<UserProfileDto>> MeAsync();
        Task<BaseResponse<string>> LogoutAsync();
    }
}
