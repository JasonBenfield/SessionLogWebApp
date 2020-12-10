using System.Threading.Tasks;
using XTI_App.Api;
using XTI_PermanentLog;
using XTI_TempLog;

namespace PermanentLogGroupApi
{
    public sealed class EndSessionAction : AppAction<EndSessionModel, EmptyActionResult>
    {
        private readonly PermanentLog permanentLog;

        public EndSessionAction(PermanentLog permanentLog)
        {
            this.permanentLog = permanentLog;
        }

        public async Task<EmptyActionResult> Execute(EndSessionModel model)
        {
            await permanentLog.EndSession(model);
            return new EmptyActionResult();
        }
    }
}
