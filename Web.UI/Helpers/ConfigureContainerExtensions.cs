namespace Web.UI.Helpers
{
    using global::Web.UI.Services.Api.Implementation;
    using global::Web.UI.Services.Api.Interface;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;

    namespace Web.UI.Helpers
    {
        public static class ConfigureContainerExtensions
        {
            // mvc cookie, auth vs islemleri
            public static IServiceCollection AddAppMvcAndSecurity(
                this IServiceCollection services,
                IConfiguration configuration)
            {
                services.AddControllersWithViews();
                services.AddHttpContextAccessor();

                // antiforgery (ajax islemleri headerlari icin)
                services.AddAntiforgery(o =>
                {
                    o.FormFieldName = "__RequestVerificationToken";
                    o.HeaderName = "RequestVerificationToken";
                    o.SuppressXFrameOptionsHeader = false;
                });

                // temiz bir cookie akis semasi olsun
                services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Account/Login";
                        options.LogoutPath = "/Account/Logout";
                        options.AccessDeniedPath = "/Account/AccessDenied";
                        options.SlidingExpiration = true;
                        options.ExpireTimeSpan = TimeSpan.FromHours(8);
                    });

                return services;
            }

            // Httpclient islemleri
            public static IServiceCollection AddHttpClients(
                this IServiceCollection services,
                IConfiguration configuration)
            {
                var baseUrl = configuration["Api:BaseUrl"]
                              ?? throw new InvalidOperationException("Api:BaseUrl not configured.");

                services.AddHttpClient("ApiClient", client =>
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                }).AddHttpMessageHandler<JwtCookieHandler>();


                //http servisleri (ui servisleri eklenecek)
                services.AddHttpClient<ICustomerApiService, CustomerApiService>()
                        .ConfigureHttpClient((sp, client) =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        })
                        .AddHttpMessageHandler<JwtCookieHandler>();

                services.AddHttpClient<IOrderApiService, OrderApiService>()
                        .ConfigureHttpClient((sp, client) =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        })
                        .AddHttpMessageHandler<JwtCookieHandler>();

                services.AddHttpClient<IStockApiService, StockApiService>()
                        .ConfigureHttpClient((sp, client) =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        })
                        .AddHttpMessageHandler<JwtCookieHandler>();

                services.AddHttpClient<ICustomerAddressApiService, CustomerAddressApiService>()
                        .ConfigureHttpClient((sp, client) =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        })
                        .AddHttpMessageHandler<JwtCookieHandler>();

                services.AddHttpClient<IDashboardApiService, DashboardApiService>()
                        .ConfigureHttpClient((sp, client) =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        })
                        .AddHttpMessageHandler<JwtCookieHandler>();

                services.AddHttpClient<IAuthApiService, AuthApiService>()
                        .ConfigureHttpClient((sp, client) =>
                        {
                            client.BaseAddress = new Uri(baseUrl);
                        });


                return services;
            }

            // genel app servisleri
            public static IServiceCollection AddAppServices(this IServiceCollection services)
            {
                services.AddTransient<JwtCookieHandler>();
                services.AddScoped<IAuthApiService, AuthApiService>();


                return services;
            }

            public static IServiceCollection AddUiJwtAuthentication(this IServiceCollection services, IConfiguration cfg)
            {
                var key = cfg["Jwt:ApiSecretKey"] ?? "";
                var issuer = cfg["Jwt:Issuer"];
                var audience = cfg["Jwt:Audience"];
                var cookieName = cfg["Auth:CookieName"] ?? "AuthToken";

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var token = ctx.HttpContext.Request.Cookies[cookieName];
                            if (!string.IsNullOrEmpty(token)) ctx.Token = token;
                            return Task.CompletedTask;
                        }
                    };

                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = !string.IsNullOrWhiteSpace(issuer),
                        ValidIssuer = issuer,
                        ValidateAudience = !string.IsNullOrWhiteSpace(audience),
                        ValidAudience = audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        RoleClaimType = ClaimTypes.Role
                    };
                });

                services.AddAuthorizationBuilder()
                    .AddPolicy("AdminOnly", p => p.RequireRole("Administrator"));

                return services;
            }
        }
    }

}
