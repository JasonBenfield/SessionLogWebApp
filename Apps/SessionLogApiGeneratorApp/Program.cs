using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PermanentLogGroupApi;
using SessionLogWebApp.Api;
using System.Threading.Tasks;
using XTI_ApiGeneratorApp.Extensions;
using XTI_App;
using XTI_App.Api;
using XTI_Configuration.Extensions;
using XTI_Core;

namespace SessionLogApiGeneratorApp
{
    class Program
    {
        static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.UseXtiConfiguration(hostingContext.HostingEnvironment, args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApiGenerator(hostContext.Configuration);
                    services.AddSingleton<Clock, UtcClock>();
                    services.AddScoped<AppFactory>();
                    services.AddScoped<PermanentLog>();
                    services.AddScoped<IAppApiUser, AppApiSuperUser>();
                    services.AddScoped<AppApiFactory, SessionLogAppApiFactory>();
                    services.AddScoped(sp =>
                    {
                        var factory = sp.GetService<AppApiFactory>();
                        return factory.CreateForSuperUser();
                    });
                    services.AddScoped(sp => (SessionLogAppApi)sp.GetService<IAppApi>());
                    services.AddHostedService<ApiGeneratorHostedService>();
                })
                .RunConsoleAsync();
        }
    }
}
