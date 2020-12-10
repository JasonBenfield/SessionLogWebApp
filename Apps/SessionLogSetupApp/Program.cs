using MainDB.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SessionLogWebApp.Api;
using System.Threading.Tasks;
using XTI_App;
using XTI_Configuration.Extensions;
using XTI_Core;

namespace SessionLogSetupApp
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
                    services.AddAppDbContextForSqlServer(hostContext.Configuration);
                    services.AddScoped<AppFactory>();
                    services.AddSingleton<Clock, UtcClock>();
                    services.AddScoped<SessionLogSetup>();
                    services.AddHostedService<HostedService>();
                })
                .RunConsoleAsync();
        }
    }
}
