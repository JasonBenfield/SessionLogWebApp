using System;
using XTI_App.Api;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogAppApiFactory : AppApiFactory
    {
        private readonly IServiceProvider sp;

        public SessionLogAppApiFactory(IServiceProvider sp)
        {
            this.sp = sp;
        }

        protected override AppApi _Create(IAppApiUser user)=> new SessionLogAppApi(user, sp);
    }
}
