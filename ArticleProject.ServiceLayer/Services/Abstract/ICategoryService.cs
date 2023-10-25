using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
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
        Task<Category> GetCategoryByGuid(Guid id);
        Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryListDto);
        Task ActiveCategory(Guid CategoryId);
        Task PassiveCategory(Guid CategoryId);
        Task DeleteCategory(Guid CategoryId);
    }
}
