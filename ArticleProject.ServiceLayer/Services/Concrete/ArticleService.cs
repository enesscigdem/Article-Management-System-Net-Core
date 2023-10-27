using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.DTOs.Register;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Extensions;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ArticleProject.ServiceLayer.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task ActiveArticle(Guid ArticleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(ArticleId);
            article.IsActive = true;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
        }
        public async Task PassiveArticle(Guid ArticleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(ArticleId);
            article.IsActive = false;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
        }

        public async Task DeleteArticle(Guid ArticleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(ArticleId);

            await unitOfWork.GetRepository<Article>().DeleteAsync(article);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<ArticleListDto>> GetAllArticlesForApprove()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync();
            var map = mapper.Map<List<ArticleListDto>>(articles);

            return map;
        }

        public async Task<List<UserArticlesDto>> GetUserArticles(UserArticlesDto userArticlesDto)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userName = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var getArticles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.AuthorId == userId);
            var map = mapper.Map<List<UserArticlesDto>>(getArticles);

            return map;
        }
        public async Task AddArticleAsync(ArticleAddDto article)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var newArticle = mapper.Map<Article>(article);
            newArticle.AuthorId = userId;
            if (article.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                string userFolder = Path.Combine(uploadsFolder, "articleimage");
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);
                string uniqueFileName = DateTime.Now.Millisecond + "_" + article.Image.FileName;
                string filePath = Path.Combine(userFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await article.Image.CopyToAsync(stream);
                }

                newArticle.Image = uniqueFileName;
            }

            await unitOfWork.GetRepository<Article>().AddAsync(newArticle);
            await unitOfWork.SaveAsync();
        }

        public async Task<Article> GetArticleByGuid(Guid ArticleId)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(ArticleId);
            return article;
        }
        public async Task UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var article = await unitOfWork.GetRepository<Article>().GetAsync(x=>x.ArticleId == articleUpdateDto.ArticleId);

            //if (articleUpdateDto.Image != null)
            //{
            //    if (article.Image != null)
            //        imageHelper.Delete(article.Image.FileName);
            //    var imageUpload = await imageHelper.Upload(articleUpdateDto.Title, articleUpdateDto.Photo, ImageType.Post);
            //    Image image = new(imageUpload.FullName, articleUpdateDto.Photo.ContentType, userEmail);
            //    await unitOfWork.GetRepository<Image>().AddAsync(image);

            //    article.ImageId = image.Id;
            //}

            article.Title = articleUpdateDto.Title;
            article.Content = articleUpdateDto.Content;
            article.CategoryId = articleUpdateDto.CategoryId;
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
        }
    }
}
