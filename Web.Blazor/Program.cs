using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Radzen;
using Refit;
using System.Reflection;
using Web.Blazor.Helpers.Extensions;
using Web.Blazor.Services.ApiServices;
namespace Web.Blazor
{
    public class Program
    {
        private static string baseApiUrl = string.Empty;
        private static string baseUrl = string.Empty;

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            baseApiUrl = builder.Configuration["App:baseApiUrl"]!;
            baseUrl = builder.Configuration["App:baseUrl"]!;
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");
            builder.Services.AddRadzenComponents();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseApiUrl) });
            builder.Services.AddBlazoredToast();
            builder.Services.AddTransientServices();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddScoped(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();

                return RestService.For<IAuthService>(httpClient);
            });
            //builder.Services.AddScoped(sp =>
            //{
            //    var httpClient = sp.GetRequiredService<HttpClient>();

            //    return RestService.For<IProjectService>(httpClient);
            //});

            //bunu max request body size hatasi icin kullanabilirsin
            //var handler = new HttpClientHandler
            //{
            //    MaxRequestContentBufferSize = 75 * 1000,
            //};
            AddRefitServices(builder);

            #region LocalizationSettings(Disabled)
            //builder.Services.AddLocalization();

            //var host = builder.Build();

            //const string defaultCulture = "tr-TR";

            //var culture = CultureInfo.GetCultureInfo(defaultCulture);


            //CultureInfo.DefaultThreadCurrentCulture = culture;
            //CultureInfo.DefaultThreadCurrentUICulture = culture;

            //await host.RunAsync();
            #endregion

            await builder.Build().RunAsync();
        }

        private static void AddRefitServices(WebAssemblyHostBuilder builder)
        {
            var nullTask = Task.FromResult<Exception>(null);
            var refitSettings = new RefitSettings
            {
                ExceptionFactory = (response) => CustomApiExceptionFactory.Create(response),
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            ProcessDictionaryKeys = true,
                            OverrideSpecifiedNames = false
                        }
                    },
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                })
            };

            //builder.Services.AddRefitClient<IProjectService>(refitSettings)
            //    .ConfigureHttpClient(ConfigureClient)
            //   .AddHttpMessageHandler<AuthHeaderHandler>()
            //    .AddHttpMessageHandler<ApiResponseHandler>();

            #region AddRefitServices(Single)
            //builder.Services.AddRefitClient<IProjectService>(refitSettings)
            //    .ConfigureHttpClient(ConfigureClient)
            //    .AddHttpMessageHandler<AuthHeaderHandler>()
            //    .AddHttpMessageHandler<ApiResponseHandler>();
            #endregion

            #region AddRefitServices(MultipleWithExtension)
            builder.Services.AddAllRefitClientsWithHandlers(
                Assembly.GetExecutingAssembly(),
                refitSettings,
                ConfigureClient);

            #endregion
        }

        private static void ConfigureClient(HttpClient client)
        {
            client.BaseAddress = new Uri(baseApiUrl);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
