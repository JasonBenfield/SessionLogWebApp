﻿using XTI_TempLog.Abstractions;

namespace SessionLogWebApp.Client
{
    partial class StartSessionModel : IStartSessionModel
    {
        public StartSessionModel(IStartSessionModel source)
        {
            SessionKey = source.SessionKey;
            UserName = source.UserName;
            RequesterKey = source.RequesterKey;
            TimeStarted = source.TimeStarted;
            RemoteAddress = source.RemoteAddress;
            UserAgent = source.UserAgent;
        }
    }
}
