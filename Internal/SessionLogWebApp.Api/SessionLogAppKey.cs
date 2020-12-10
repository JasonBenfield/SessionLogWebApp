using XTI_App;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogAppKey
    {
        public static readonly AppKey AppKey = new AppKey(new AppName("SessionLog"), AppType.Values.WebApp);
    }
}
