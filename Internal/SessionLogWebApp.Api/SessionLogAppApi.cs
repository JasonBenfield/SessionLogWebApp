using PermanentLogGroupApi;
using XTI_App.Api;
using XTI_WebApp.Api;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogAppApi : WebAppApi
    {
        public SessionLogAppApi(IAppApiUser user, PermanentLog permanentLog)
            : base
            (
                SessionLogAppKey.AppKey,
                user,
                ResourceAccess.AllowAuthenticated()
            )
        {
            PermanentLog = AddGroup(u => new PermanentLogGroup(this, u, permanentLog));
        }

        public PermanentLogGroup PermanentLog { get; }
    }
}
