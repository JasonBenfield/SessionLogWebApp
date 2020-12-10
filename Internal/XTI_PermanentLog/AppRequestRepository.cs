﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainDB.Entities;
using XTI_Core;
using XTI_App;

namespace XTI_PermanentLog
{
    public sealed class AppRequestRepository
    {
        private readonly SessionFactory factory;
        private readonly DataRepository<AppRequestRecord> repo;

        internal AppRequestRepository(SessionFactory factory, DataRepository<AppRequestRecord> repo)
        {
            this.factory = factory;
            this.repo = repo;
        }

        internal async Task<AppRequest> Add(AppSession session, string requestKey, AppVersion version, Resource resource, Modifier modifier, string path, DateTime timeRequested)
        {
            var record = new AppRequestRecord
            {
                SessionID = session.ID.Value,
                RequestKey = requestKey,
                VersionID = version.ID.Value,
                ResourceID = resource.ID.Value,
                ModifierID = modifier.ID.Value,
                Path = path ?? "",
                TimeStarted = timeRequested
            };
            await repo.Create(record);
            return factory.Request(record);
        }

        public async Task<AppRequest> Request(string requestKey)
        {
            var requestRecord = await repo.Retrieve()
                .FirstOrDefaultAsync(r => r.RequestKey == requestKey);
            return factory.Request(requestRecord);
        }

        internal async Task<IEnumerable<AppRequest>> RetrieveBySession(AppSession session)
        {
            var requests = await repo.Retrieve()
                .Where(r => r.SessionID == session.ID.Value)
                .ToArrayAsync();
            return requests.Select(r => factory.Request(r));
        }
    }
}