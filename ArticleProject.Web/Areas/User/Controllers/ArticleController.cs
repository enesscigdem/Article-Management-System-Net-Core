using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.ServiceLayer.Services.Abstract;
using ArticleProject.ServiceLayer.Services.Concrete;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ArticleProject.Web.Areas.User.Controllers
{
    [Area("User")]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly ICategoryService categoryService;

        private readonly IToastNotification toastNotification;
        public ArticleController(IArticleService article, IToastNotification toastNotification, ICategoryService categoryService)
        {
            this.articleService = article;
            this.toastNotification = toastNotification;
            this.categoryService = categoryService;
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
    }
}
