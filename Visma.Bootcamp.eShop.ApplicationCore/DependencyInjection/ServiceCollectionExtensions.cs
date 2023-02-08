using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Visma.Bootcamp.eShop.ApplicationCore.Database;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Profiles;
using Visma.Bootcamp.eShop.ApplicationCore.Services;
using Visma.Bootcamp.eShop.ApplicationCore.Services.Interfaces;

namespace Visma.Bootcamp.eShop.ApplicationCore.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            AddAutoMapper(services);
            AddDatabase(services, configuration);
            AddLogging(services, environment);
            AddServices(services);
            AddCache(services);

            return services;
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICatalogService, CatalogService>();
            services.AddTransient<IBasketService, BasketService>();
        }

        #region Infrastructure
        private static void AddLogging(IServiceCollection services, IWebHostEnvironment environment)
        {
            services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddSerilog(new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(new EnvironmentLoggingLevelSwitch(environment))
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Conditional(l => environment.IsDevelopment(), c => c.Console())
                    .CreateLogger());
            });
        }

        private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseInMemoryDatabase("Visma.Bootcamp.eShop-db");
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile));
        }

        private static void AddCache(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<CacheManager>();
        }
        #endregion
    }

    internal class EnvironmentLoggingLevelSwitch : LoggingLevelSwitch
    {
        public EnvironmentLoggingLevelSwitch(IWebHostEnvironment environment)
        {
            MinimumLevel = environment.IsDevelopment()
                ? LogEventLevel.Debug
                : LogEventLevel.Information;
        }
    }
}
