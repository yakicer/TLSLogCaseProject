using Microsoft.AspNetCore.Components.Authorization;
using Web.Blazor.Services.ApiServices;
using Web.Blazor.Services.Implementation;
using Web.Blazor.Services.Interface;

namespace Web.Blazor.Helpers.Extensions
{
    public static class ConfigureContainerExtentions
    {
        public static void AddTransientServices(this IServiceCollection services)
        {
            #region Scoped

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            #endregion

            #region Singleton

            services.AddSingleton<LocalStorageService>();

            #endregion

            #region Transient

            services.AddTransient<AuthHeaderHandler>();
            services.AddTransient<ApiResponseHandler>();
            services.AddTransient<IAuthStateService, AuthStateService>();

            #endregion
        }
    }
}
