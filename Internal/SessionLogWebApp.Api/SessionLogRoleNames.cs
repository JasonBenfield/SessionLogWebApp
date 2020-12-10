using XTI_App;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogRoleNames : AppRoleNames
    {
        public static readonly SessionLogRoleNames Instance = new SessionLogRoleNames();

        public SessionLogRoleNames()
        {
            Admin = Add(nameof(Admin));
        }

        public AppRoleName Admin { get; }
    }
}
