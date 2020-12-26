using XTI_App;
using XTI_App.Api;
using XTI_TempLog;
using XTI_WebApp.Api;

namespace PermanentLogGroupApi
{
    public sealed class PermanentLogGroup : AppApiGroup
    {
        public PermanentLogGroup(AppApi api, IAppApiUser user, PermanentLog permanentLog)
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
            LogBatch = actions.AddAction(nameof(LogBatch), () => new LogBatchAction(permanentLog));
            StartSession = actions.AddAction(nameof(StartSession), () => new StartSessionAction(permanentLog));
            StartRequest = actions.AddAction(nameof(StartRequest), () => new StartRequestAction(permanentLog));
            EndRequest = actions.AddAction(nameof(EndRequest), () => new EndRequestAction(permanentLog));
            EndSession = actions.AddAction(nameof(EndSession), () => new EndSessionAction(permanentLog));
            LogEvent = actions.AddAction(nameof(LogEvent), () => new LogEventAction(permanentLog));
            AuthenticateSession = actions.AddAction(nameof(AuthenticateSession), () => new AuthenticateSessionAction(permanentLog));
        }

        public AppApiAction<LogBatchModel, EmptyActionResult> LogBatch { get; }
        public AppApiAction<StartSessionModel, EmptyActionResult> StartSession { get; }
        public AppApiAction<StartRequestModel, EmptyActionResult> StartRequest { get; }
        public AppApiAction<EndRequestModel, EmptyActionResult> EndRequest { get; }
        public AppApiAction<EndSessionModel, EmptyActionResult> EndSession { get; }
        public AppApiAction<LogEventModel, EmptyActionResult> LogEvent { get; }
        public AppApiAction<AuthenticateSessionModel, EmptyActionResult> AuthenticateSession { get; }
    }
}
