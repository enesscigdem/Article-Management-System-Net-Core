using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Categories;
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
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IToastNotification toastNotification;
        private readonly IMapper mapper;
        private readonly IValidator<Category> validator;
        public CategoryController(IToastNotification toastNotification, ICategoryService categoryService, IUserService userService, IMapper mapper, IValidator<Category> validator)
        {
            this.toastNotification = toastNotification;
            this.categoryService = categoryService;
            this.userService = userService;
            this.mapper = mapper;
            this.validator = validator;
        }
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var result = await categoryService.GetActiveCategories();
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> FollowCategoryList()
        {
            var result = await categoryService.GetCategoriesFollowedByUser();
            return View(result);
        }
        public async Task<IActionResult> FollowCategory(Guid CategoryId)
        {
            var result = await categoryService.UsersFollowCategory(CategoryId);

            if (result.Contains("zaten takip ediliyor"))
            {
                toastNotification.AddWarningToastMessage(result, new ToastrOptions { Title = "Uyarı" });
            }
            else
            {
                toastNotification.AddSuccessToastMessage(result, new ToastrOptions { Title = "İşlem Başarılı" });
            }

            return RedirectToAction("FollowCategoryList", "Category", new { Area = "User" });
        }

        public async Task<IActionResult> UnfollowCategory(Guid CategoryId)
        {
            var result = await categoryService.UsersUnfollowFollowCategory(CategoryId);
            toastNotification.AddInfoToastMessage(result + " adlı kategorinin takibi bırakılmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("FollowCategoryList", "Category", new { Area = "User" });
        }
        [HttpGet]
        public IActionResult CategoryAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CategoryAdd(CategoryListDto categoryListDto)
        {
            var map = mapper.Map<Category>(categoryListDto);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await categoryService.UsersAddCategory(categoryListDto);

                toastNotification.AddSuccessToastMessage("Kategori onaya gönderilmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });

                return RedirectToAction("CategoryList", "Category", new { Area = "User" });
            }
            result.AddToModelState(this.ModelState);
            return View();
        }
    }
}
