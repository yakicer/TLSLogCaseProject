using Web.UI.Helpers.Web.UI.Helpers;

namespace Web.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // klasik mvc ve security ayarlari
            builder.Services.AddAppMvcAndSecurity(builder.Configuration);

            // httpclient servisleri (apiye istek atmak icin)
            builder.Services.AddHttpClients(builder.Configuration);

            // apideki gibi servisler
            builder.Services.AddAppServices();

            //ui tarafinda role based yapi kullanabilmek icin gerekli ayarlar
            //builder.Services.AddUiJwtAuthentication(builder.Configuration);
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
