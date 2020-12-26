using Microsoft.Extensions.DependencyInjection;
using PermanentLogGroupApi;
using System;
using XTI_App.Api;

namespace SessionLogWebApp.Api
{
    public sealed class SessionLogAppApiTemplateFactory : IAppApiTemplateFactory
    {
        private readonly IServiceProvider sp;

        public SessionLogAppApiTemplateFactory(IServiceProvider sp)
        {
            this.sp = sp;
        }

        public AppApiTemplate Create()
        {
            var permanentLog = sp.GetService<PermanentLog>();
            var api = new SessionLogAppApi(new AppApiSuperUser(), permanentLog);
            return api.Template();
        }
    }
}
