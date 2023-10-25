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
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var result = await userService.GetAllUsersForApprove();
            return View(result);
        }
        public async Task<IActionResult> ActiveUser(Guid UserId)
        {
            await userService.ActiveUser(UserId);
            toastNotification.AddSuccessToastMessage("Kullanıcı Onaylanmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("UserList", "User");
        }
        public async Task<IActionResult> PassiveUser(Guid UserId)
        {
            await userService.PassiveUser(UserId);
            toastNotification.AddInfoToastMessage("Kullanıcı Pasife Alınmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("UserList", "User");
        }
        public async Task<IActionResult> DeleteUser(Guid UserId)
        {
            await userService.DeleteUser(UserId);
            toastNotification.AddErrorToastMessage("Kullanıcı Silinmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("UserList", "User");
        }
    }
}
