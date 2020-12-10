using XTI_App;
using XTI_App.Api;
using XTI_PermanentLog;
using XTI_TempLog;

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
                (n, a, u) => new AppApiActionCollection(n, a, u)
            )
        {
            var actions = Actions<AppApiActionCollection>();
            LogBatch = actions.Add(nameof(LogBatch), () => new LogBatchAction(permanentLog));
            StartSession = actions.Add(nameof(StartSession), () => new StartSessionAction(permanentLog));
            StartRequest = actions.Add(nameof(StartRequest), () => new StartRequestAction(permanentLog));
            EndRequest = actions.Add(nameof(EndRequest), () => new EndRequestAction(permanentLog));
            EndSession = actions.Add(nameof(EndSession), () => new EndSessionAction(permanentLog));
            LogEvent = actions.Add(nameof(LogEvent), () => new LogEventAction(permanentLog));
            AuthenticateSession = actions.Add(nameof(AuthenticateSession), () => new AuthenticateSessionAction(permanentLog));
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
