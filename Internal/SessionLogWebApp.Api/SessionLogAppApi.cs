using PermanentLogGroupApi;
using System;
using XTI_App.Api;
using XTI_WebApp.Api;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogAppApi : WebAppApiWrapper
    {
        public SessionLogAppApi(IAppApiUser user, IServiceProvider services)
            : base
            (
                new AppApi
                (
                    SessionLogAppKey.AppKey,
                    user,
                    ResourceAccess.AllowAuthenticated()
                        .WithAllowed(SessionLogRoleNames.Instance.Admin)
                )
            )
        {
            PermanentLog = new PermanentLogGroup
            (
                source.AddGroup(nameof(PermanentLog)),
                new PermanentLogGroupActionFactory(services)
            );
        }

        public PermanentLogGroup PermanentLog { get; }
    }
}
