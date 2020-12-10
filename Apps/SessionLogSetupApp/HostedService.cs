using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SessionLogWebApp.Api;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SessionLogSetupApp
{
    public sealed class HostedService : IHostedService
    {
        private readonly IServiceProvider sp;

        public HostedService(IServiceProvider sp)
        {
            this.sp = sp;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = sp.CreateScope();
            var setup = scope.ServiceProvider.GetService<SessionLogSetup>();
            await setup.Run();
            var lifetime = sp.GetService<IHostApplicationLifetime>();
            lifetime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
