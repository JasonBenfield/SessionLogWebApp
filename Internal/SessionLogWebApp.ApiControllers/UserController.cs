// Generated Code
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using XTI_WebApp.Api;
using SessionLogWebApp.Api;
using XTI_App;
using XTI_App.Api;

namespace SessionLogWebApp.ApiControllers
{
    [Authorize]
    public class UserController : Controller
    {
        public UserController(SessionLogAppApi api)
        {
            this.api = api;
        }

        private readonly SessionLogAppApi api;
        public async Task<IActionResult> Index(UserStartRequest model)
        {
            var result = await api.Group("User").Action<UserStartRequest, AppActionViewResult>("Index").Execute(model);
            return View(result.Data.ViewName);
        }
    }
}