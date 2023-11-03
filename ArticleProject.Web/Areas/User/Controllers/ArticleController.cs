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
using System.ComponentModel.DataAnnotations;

namespace ArticleProject.Web.Areas.User.Controllers
{
    [Area("User")]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;
        private readonly IToastNotification toastNotification;
        private readonly IMapper mapper;
        private readonly IValidator<ArticleAddDto> validator;
        private readonly IValidator<ArticleUpdateDto> validatorUpdate;
        public ArticleController(IArticleService article, IToastNotification toastNotification, ICategoryService categoryService, IMapper mapper, IValidator<ArticleAddDto> validator, IValidator<ArticleUpdateDto> validatorUpdate)
        {
            this.articleService = article;
            this.toastNotification = toastNotification;
            this.categoryService = categoryService;
            this.mapper = mapper;
            this.validator = validator;
            this.validatorUpdate = validatorUpdate;
        }
        [HttpGet]
        public async Task<IActionResult> ArticleList(UserArticlesDto userArticlesDto)
        {
            var result = await articleService.GetUserArticles(userArticlesDto);
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> ArticleAdd()
        {
            var categories = await categoryService.GetAllCategoriesForApprove();
            return View(new ArticleAddDto { Categories = categories });
        }

        [HttpPost]
        public async Task<IActionResult> ArticleAdd(ArticleAddDto article)
        {
            var result = await validator.ValidateAsync(article);

            if (result.IsValid)
            {
                await articleService.AddArticleAsync(article);

                toastNotification.AddSuccessToastMessage("Makale başarıyla eklenmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });

                return RedirectToAction("ArticleList", "Article", new { Area = "User" });
            }
            result.AddToModelState(this.ModelState);
            article.Categories = await categoryService.GetAllCategoriesForApprove();
            return View(article);
        }

        public async Task<IActionResult> DeleteArticle(Guid ArticleId)
        {
            await articleService.DeleteArticle(ArticleId);
            toastNotification.AddErrorToastMessage("Makale Silinmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("ArticleList", "Article", new { Area = "User" });
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid ArticleId)
        {
            var article = await articleService.GetArticleByGuid(ArticleId);
            var categories = await categoryService.GetAllCategoriesForApprove();

            var articleUpdateDto = mapper.Map<ArticleUpdateDto>(article);
            articleUpdateDto.Categories = categories;
            return View(articleUpdateDto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateDto articleUpdateDto, IFormFile image)
        {
            var result = await validatorUpdate.ValidateAsync(articleUpdateDto);

            if (result.IsValid)
            {
                if (image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        byte[] imageBytes = ms.ToArray();
                        articleUpdateDto.Image = Convert.ToBase64String(imageBytes);
                    }
                }
                // Kategorileri seçilmiş kategori ID'lerine dönüştür
                if (articleUpdateDto.CategoryIds != null)
                {
                    articleUpdateDto.CategoryIds = articleUpdateDto.CategoryIds ?? new List<Guid>();
                }
                await articleService.UpdateArticleAsync(articleUpdateDto);
                toastNotification.AddSuccessToastMessage("Makale başarıyla güncellenmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("ArticleList", "Article", new { Area = "User" });
            }
            result.AddToModelState(this.ModelState);
            articleUpdateDto.Categories = await categoryService.GetAllCategoriesForApprove();
            return View(articleUpdateDto);
        }
    }
}
