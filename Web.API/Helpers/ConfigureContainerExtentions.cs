using Businness.Implementation.API;
using Businness.Interface.API;
using DataAccess.Repository.Base;
using DataAccess.Repository.Implementation;
using DataAccess.Repository.Interface;

namespace Web.Helpers
{
    public static class ConfigureContainerExtentions
    {
        public static void AddTransientServices(this IServiceCollection services)
        {
            //Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IStockRepository, StockRepository>();

            //Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IDashboardService, DashboardService>();
            //Identity
            services.AddScoped<IAuthService, AuthService>();

            //BaseServices
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Helpers
            //services.AddTransient<JwtAuthorizationHandler>();

        }
    }
}
