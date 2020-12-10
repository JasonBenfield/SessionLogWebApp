using XTI_TempLog.Abstractions;

namespace SessionLogWebApp.Client
{
    partial class EndSessionModel : IEndSessionModel
    {
        public EndSessionModel(IEndSessionModel source)
        {
            SessionKey = source.SessionKey;
            TimeEnded = source.TimeEnded;
        }
    }
}
