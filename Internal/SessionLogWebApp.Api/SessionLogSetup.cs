using System.Threading.Tasks;
using XTI_App;
using XTI_Core;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogSetup : IAppSetup
    {
        private readonly AppFactory appFactory;
        private readonly Clock clock;

        public SessionLogSetup(AppFactory appFactory, Clock clock)
        {
            this.appFactory = appFactory;
            this.clock = clock;
        }

        public async Task Run()
        {
            var app = await appFactory.Apps().App(SessionLogAppKey.AppKey);
            const string title = "Session Log";
            if (app.Key().Equals(SessionLogAppKey.AppKey))
            {
                await app.SetTitle(title);
            }
            else
            {
                app = await appFactory.Apps().Add(SessionLogAppKey.AppKey, title, clock.Now());
            }
            var currentVersion = await app.CurrentVersion();
            if (!currentVersion.IsCurrent())
            {
                currentVersion = await app.StartNewMajorVersion(clock.Now());
                await currentVersion.Publishing();
                await currentVersion.Published();
            }
            await app.SetRoles(SessionLogRoleNames.Instance.Values());
        }
    }
}
