using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.Services.Abstract
{
    public interface IArticleService
    {
        Task<List<ArticleListDto>> GetAllArticlesForApprove();
        Task<List<UserArticlesDto>> GetUserArticles(UserArticlesDto userArticlesDto);
        Task<Article> GetArticleByGuid(Guid ArticleId);
        Task ActiveArticle(Guid ArticleId);
        Task PassiveArticle(Guid ArticleId);
        Task DeleteArticle(Guid ArticleId);
        Task AddArticleAsync(ArticleAddDto article);
        Task UpdateArticleAsync(ArticleUpdateDto articleUpdateDto);
    }
}
