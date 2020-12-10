using PermanentLogGroupApi;
using XTI_App;
using XTI_App.Api;
using XTI_PermanentLog;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogAppApi : AppApi
    {
        public SessionLogAppApi(AppKey appKey, AppVersionKey versionKey, IAppApiUser user, PermanentLog permanentLog)
            : base
            (
                appKey,
                versionKey,
                user,
                ResourceAccess.AllowAuthenticated()
            )
        {
            PermanentLog = AddGroup(u => new PermanentLogGroup(this, u, permanentLog));
        }

        public PermanentLogGroup PermanentLog { get; }
    }
}
