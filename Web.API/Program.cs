using DataAccess.Context;
using Entities.Entity;
using Entities.RequestModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Web.Helpers;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

//dbcontext appsettings.josn dan cek
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

// identity auth
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

//guvenli password politikasi
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
});

// jwt settings parameters etc
var apiSecretKey = Encoding.UTF8.GetBytes(config["JWT:ApiSecretKey"]!);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            ValidIssuer = config["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(apiSecretKey)
        };
    });

// role based - gerek kalmadi sanirim nolur nolmaz kalsin simdilik
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", p => p.RequireRole("Admin"));
//    options.AddPolicy("UserPolicy", p => p.RequireRole("User"));
//});

// cors for mvc - development mode oldugu icin strict bir yapi kurmuyorum buraya
builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});


// controllers + newtonsoftJson for reference loop handling - bazen hata verdiriyor nolur nolmaz ekledim
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(x =>
        x.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // sacma swagger ornekleri yerine istege bagli example ekelemk icin basit bir yapi - user experience var da developer experience yok mu ??
    var modelAssembly = typeof(UserRegisterModel).Assembly;
    var apiAssembly = Assembly.GetExecutingAssembly();
    var assemblies = new List<Assembly> { modelAssembly, apiAssembly };

    foreach (var assembly in assemblies.Distinct())
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    }

    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TLS Log Case API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token: Bearer {token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
    });
});

// custom di services - toplu dursun da ugrasmayalim her seferinde
builder.Services.AddTransientServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
