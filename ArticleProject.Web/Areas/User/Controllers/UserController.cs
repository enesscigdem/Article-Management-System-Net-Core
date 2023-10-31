using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using ArticleProject.ServiceLayer.Services.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ArticleProject.Web.Areas.User.Controllers
{
    [Area("User")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IToastNotification toastNotification;
        private readonly IMapper mapper;
        public UserController(IToastNotification toastNotification, IUserService userService, IMapper mapper)
        {
            this.toastNotification = toastNotification;
            this.userService = userService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await userService.GetUserProfileAsync();

            return View(profile);
        }
        public async Task<IActionResult> PassiveAccount()
        {
            await userService.PassiveAccount();
            return RedirectToAction("Login", "Account", new { Area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileDto userProfileDto, IFormFile profilePicture)
        {
            if (ModelState.IsValid)
            {
                if (profilePicture != null)
                {
                    // Profil resmini işleme ve string olarak saklama işlemi
                    using (MemoryStream ms = new MemoryStream())
                    {
                        profilePicture.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        userProfileDto.ProfilePicture = Convert.ToBase64String(imageBytes);
                    }
                }

                // UserProfileDto'yu kullanarak kullanıcı profili güncelleme işlemi
                await userService.UpdateUserProfileAsync(userProfileDto);

                return RedirectToAction("Profile", "User", new { Area = "User" });
            }


            return View(userProfileDto);
        }
       
    }
}
