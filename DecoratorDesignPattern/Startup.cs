using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DecoratorDesignPattern.OpenWeatherMap;
using DecoratorDesignPattern.WeatherInterface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DecoratorDesignPattern
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddMemoryCache();

            services.AddScoped<IWeatherService>(serviceProvider => 
            {
                string apiKey = Configuration.GetValue<string>("AppSettings:OpenWeatherMapApiKey");
                var logger = serviceProvider.GetService<ILogger<WeatherServiceLoggingDecorator>>();
                var memoryCache = serviceProvider.GetService<IMemoryCache>();
                IWeatherService innerService = new WeatherService(apiKey);
                IWeatherService withLoggingDecorator = new WeatherServiceLoggingDecorator(innerService, logger);
                IWeatherService withCachingDecorator = new WeatherServiceCachingDecorator(withLoggingDecorator, memoryCache);
                return withCachingDecorator;
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
