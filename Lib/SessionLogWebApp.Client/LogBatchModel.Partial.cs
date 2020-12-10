using System.Linq;
using XTI_TempLog.Abstractions;

namespace SessionLogWebApp.Client
{
    partial class LogBatchModel : ILogBatchModel
    {
        public LogBatchModel(ILogBatchModel source)
        {
            StartSessions = source.StartSessions?.Cast<StartSessionModel>().ToArray()
                ?? new StartSessionModel[] { };
            AuthenticateSessions = source.AuthenticateSessions?.Cast<AuthenticateSessionModel>().ToArray()
                ?? new AuthenticateSessionModel[] { };
            StartRequests = source.StartRequests?.Cast<StartRequestModel>().ToArray()
                ?? new StartRequestModel[] { };
            LogEvents = source.LogEvents?.Cast<LogEventModel>().ToArray()
                ?? new LogEventModel[] { };
            EndRequests = source.EndRequests?.Cast<EndRequestModel>().ToArray()
                ?? new EndRequestModel[] { };
            EndSessions = source.EndSessions?.Cast<EndSessionModel>().ToArray()
                ?? new EndSessionModel[] { };
        }

        IAuthenticateSessionModel[] ILogBatchModel.AuthenticateSessions
        {
            get => AuthenticateSessions;
            set => AuthenticateSessions = value.Cast<AuthenticateSessionModel>().ToArray();
        }
        IEndRequestModel[] ILogBatchModel.EndRequests
        {
            get => EndRequests;
            set => EndRequests = value.Cast<EndRequestModel>().ToArray();
        }
        ILogEventModel[] ILogBatchModel.LogEvents
        {
            get => LogEvents;
            set => LogEvents = value.Cast<LogEventModel>().ToArray();
        }
        IStartRequestModel[] ILogBatchModel.StartRequests
        {
            get => StartRequests;
            set => StartRequests = value.Cast<StartRequestModel>().ToArray();
        }
        IStartSessionModel[] ILogBatchModel.StartSessions
        {
            get => StartSessions;
            set => StartSessions = value.Cast<StartSessionModel>().ToArray();
        }
        IEndSessionModel[] ILogBatchModel.EndSessions
        {
            get => EndSessions;
            set => EndSessions = value.Cast<EndSessionModel>().ToArray();
        }
    }
}
