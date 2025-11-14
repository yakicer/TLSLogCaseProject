using Refit;
using System.Reflection;
using Web.Blazor.Helpers.Attributes;

namespace Web.Blazor.Helpers.Extensions
{
    public static class RefitClientRegistrationExtensions
    {
        public static IServiceCollection AddAllRefitClientsWithHandlers(
            this IServiceCollection services,
            Assembly assembly,
            RefitSettings settings,
            Action<HttpClient> configureClient)
        {
            var serviceInterfaces = assembly
                .GetTypes()
                .Where(t => t.IsInterface && t.GetCustomAttribute<RefitServiceAttribute>() != null);

            foreach (var serviceInterface in serviceInterfaces)
            {
                var method = typeof(RefitClientRegistrationExtensions)
                    .GetMethod(nameof(AddRefitClientWithHandlers), BindingFlags.Public | BindingFlags.Static)!
                    .MakeGenericMethod(serviceInterface);

                method.Invoke(null, new object[] { services, settings, configureClient });
            }

            return services;
        }

        public static IServiceCollection AddRefitClientWithHandlers<T>(
            this IServiceCollection services,
            RefitSettings settings,
            Action<HttpClient> configureClient)
            where T : class
        {
            services.AddRefitClient<T>(settings)
                    .ConfigureHttpClient(configureClient)
                    .AddHttpMessageHandler<AuthHeaderHandler>()
                    .AddHttpMessageHandler<ApiResponseHandler>();

            return services;
        }
    }

}
