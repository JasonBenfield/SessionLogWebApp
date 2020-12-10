using MainDB.Entities;

namespace XTI_PermanentLog
{
    public sealed class SessionFactory
    {
        private readonly IMainDataRepositoryFactory repos;

        public SessionFactory(IMainDataRepositoryFactory repos)
        {
            this.repos = repos;
        }

        private AppSessionRepository sessions;
        public AppSessionRepository Sessions()
            => sessions ?? (sessions = new AppSessionRepository(this, repos.CreateSessions()));
        internal AppSession Session(AppSessionRecord record) =>
            new AppSession(this, repos.CreateSessions(), record);

        private AppRequestRepository requests;
        public AppRequestRepository Requests()
            => requests ?? (requests = new AppRequestRepository(this, repos.CreateRequests()));
        internal AppRequest Request(AppRequestRecord record) =>
            new AppRequest(this, repos.CreateRequests(), record);

        private AppEventRepository events;
        public AppEventRepository Events()
            => events ?? (events = new AppEventRepository(this, repos.CreateEvents()));
        internal AppEvent Event(AppEventRecord record) => new AppEvent(record);

    }
}
