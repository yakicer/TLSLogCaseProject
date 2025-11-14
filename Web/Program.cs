using DataAccess.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Web.Helpers;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policy => policy.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader());
            });
            var apiSecretKey = Encoding.UTF8.GetBytes(config["JWT:ApiSecretKey"]!);


            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromHours(6);

            //    options.LoginPath = "/Account/Login";
            //    options.AccessDeniedPath = "/Account/Login";
            //    options.SlidingExpiration = true;
            //});

            //.AddCookie(options =>
            // {
            //     options.LoginPath = "/Account/Login";
            //     options.AccessDeniedPath = "/Account/Login";
            //     options.Cookie.HttpOnly = true;
            //     options.ExpireTimeSpan = TimeSpan.FromHours(12);
            //     options.SlidingExpiration = true;
            // })

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(12);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Giriþ sayfasý
                options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz eriþim
                options.Cookie.Name = "MyAppAuthCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
            });
            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDBContext>()
            //    .AddDefaultTokenProviders();

            //builder.Services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequiredLength = 6;
            //});
            // Token ekleyen HttpClient
            builder.Services.AddHttpClient("ApiClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7182/");
            }).AddHttpMessageHandler<JwtAuthorizationHandler>();

            builder.Services.AddAuthorization();
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.SwaggerDoc("v1", new OpenApiInfo { Title = "EducaDesign API", Version = "v1" });

            //    // JWT Authentication için yapýlandýrma
            //    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "Bearer",
            //        BearerFormat = "JWT",
            //        In = ParameterLocation.Header,
            //        Description = "JWT Token buraya girilmeli. Format: Bearer {token}"
            //    });
            //    //builder.Services.ConfigureApplicationCookie(options =>
            //    //{
            //    //    options.LoginPath = "/Account/Login"; // Giriþ yapmamýþ kullanýcýlar buraya yönlendirilir
            //    //    options.AccessDeniedPath = "/Account/Login"; // Yetkisi olmayanlar buraya yönlendirilir
            //    //    options.ReturnUrlParameter = "returnUrl";
            //    //    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);// Login sonrasý geri dönülecek sayfa
            //    //    options.SlidingExpiration = true; // Oturum süresi içinde hareket edilirse süre yenilensin
            //    //});
            //    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] { }
            //        }
            //    });
            //});
            builder.Services.AddTransientServices();

            var app = builder.Build();

            //Identity Rollerin oluþturulmasý
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    CreateRoles(services);
            //}

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseStatusCodePagesWithRedirects("/Account/Login?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads")),
            //    RequestPath = "/uploads"
            //});
            //app.UseCors();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Dashboard}/{action=Index}/{id?}");

            app.MapControllers();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.Run();
        }


        #region Identity CreateRoles
        //Identity Rollerin oluþturulmasý için gerekli method
        //protected static void CreateRoles(IServiceProvider serviceProvider)
        //{
        //    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //    string[] roleNames = { "Admin", "User", "Moderator" };
        //    foreach (var roleName in roleNames)
        //    {
        //        var roleExists = roleManager.RoleExistsAsync(roleName).Result;
        //        if (!roleExists)
        //        {
        //            roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
        //        }
        //    }
        //}
        #endregion
    }
}
