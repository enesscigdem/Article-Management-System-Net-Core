using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using ArticleProject.Web.Models;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;
using System.Security.Claims;

namespace ArticleProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService articleService;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;
        public HomeController(IArticleService article, IMapper mapper, IToastNotification toastNotification)
        {
            this.articleService = article;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await articleService.Get10ActiveArticles();
            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> ArticleDetail(Guid ArticleId)
        {
            var result = await articleService.GetArticleDetailsByGuid(ArticleId);
            return View(result);
        }
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(Guid articleId, string content)
        {
            var comment = await articleService.AddComment(articleId,content);
            if (comment == null)
            {
                toastNotification.AddWarningToastMessage("Yorum yaparken bir hata oluştu!.", new ToastrOptions { Title = "Hatalı İşlem" });
            }
            toastNotification.AddInfoToastMessage("Yorum başarıyla yapılmıştır", new ToastrOptions { Title = "İşlem Başarılı" });
            return RedirectToAction("ArticleDetail", "Home", new { ArticleId = articleId, Area = "" });
        }
        public async Task<IActionResult> LikeArticle(Guid articleId)
        {
            await articleService.LikeArticle(articleId);
            return RedirectToAction("ArticleDetail", "Home", new { ArticleId = articleId, Area=""});
        }
    }
}