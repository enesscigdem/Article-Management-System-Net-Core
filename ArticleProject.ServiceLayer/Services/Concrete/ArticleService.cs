using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Extensions;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
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
            await unitOfWork.GetRepository<Article>().AddAsync(newArticle);
            await unitOfWork.SaveAsync();
        }
    }
}
