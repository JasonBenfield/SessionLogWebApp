using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PermanentLogGroupApi;
using SessionLogWebApp.Api;
using SessionLogWebApp.ApiControllers;
using XTI_App.Api;
using XTI_WebApp.Extensions;

namespace SessionLogWebApp.Extensions
{
    public static class Extensions
    {
        public static void AddSessionLogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebAppServices(configuration);
            services.AddSingleton(sp => SessionLogAppKey.AppKey);
            services.AddScoped<PermanentLog>();
            services.AddScoped<AppApiFactory, SessionLogAppApiFactory>();
            services.AddScoped(sp => (SessionLogAppApi)sp.GetService<IAppApi>());
            services
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                })
                .AddMvcOptions(options =>
                {
                });
            services.AddControllersWithViews()
                .PartManager.ApplicationParts.Add
                (
                    new AssemblyPart(typeof(PermanentLogController).Assembly)
                );
        }
    }
}
