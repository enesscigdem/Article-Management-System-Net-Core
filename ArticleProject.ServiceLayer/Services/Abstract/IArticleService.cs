using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Categories;
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
        Task ActiveArticle(Guid ArticleId);
        Task PassiveArticle(Guid ArticleId);
        Task DeleteArticle(Guid ArticleId);
        Task AddArticleAsync(ArticleAddDto article);
    }
}
