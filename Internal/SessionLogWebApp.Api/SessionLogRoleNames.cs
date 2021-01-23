using XTI_App;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogRoleNames
    {
        public static readonly SessionLogRoleNames Instance = new SessionLogRoleNames();

        public AppRoleName Admin { get; } = new AppRoleName(nameof(Admin));
    }
}
