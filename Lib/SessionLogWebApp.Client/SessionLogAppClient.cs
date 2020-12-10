// Generated Code
using XTI_WebAppClient;
using System.Net.Http;

namespace SessionLogWebApp.Client
{
    public sealed partial class SessionLogAppClient : AppClient
    {
        public SessionLogAppClient(IHttpClientFactory httpClientFactory, XtiToken xtiToken, string baseUrl, string version = DefaultVersion): base(httpClientFactory, baseUrl, "SessionLog", string.IsNullOrWhiteSpace(version) ? DefaultVersion : version)
        {
            this.xtiToken = xtiToken;
            PermanentLog = new PermanentLogGroup(httpClientFactory, xtiToken, url);
        }

        public const string DefaultVersion = "V66";
        public PermanentLogGroup PermanentLog
        {
            get;
        }
    }
}