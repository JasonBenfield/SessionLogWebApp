// Generated Code
using System;
using System.Collections.Generic;

namespace SessionLogWebApp.Client
{
    public sealed partial class LogBatchModel
    {
        public StartSessionModel[] StartSessions
        {
            get;
            set;
        }

        public StartRequestModel[] StartRequests
        {
            get;
            set;
        }

        public LogEventModel[] LogEvents
        {
            get;
            set;
        }

        public EndRequestModel[] EndRequests
        {
            get;
            set;
        }

        public AuthenticateSessionModel[] AuthenticateSessions
        {
            get;
            set;
        }

        public EndSessionModel[] EndSessions
        {
            get;
            set;
        }
    }
}