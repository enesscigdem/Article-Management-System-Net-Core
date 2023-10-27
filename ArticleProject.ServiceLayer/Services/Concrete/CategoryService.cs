using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.DTOs.Users;
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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task DeleteCategory(Guid CategoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(CategoryId);

            await unitOfWork.GetRepository<Category>().DeleteAsync(category);
            await unitOfWork.SaveAsync();
        }
        public async Task ActiveCategory(Guid CategoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(CategoryId);
            category.IsActive = true;
            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
        }
        public async Task PassiveCategory(Guid CategoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(CategoryId);
            category.IsActive = false;
            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<CategoryListDto>> GetAllCategoriesForApprove()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync();  
            var map = mapper.Map<List<CategoryListDto>>(categories);

            return map;
        }
        public async Task<Category> GetCategoryByGuid(Guid id)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByGuidAsync(id);
            return category;
        }
        public async Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryListDto)
        {
            var category = await unitOfWork.GetRepository<Category>().GetAsync(x=>x.CategoryId == categoryListDto.CategoryId);
            category.CategoryName = categoryListDto.CategoryName;

            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();

            return category.CategoryName;
        }
    }
}
