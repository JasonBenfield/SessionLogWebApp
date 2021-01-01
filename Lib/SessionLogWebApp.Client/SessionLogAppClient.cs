// Generated Code
using XTI_WebAppClient;
using System.Net.Http;

namespace SessionLogWebApp.Client
{
    public sealed partial class SessionLogAppClient : AppClient
    {
        public SessionLogAppClient(IHttpClientFactory httpClientFactory, IXtiToken xtiToken, string baseUrl, string version = DefaultVersion): base(httpClientFactory, baseUrl, "SessionLog", string.IsNullOrWhiteSpace(version) ? DefaultVersion : version)
        {
            this.xtiToken = xtiToken;
            User = new UserGroup(httpClientFactory, xtiToken, url);
            PermanentLog = new PermanentLogGroup(httpClientFactory, xtiToken, url);
        }

        public const string DefaultVersion = "V1124";
        public UserGroup User
        {
            get;
        }

        public PermanentLogGroup PermanentLog
        {
            get;
        }
    }
}