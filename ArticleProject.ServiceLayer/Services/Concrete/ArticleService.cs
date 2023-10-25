using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.Services.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ArticleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
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
    }
}
