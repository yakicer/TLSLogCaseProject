using Businness.Application;
using Businness.Interface.API;
using Entities.Entity;
using Entities.RequestModel;
using Entities.ResponseModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Businness.Implementation.API
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddRoleAsync(string roleName)
        {
            try
            {
                var role = await _roleManager.RoleExistsAsync(roleName);
                if (!role)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Code = "", Description = "Rol Mevcut !" });

                }
            }
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError { Description = " Bilinmeyen bir hata oluştu. Lütfen sistem yöneticinize başvurunuz." });
            }

        }

        public async Task<IdentityResult> AssignRoleAsync(string userEmail, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı Bulunamadı !" });

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<BaseResponse<CurrentUserModel>> GetCurrentUserInfo(string userId)
        {

            if (string.IsNullOrEmpty(userId))
                return new BaseResponse<CurrentUserModel>() { Success = false, Response = "Geçerli Id'ye ait kullanıcı bulunamadı!" };

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return new BaseResponse<CurrentUserModel>() { Success = false, Response = "Kullanıcı bulunamadı!" };

            var userRoles = await _userManager.GetRolesAsync(user);

            var currentUser = new CurrentUserModel
            {
                UserId = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Name = user.Name,
                SurName = user.SurName,
                Roles = userRoles.ToList()
            };

            return new BaseResponse<CurrentUserModel>() { Success = false, Data = currentUser };

        }

        public async Task<string> LoginAsync(UserLoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return null;

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded) return null;

            return await GenerateJwtToken(user);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> RegisterAsync(UserRegisterModel model)
        {
            var user = new ApplicationUser
            {
                Name = model.Name,
                SurName = model.Surname,
                UserName = model.Email,
                Email = model.Email,
                CreatedBy = model.Name + " " + model.Surname,
                CreatedDate = DateTime.UtcNow,
                UpdatedBy = model.Name + " " + model.Surname,
                UpdatedDate = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!(await _roleManager.RoleExistsAsync("User")))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                }
                await _userManager.AddToRoleAsync(user, "User");
            }
            //else
            //{
            //    if (user != null)
            //        await _userManager.DeleteAsync(user);
            //}

            return result;
        }


        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("SurName", user.SurName)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:ApiSecretKey"]!));
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(8),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
