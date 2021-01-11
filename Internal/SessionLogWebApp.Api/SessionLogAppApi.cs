using PermanentLogGroupApi;
using System;
using XTI_App.Api;
using XTI_WebApp.Api;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogAppApi : WebAppApi
    {
        public SessionLogAppApi(IAppApiUser user, IServiceProvider services)
            : base
            (
                SessionLogAppKey.AppKey,
                user,
                ResourceAccess.AllowAuthenticated()
                    .WithAllowed(SessionLogRoleNames.Instance.Admin)
            )
        {
            PermanentLog = AddGroup(u => new PermanentLogGroup(this, u, new PermanentLogGroupActionFactory(services)));
        }

        public PermanentLogGroup PermanentLog { get; }
    }
}
