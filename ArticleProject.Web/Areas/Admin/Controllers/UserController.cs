using ArticleProject.ServiceLayer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ArticleProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IToastNotification toastNotification;

        public UserController(IUserService userService, IToastNotification toastNotification)
        {
            this.userService = userService;
            this.toastNotification = toastNotification;
        }
        public async Task<IActionResult> ApproveUser()
        {
            var result = await userService.GetAllUsersForApprove();
            return View(result);
        }
    }
}
