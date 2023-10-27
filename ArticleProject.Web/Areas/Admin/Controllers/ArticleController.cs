using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using ArticleProject.ServiceLayer.Services.Concrete;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.ComponentModel.DataAnnotations;

namespace ArticleProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;

        private readonly IToastNotification toastNotification;
        public ArticleController(IArticleService article, IToastNotification toastNotification)
        {
            this.articleService = article;
            this.toastNotification = toastNotification;
        }
        [HttpGet]
        public async Task<IActionResult> ArticleList()
        {
            var result = await articleService.GetAllArticlesForApprove();
            return View(result);
        }
        public async Task<IActionResult> ConfirmArticle(Guid CategoryId)
        {
            await articleService.ActiveArticle(CategoryId);
            toastNotification.AddSuccessToastMessage("Makale Onaylanmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("ArticleList", "Article");
        }
        public async Task<IActionResult> PassiveArticle(Guid CategoryId)
        {
            await articleService.PassiveArticle(CategoryId);
            toastNotification.AddInfoToastMessage("Makale Pasife Alınmıştır.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("ArticleList", "Article");
        }
        public async Task<IActionResult> DeleteArticle(Guid CategoryId)
        {
            await articleService.DeleteArticle(CategoryId);
            toastNotification.AddErrorToastMessage("Makale Silinmiştir.", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("ArticleList", "Article");
        }
    }
}
