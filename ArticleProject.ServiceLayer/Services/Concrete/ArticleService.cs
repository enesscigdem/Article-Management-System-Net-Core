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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        public async Task<List<ArticleListAllDto>> Get10ActiveArticles()
        {
            var articles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.IsActive == true, y => y.Categories, z => z.Author, c => c.Comments);
            var map = mapper.Map<List<ArticleListAllDto>>(articles);
            return map.OrderByDescending(a=>a.CreationDate).Take(10).ToList();
        }
        public async Task<List<UserArticlesDto>> GetUserArticles(UserArticlesDto userArticlesDto)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userName = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

            var getArticles = await unitOfWork.GetRepository<Article>().GetAllAsync(x => x.AuthorId == userId);
            var map = mapper.Map<List<UserArticlesDto>>(getArticles);

            return map;
        }
        private async Task<List<Category>> GetSelectedCategories(List<Guid> categoryIds)
        {
            var categories = new List<Category>();
            foreach (var categoryId in categoryIds)
            {
                var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(categoryId);
                if (category != null)
                {
                    categories.Add(category);
                }
            }
            return categories;
        }
        public async Task AddArticleAsync(ArticleAddDto article)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var newArticle = mapper.Map<Article>(article);
            newArticle.AuthorId = userId;
            newArticle.Categories = await GetSelectedCategories(article.CategoryIds);
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
        public async Task<Article> GetArticleDetailsByGuid(Guid ArticleId)
        {
            var article = await unitOfWork.GetRepository<Article>()
         .GetAsync(x => x.ArticleId == ArticleId, y => y.Author, z => z.Categories);

            article.Comments = await unitOfWork.GetRepository<Comment>()
                .GetAllAsync(c => c.ArticleId == ArticleId, c => c.User);

            return article;
        }
        public async Task UpdateArticleAsync(ArticleUpdateDto articleUpdateDto)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var article = await unitOfWork.GetRepository<Article>().Include(a => a.Categories).FirstOrDefaultAsync(a => a.ArticleId == articleUpdateDto.ArticleId);


            if (articleUpdateDto.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                string userFolder = Path.Combine(uploadsFolder, "articleimage");
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);

                // Resim byte dizisine dönüştürme
                byte[] imageBytes = Convert.FromBase64String(articleUpdateDto.Image);

                // Resmi kaydetme
                string uniqueFileName = DateTime.Now.Millisecond + "_" + Guid.NewGuid() + ".jpg"; // Resim dosyasının adını benzersiz hale getir
                string filePath = Path.Combine(userFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }

                article.Image = uniqueFileName;
            }

            article.Title = articleUpdateDto.Title;
            article.Content = articleUpdateDto.Content;
            // Yeni seçilen kategorileri al
            var selectedCategories = await GetSelectedCategories(articleUpdateDto.CategoryIds);

            // Önceki kategorileri makaleden kaldır
            foreach (var category in article.Categories.ToList())
            {
                if (!selectedCategories.Contains(category))
                {
                    article.Categories.Remove(category);
                }
            }

            // Eğer yeni bir kategori eklendiyse, makaleye ekle
            foreach (var category in selectedCategories)
            {
                if (!article.Categories.Contains(category))
                {
                    article.Categories.Add(category);
                }
            }

            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
        }
        public async Task<Comment> AddComment(Guid articleId, string content)
        {
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                CreationDate = DateTime.Now,
                ArticleId = article.ArticleId,
            };
            await unitOfWork.GetRepository<Comment>().AddAsync(comment);
            await unitOfWork.SaveAsync();
            return comment;
        }
        public async Task LikeArticle(Guid articleId)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var article = await unitOfWork.GetRepository<Article>().GetByGuidAsync(articleId);

            var existingLike = await unitOfWork.GetRepository<Like>()
                .GetAsync(x => x.UserId == userId && x.ArticleId == articleId);

            if (existingLike == null)
            {
                var like = new Like
                {
                    UserId = userId,
                    ArticleId = articleId
                };
                await unitOfWork.GetRepository<Like>().AddAsync(like);

                article.Likes++;
            }
            else
            {
                await unitOfWork.GetRepository<Like>().DeleteAsync(existingLike);

                article.Likes--;
            }
            await unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await unitOfWork.SaveAsync();
        }
    }
}
