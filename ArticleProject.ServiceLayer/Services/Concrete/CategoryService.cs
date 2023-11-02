using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
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
        public async Task<List<CategoryListDto>> GetActiveCategories()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync(x => x.IsActive == true);
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
            var category = await unitOfWork.GetRepository<Category>().GetAsync(x => x.CategoryId == categoryListDto.CategoryId);
            category.CategoryName = categoryListDto.CategoryName;
            category.Description = categoryListDto.Description;

            await unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await unitOfWork.SaveAsync();

            return category.CategoryName;
        }
        public async Task UsersAddCategory(CategoryListDto categoryListDto)
        {
            var category = mapper.Map<Category>(categoryListDto);
            category.IsActive = false;
            await unitOfWork.GetRepository<Category>().AddAsync(category);
            await unitOfWork.SaveAsync();

        }
        public async Task<string> UsersFollowCategory(Guid categoryId)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var category = await unitOfWork.GetRepository<Category>().GetAsync(x => x.CategoryId == categoryId);

            // Check if the user is already following the category
            var isAlreadyFollowing = await unitOfWork.GetRepository<FollowCategory>()
                .GetAsync(fc => fc.UserId == userId && fc.CategoryId == categoryId);

            if (isAlreadyFollowing != null)
            {
                // User is already following the category, return a message or handle as needed
                return $"{category.CategoryName} adlı kategori zaten takip ediliyor.";
            }

            var follow = new FollowCategory
            {
                UserId = userId,
                CategoryId = categoryId
            };

            await unitOfWork.GetRepository<FollowCategory>().AddAsync(follow);
            await unitOfWork.SaveAsync();

            return $"{category.CategoryName} adlı kategori takip edilmiştir.";
        }

        public async Task<string> UsersUnfollowFollowCategory(Guid categoryId)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var unfCategory = await unitOfWork.GetRepository<FollowCategory>().GetAsync(x => x.CategoryId == categoryId && x.UserId==userId, y=>y.Category);
            await unitOfWork.GetRepository<FollowCategory>().DeleteAsync(unfCategory);
            await unitOfWork.SaveAsync();
            return unfCategory.Category.CategoryName;
        }
        public async Task<List<FollowCategory>> GetCategoriesFollowedByUser()
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await unitOfWork.GetRepository<User>().GetAsync(x => x.IsActive == true && x.UserId==userId, y=>y.FollowedCategories);
            var userFollowCategories = await unitOfWork.GetRepository<FollowCategory>().GetAllAsync(x => x.UserId==user.UserId, y=>y.Category);
           
            return userFollowCategories;

        }
    }
}
