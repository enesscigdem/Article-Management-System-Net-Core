using ArticleProject.DataLayer.UnitOfWorks;
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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task DeleteUser(Guid UserId)
        {
            var user = await unitOfWork.GetRepository<User>().GetByGuidAsync(UserId);
            
            await unitOfWork.GetRepository<User>().DeleteAsync(user);
            await unitOfWork.SaveAsync();
        }
        public async Task ConfirmUser(Guid UserId)
        {
            var user = await unitOfWork.GetRepository<User>().GetByGuidAsync(UserId);
            user.IsActive = true;
            await unitOfWork.GetRepository<User>().UpdateAsync(user);
            await unitOfWork.SaveAsync(); 
        }

        public async Task<List<UserListDto>> GetAllUsersForApprove()
        {
            var users = await unitOfWork.GetRepository<User>().GetAllAsync();
            var map = mapper.Map<List<UserListDto>>(users);

            return map;
        }
    }
}
