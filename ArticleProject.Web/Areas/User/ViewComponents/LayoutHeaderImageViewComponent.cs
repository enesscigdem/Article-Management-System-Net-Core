using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;

namespace ArticleProject.Web.Areas.User.ViewComponents
{
    public class LayoutHeaderImageViewComponent : ViewComponent
    {
        private readonly IUserService userService;
        public LayoutHeaderImageViewComponent(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await userService.GetUserProfileAsync();
            return View(user);
        }
    }
}
