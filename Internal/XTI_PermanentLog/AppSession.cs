using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MainDB.Entities;
using XTI_App;
using XTI_Core;

namespace XTI_PermanentLog
{
    public sealed class AppSession
    {
        private readonly SessionFactory factory;
        private readonly DataRepository<AppSessionRecord> repo;
        private readonly AppSessionRecord record;

        internal AppSession(SessionFactory factory, DataRepository<AppSessionRecord> repo, AppSessionRecord record)
        {
            this.factory = factory;
            this.repo = repo;
            this.record = record ?? new AppSessionRecord();
            ID = new EntityID(this.record.ID);
        }

        public EntityID ID { get; }
        public int UserID { get => record.UserID; }

        public bool HasStarted() => new Timestamp(record.TimeStarted).IsValid();
        public bool HasEnded() => new Timestamp(record.TimeEnded).IsValid();

        public Task<AppRequest> LogRequest
        (
            string requestKey,
            AppVersion version,
            Resource resource,
            Modifier modifier,
            string path,
            DateTime timeRequested
        )
        {
            var requestRepo = factory.Requests();
            return requestRepo.Add(this, requestKey, version, resource, modifier, path, timeRequested);
        }

        internal Task Edit(AppUser user, DateTime timeStarted, string requesterKey, string userAgent, string remoteAddress)
            => repo.Update
                (
                    record,
                    r =>
                    {
                        r.UserID = user.ID.Value;
                        r.TimeStarted = timeStarted;
                        r.RequesterKey = requesterKey;
                        r.UserAgent = userAgent ?? "";
                        r.RemoteAddress = remoteAddress ?? "";
                    }
                );

        public Task Authenticate(IAppUser user)
        {
            return repo.Update(record, r =>
            {
                r.UserID = user.ID.Value;
            });
        }

        public Task End(DateTime timeEnded)
        {
            return repo.Update(record, r =>
            {
                r.TimeEnded = timeEnded;
            });
        }

        public Task<IEnumerable<AppRequest>> Requests()
        {
            var requestRepo = factory.Requests();
            return requestRepo.RetrieveBySession(this);
        }

        public override string ToString() => $"{nameof(AppSession)} {ID.Value}";
    }
}
