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
        public async Task<IActionResult> ConfirmUser(Guid UserId)
        {
            await userService.ConfirmUser(UserId);
            toastNotification.AddSuccessToastMessage("Kullanıcı Onaylanmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("ApproveUser", "User");
        }
        public async Task<IActionResult> DeleteUser(Guid UserId)
        {
            await userService.DeleteUser(UserId);
            toastNotification.AddErrorToastMessage("Kullanıcı Silinmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("ApproveUser", "User");
        }
    }
}
