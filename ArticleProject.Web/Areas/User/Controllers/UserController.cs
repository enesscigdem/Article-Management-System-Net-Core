using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using ArticleProject.ServiceLayer.Services.Concrete;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
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
        private readonly IValidator<ArticleProject.EntityLayer.Entities.User> validator;
        public UserController(IToastNotification toastNotification, IUserService userService, IMapper mapper, IValidator<ArticleProject.EntityLayer.Entities.User> validator)
        {
            this.toastNotification = toastNotification;
            this.userService = userService;
            this.mapper = mapper;
            this.validator = validator;
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
            try
            {
                var map = mapper.Map<ArticleProject.EntityLayer.Entities.User>(userProfileDto);
                var result = await validator.ValidateAsync(map);
                if (result.IsValid)
                {
                    if (profilePicture != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            profilePicture.CopyTo(ms);
                            byte[] imageBytes = ms.ToArray();
                            userProfileDto.ProfilePicture = Convert.ToBase64String(imageBytes);
                        }
                    }

                    await userService.UpdateUserProfileAsync(userProfileDto);

                    return RedirectToAction("Profile", "User", new { Area = "User" });

                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("NewPassword", ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("NewPassword", "An error occurred while updating the user profile.");
            }

            var profile = await userService.GetUserProfileAsync();

            return View(profile);
        }
       
    }
}
