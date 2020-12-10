using Microsoft.Extensions.DependencyInjection;
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
            var api = sp.GetService<AppApi>();
            return api.Template();
        }
    }
}
