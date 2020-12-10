using XTI_TempLog.Abstractions;

namespace SessionLogWebApp.Client
{
    partial class EndRequestModel : IEndRequestModel
    {
        public EndRequestModel(IEndRequestModel source)
        {
            RequestKey = source.RequestKey;
            TimeEnded = source.TimeEnded;
        }
    }
}
