using System.Threading.Tasks;
using XTI_App;
using XTI_Core;
using XTI_TempLog;
using XTI_TempLog.Abstractions;

namespace XTI_PermanentLog
{
    public sealed class PermanentLog
    {
        private readonly SessionFactory sessionFactory;
        private readonly AppFactory appFactory;
        private readonly Clock clock;

        public PermanentLog(SessionFactory sessionFactory, AppFactory appFactory, Clock clock)
        {
            this.sessionFactory = sessionFactory;
            this.appFactory = appFactory;
            this.clock = clock;
        }

        public async Task LogBatch(ILogBatchModel model)
        {
            foreach (var startSession in model.StartSessions)
            {
                await StartSession(startSession);
            }
            foreach (var authSession in model.AuthenticateSessions)
            {
                await AuthenticateSession(authSession);
            }
            foreach (var startRequest in model.StartRequests)
            {
                await StartRequest(startRequest);
            }
            foreach (var logEvent in model.LogEvents)
            {
                await LogEvent(logEvent);
            }
            foreach (var endRequest in model.EndRequests)
            {
                await EndRequest(endRequest);
            }
            foreach (var endSession in model.EndSessions)
            {
                await EndSession(endSession);
            }
        }

        public async Task StartSession(IStartSessionModel startSession)
        {
            var user = await retrieveUser(startSession.UserName);
            var session = await sessionFactory.Sessions().Session(startSession.SessionKey);
            if (session.ID.IsValid())
            {
                await session.Edit
                (
                    user,
                    startSession.TimeStarted,
                    startSession.RequesterKey,
                    startSession.UserAgent,
                    startSession.RemoteAddress
                );
            }
            else
            {
                await sessionFactory.Sessions().Create
                (
                    startSession.SessionKey,
                    user,
                    startSession.TimeStarted,
                    startSession.RequesterKey,
                    startSession.UserAgent,
                    startSession.RemoteAddress
                );
            }
        }

        public async Task AuthenticateSession(IAuthenticateSessionModel model)
        {
            var session = await sessionFactory.Sessions().Session(model.SessionKey);
            if (session.ID.IsNotValid())
            {
                session = await startPlaceholderSession(model.SessionKey, new GeneratedKey().Value());
            }
            var user = await retrieveUser(model.UserName);
            await session.Authenticate(user);
        }

        public async Task StartRequest(IStartRequestModel startRequest)
        {
            var session = await sessionFactory.Sessions().Session(startRequest.SessionKey);
            if (session.ID.IsNotValid())
            {
                session = await startPlaceholderSession(startRequest.SessionKey, new GeneratedKey().Value());
            }
            var path = XtiPath.Parse(startRequest.Path);
            if (string.IsNullOrWhiteSpace(path.Group))
            {
                path = path.WithGroup("Home");
            }
            if (string.IsNullOrWhiteSpace(path.Action))
            {
                path = path.WithAction("Index");
            }
            var appKey = new AppKey(path.App, AppType.Values.Value(startRequest.AppType));
            var app = await appFactory.Apps().App(appKey);
            var version = await app.Version(path.Version);
            var resourceGroup = await app.ResourceGroup(path.Group);
            var resource = await resourceGroup.Resource(path.Action);
            var modCategory = await resourceGroup.ModCategory();
            var modifier = await modCategory.Modifier(path.Modifier);
            var request = await sessionFactory.Requests().Request(startRequest.RequestKey);
            if (request.ID.IsValid())
            {
                await request.Edit
                (
                    session,
                    version,
                    resource,
                    modifier,
                    startRequest.Path,
                    startRequest.TimeStarted
                );
            }
            else
            {
                await session.LogRequest
                (
                    startRequest.RequestKey,
                    version,
                    resource,
                    modifier,
                    startRequest.Path,
                    startRequest.TimeStarted
                );
            }
        }

        public async Task LogEvent(ILogEventModel model)
        {
            var request = await sessionFactory.Requests().Request(model.RequestKey);
            if (request.ID.IsNotValid())
            {
                request = await startPlaceholderRequest(model.RequestKey);
            }
            var severity = AppEventSeverity.Values.Value(model.Severity);
            await request.LogEvent
            (
                model.EventKey,
                severity,
                model.TimeOccurred,
                model.Caption,
                model.Message,
                model.Detail
            );
        }

        public async Task EndRequest(IEndRequestModel model)
        {
            var request = await sessionFactory.Requests().Request(model.RequestKey);
            if (request.ID.IsNotValid())
            {
                request = await startPlaceholderRequest(model.RequestKey);
            }
            await request.End(model.TimeEnded);
        }

        public async Task EndSession(IEndSessionModel model)
        {
            var session = await sessionFactory.Sessions().Session(model.SessionKey);
            if (session.ID.IsNotValid())
            {
                session = await startPlaceholderSession(model.SessionKey, new GeneratedKey().Value());
            }
            await session.End(model.TimeEnded);
        }

        private async Task<AppRequest> startPlaceholderRequest(string requestKey)
        {
            var session = await defaultSession();
            var app = await appFactory.Apps().App(AppKey.Unknown);
            var version = await app.CurrentVersion();
            var resourceGroup = await app.ResourceGroup(ResourceGroupName.Unknown);
            var resource = await resourceGroup.Resource(ResourceName.Unknown);
            var modCategory = await app.ModCategory(ModifierCategoryName.Default);
            var modifier = await modCategory.Modifier(ModifierKey.Default);
            var request = await session.LogRequest
            (
                string.IsNullOrWhiteSpace(requestKey) ? new GeneratedKey().Value() : requestKey,
                version,
                resource,
                modifier,
                "",
                clock.Now()
            );
            return request;
        }

        private async Task<AppUser> retrieveUser(string userName)
        {
            var user = await appFactory.Users().User(new AppUserName(userName));
            if (!user.Exists())
            {
                user = await appFactory.Users().User(AppUserName.Anon);
            }
            return user;
        }

        private async Task<AppSession> defaultSession()
        {
            var session = await sessionFactory.Sessions().DefaultSession(clock.Now());
            if (session.ID.IsNotValid() || session.HasEnded())
            {
                session = await startPlaceholderSession(new GeneratedKey().Value(), "default");
            }
            return session;
        }

        private async Task<AppSession> startPlaceholderSession(string sessionKey, string requesterKey)
        {
            var user = await appFactory.Users().User(AppUserName.Anon);
            var session = await sessionFactory.Sessions().Create
            (
                sessionKey,
                user,
                clock.Now(),
                requesterKey,
                "",
                ""
            );
            return session;
        }
    }
}
