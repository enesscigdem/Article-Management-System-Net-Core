using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.Services.Abstract
{
    public interface ICategoryService
    {
        Task<List<CategoryListDto>> GetAllCategoriesForApprove();
        Task ActiveCategory(Guid CategoryId);
        Task PassiveCategory(Guid CategoryId);
        Task DeleteCategory(Guid CategoryId);
    }
}
