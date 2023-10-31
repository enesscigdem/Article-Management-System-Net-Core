using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using ArticleProject.ServiceLayer.Services.Concrete;
using AutoMapper;
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
        private readonly IUserService userService;
        private readonly IToastNotification toastNotification;
        private readonly IMapper mapper;
        public ArticleController(IArticleService article, IToastNotification toastNotification, ICategoryService categoryService, IUserService userService, IMapper mapper)
        {
            this.articleService = article;
            this.toastNotification = toastNotification;
            this.categoryService = categoryService;
            this.userService = userService;
            this.mapper = mapper;
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
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            await articleService.AddArticleAsync(article);

            toastNotification.AddSuccessToastMessage("Makale başarıyla eklenmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });

            return RedirectToAction("ArticleList", "Article", new { Area = "User" });
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
    }
}
