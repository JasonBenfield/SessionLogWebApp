using System.Threading.Tasks;
using XTI_App;
using XTI_App.Api;
using XTI_Core;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogSetup : IAppSetup
    {
        private readonly AppFactory appFactory;
        private readonly Clock clock;
        private readonly AppApiFactory apiFactory;

        public SessionLogSetup(AppFactory appFactory, Clock clock, AppApiFactory apiFactory)
        {
            this.appFactory = appFactory;
            this.clock = clock;
            this.apiFactory = apiFactory;
        }

        public async Task Run()
        {
            await new AllAppSetup(appFactory, clock).Run();
            await new DefaultAppSetup
            (
                appFactory,
                clock,
                apiFactory.CreateTemplate(),
                "Session Log"
            ).Run();
        }
    }
}
