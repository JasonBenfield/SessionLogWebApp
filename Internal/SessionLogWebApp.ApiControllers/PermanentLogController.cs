// Generated Code
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XTI_TempLog;
using XTI_App.Api;
using SessionLogWebApp.Api;
using XTI_App;
using XTI_WebApp.Api;

namespace SessionLogWebApp.ApiControllers
{
    [Authorize]
    public class PermanentLogController : Controller
    {
        public PermanentLogController(SessionLogAppApi api)
        {
            this.api = api;
        }

        private readonly SessionLogAppApi api;
        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> LogBatch([FromBody] LogBatchModel model)
        {
            return api.Group("PermanentLog").Action<LogBatchModel, EmptyActionResult>("LogBatch").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> StartSession([FromBody] StartSessionModel model)
        {
            return api.Group("PermanentLog").Action<StartSessionModel, EmptyActionResult>("StartSession").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> StartRequest([FromBody] StartRequestModel model)
        {
            return api.Group("PermanentLog").Action<StartRequestModel, EmptyActionResult>("StartRequest").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> EndRequest([FromBody] EndRequestModel model)
        {
            return api.Group("PermanentLog").Action<EndRequestModel, EmptyActionResult>("EndRequest").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> EndSession([FromBody] EndSessionModel model)
        {
            return api.Group("PermanentLog").Action<EndSessionModel, EmptyActionResult>("EndSession").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> LogEvent([FromBody] LogEventModel model)
        {
            return api.Group("PermanentLog").Action<LogEventModel, EmptyActionResult>("LogEvent").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> AuthenticateSession([FromBody] AuthenticateSessionModel model)
        {
            return api.Group("PermanentLog").Action<AuthenticateSessionModel, EmptyActionResult>("AuthenticateSession").Execute(model);
        }

        [HttpPost]
        public Task<ResultContainer<EmptyActionResult>> EndExpiredSessions()
        {
            return api.Group("PermanentLog").Action<EmptyRequest, EmptyActionResult>("EndExpiredSessions").Execute(new EmptyRequest());
        }
    }
}