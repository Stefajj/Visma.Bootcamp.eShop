using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using Visma.Bootcamp.eShop.ApplicationCore.DependencyInjection;
using Visma.Bootcamp.eShop.ApplicationCore.Entities.DTO;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure;
using Visma.Bootcamp.eShop.ApplicationCore.Infrastructure.Middlewates;

namespace Visma.Bootcamp.eShop
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Visma.Bootcamp.eShop",
                    Description = "This is a simple eShop API used for education purposes only.",
                    License = new OpenApiLicense
                    {
                        Name = "Visma Labs s.r.o.",
                        Url = new Uri("https://visma.sk")
                    },
                    Contact = new OpenApiContact
                    {
                        Email = "tomas.blanarik@visma.com",
                        Name = "Tomas Blanarik - Github",
                        Url = new Uri("https://github.com/tomas-blanarik/Visma.Bootcamp.eShop")
                    },
                    Version = "1.0.0"
                });
                c.EnableAnnotations();
                c.AddServer(new OpenApiServer
                {
                    Description = "Development localhost server - Kestrel",
                    Url = "https://localhost:5001"
                });
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMemoryCache();
            services.AddApplicationServices(Configuration, Environment);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Visma.Bootcamp.eShop v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // custom middlewares here

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            SeedProducts(app.ApplicationServices);
        }

        private void SeedProducts(IServiceProvider provider) 
        {
            var cache = provider.GetRequiredService<CacheManager>();
            var list = new List<ProductDto>
            {
                new ProductDto
                {
                    ProductId = Guid.Parse("9f540087-e489-4288-81da-4ef7b9734bdb"),
                    Name = "test product #1",
                    Description = "test decription #1",
                    Price = 128.34M
                },
                new ProductDto
                {
                    ProductId = Guid.Parse("c2457c45-27e8-4d10-b7fe-5925c8a9a163"),
                    Name = "test product #2",
                    Description = "test description #2",
                    Price = 25.99M
                },
                new ProductDto
                {
                    ProductId = Guid.Parse("7e6710ea-b6aa-4da4-b37d-955133267e00"),
                    Name = "test product #3",
                    Description = "test description #3",
                    Price = 49.99M
                }
            };

            foreach(ProductDto item in list)
            {
                cache.Set(item);
            }
        }
    }
}
