using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SessionLogWebApp.Api;
using System.Threading.Tasks;
using XTI_ApiGeneratorApp.Extensions;
using XTI_App;
using XTI_App.Api;
using XTI_Configuration.Extensions;
using XTI_Core;
using XTI_PermanentLog;

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
                    services.AddScoped<SessionFactory>();
                    services.AddScoped<PermanentLog>();
                    services.AddScoped<AppApi>(sp =>
                    {
                        var permanentLog = sp.GetService<PermanentLog>();
                        return new SessionLogAppApi
                        (
                            SessionLogAppKey.AppKey,
                            AppVersionKey.Current,
                            new AppApiSuperUser(),
                            permanentLog
                        );
                    });
                    services.AddScoped<IAppApiTemplateFactory, SessionLogAppApiTemplateFactory>();
                    services.AddHostedService<ApiGeneratorHostedService>();
                })
                .RunConsoleAsync();
        }
    }
}
