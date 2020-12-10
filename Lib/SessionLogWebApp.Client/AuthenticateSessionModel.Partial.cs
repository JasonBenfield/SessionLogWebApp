using XTI_TempLog.Abstractions;

namespace SessionLogWebApp.Client
{
    partial class AuthenticateSessionModel : IAuthenticateSessionModel
    {
        public AuthenticateSessionModel(IAuthenticateSessionModel source)
        {
            SessionKey = source.SessionKey;
            UserName = source.UserName;
        }
    }
}
