using Businness.Implementation.API;
using Businness.Implementation.Base;
using Businness.Interface.API;
using Businness.Interface.Base;
using DataAccess.Repository.Implementation;
using DataAccess.Repository.Interface;

namespace Web.Helpers
{
    public static class ConfigureContainerExtentions
    {
        public static void AddTransientServices(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ICareerRepository, CareerRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<IProjectImageRepository, ProjectImageRepository>();
            services.AddScoped<ISolutionRepository, SolutionRepository>();
            services.AddScoped<ISystemSettingsRepository, SystemSettingsRepository>();
            services.AddScoped<IThumbLogoRepository, ThumbLogoRepository>();

            //Services
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IProjectImageService, ProjectImageService>();
            services.AddTransient<ICareerService, CareerService>();
            services.AddTransient<IHomeService, HomeService>();
            services.AddTransient<ISolutionService, SolutionService>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<ISystemManagementService, SystemManagementService>();
            services.AddTransient<ITeamService, TeamService>();

            //Identity
            services.AddScoped<IAuthService, AuthService>();

            //BaseServices
            services.AddTransient<IApiClientService, ApiClientService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ITokenService, TokenService>();

            //Helpers
            services.AddTransient<JwtAuthorizationHandler>();

        }
    }
}
