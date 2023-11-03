using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace ArticleProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IToastNotification toastNotification;
        private readonly IMapper mapper;
        private readonly IValidator<Category> validator;

        public CategoryController(ICategoryService categoryService, IToastNotification toastNotification, IMapper mapper, IValidator<Category> validator)
        {
            this.categoryService = categoryService;
            this.toastNotification = toastNotification;
            this.mapper = mapper;
            this.validator = validator;
        }
        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var result = await categoryService.GetAllCategoriesForApprove();
            return View(result);
        }
        public async Task<IActionResult> ActiveCategory(Guid CategoryId)
        {
            await categoryService.ActiveCategory(CategoryId);
            toastNotification.AddSuccessToastMessage("Kategori Onaylanmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("CategoryList", "Category");
        }
        public async Task<IActionResult> PassiveCategory(Guid CategoryId)
        {
            await categoryService.PassiveCategory(CategoryId);
            toastNotification.AddInfoToastMessage("Kategori Pasife Alınmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("CategoryList", "Category");
        }
        public async Task<IActionResult> DeleteCategory(Guid CategoryId)
        {
            await categoryService.DeleteCategory(CategoryId);
            toastNotification.AddErrorToastMessage("Kategori Silinmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("CategoryList", "Category");
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await categoryService.GetCategoryByGuid(categoryId);
            var map = mapper.Map<Category, CategoryUpdateDto>(category);
            return View(map);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            var map = mapper.Map<Category>(categoryUpdateDto);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var name = await categoryService.UpdateCategoryAsync(categoryUpdateDto);
                toastNotification.AddSuccessToastMessage(name + " Başlıklı kategori güncellenmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("CategoryList", "Category");
            }
            result.AddToModelState(this.ModelState);
            return View();
        }
    }
}
