using Entities.DTO;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace Web.Controllers
{
    public class BaseController : Controller
    {
        //yazdim ama kullanmayacam muhtemelen
        protected BaseResponse<CurrentUserDTO> GetCurrentUser()
        {
            var isAuthenticated = User.Identity!.IsAuthenticated;

            if (!isAuthenticated)
                return new BaseResponse<CurrentUserDTO> { Success = false, Response = "Kullanıcı bilgileri bulunamadı!" };

            var user = new CurrentUserDTO
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "",
                UserName = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                Name = User.FindFirst(ClaimTypes.Name)?.Value ?? "",
                SurName = User.FindFirst("SurName")?.Value ?? "",
                Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                Roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
            };

            return new BaseResponse<CurrentUserDTO> { Success = true, Data = user };
        }


    }
}
