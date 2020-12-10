using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SessionLogWebApp.Api;
using SessionLogWebApp.ApiControllers;
using XTI_App;
using XTI_App.Api;
using XTI_PermanentLog;
using XTI_WebApp.Extensions;

namespace SessionLogWebApp.Extensions
{
    public static class Extensions
    {
        public static void AddSessionLogServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebAppServices(configuration);
            services.AddSingleton(sp => SessionLogAppKey.AppKey);
            services.AddScoped<SessionFactory>();
            services.AddScoped<PermanentLog>();
            services.AddScoped(sp =>
            {
                var appKey = sp.GetService<AppKey>();
                var path = sp.GetService<XtiPath>();
                var user = sp.GetService<IAppApiUser>();
                var permanentLog = sp.GetService<PermanentLog>();
                return new SessionLogAppApi
                (
                    appKey,
                    path.Version,
                    user,
                    permanentLog
                );
            });
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
