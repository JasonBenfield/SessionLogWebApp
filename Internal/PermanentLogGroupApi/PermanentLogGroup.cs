using XTI_App;
using XTI_App.Api;
using XTI_TempLog;
using XTI_WebApp.Api;

namespace PermanentLogGroupApi
{
    public sealed class PermanentLogGroup : AppApiGroup
    {
        public PermanentLogGroup(AppApi api, IAppApiUser user, PermanentLogGroupActionFactory actionFactory)
            : base
            (
                api,
                new NameFromGroupClassName(nameof(PermanentLogGroup)).Value,
                ModifierCategoryName.Default,
                ResourceAccess.AllowAuthenticated(),
                user,
                (n, a, u) => new WebAppApiActionCollection(n, a, u)
            )
        {
            var actions = Actions<WebAppApiActionCollection>();
            LogBatch = actions.AddAction(nameof(LogBatch), actionFactory.CreateLogBatch);
            StartSession = actions.AddAction(nameof(StartSession), actionFactory.CreateStartSession);
            StartRequest = actions.AddAction(nameof(StartRequest), actionFactory.CreateStartRequest);
            EndRequest = actions.AddAction(nameof(EndRequest), actionFactory.CreateEndRequest);
            EndSession = actions.AddAction(nameof(EndSession), actionFactory.CreateEndSession);
            LogEvent = actions.AddAction(nameof(LogEvent), actionFactory.CreateLogEvent);
            AuthenticateSession = actions.AddAction(nameof(AuthenticateSession), actionFactory.CreateAuthenticateSession);
            EndExpiredSessions = actions.AddAction(nameof(EndExpiredSessions), actionFactory.CreateEndExpiredSessions);
        }

        public AppApiAction<LogBatchModel, EmptyActionResult> LogBatch { get; }
        public AppApiAction<StartSessionModel, EmptyActionResult> StartSession { get; }
        public AppApiAction<StartRequestModel, EmptyActionResult> StartRequest { get; }
        public AppApiAction<EndRequestModel, EmptyActionResult> EndRequest { get; }
        public AppApiAction<EndSessionModel, EmptyActionResult> EndSession { get; }
        public AppApiAction<LogEventModel, EmptyActionResult> LogEvent { get; }
        public AppApiAction<AuthenticateSessionModel, EmptyActionResult> AuthenticateSession { get; }
        public AppApiAction<EmptyRequest, EmptyActionResult> EndExpiredSessions { get; }
    }
}
