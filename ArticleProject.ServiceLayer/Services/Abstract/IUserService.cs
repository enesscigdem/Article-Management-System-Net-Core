using ArticleProject.EntityLayer.DTOs.Register;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.Services.Abstract
{
    public interface IUserService
    {
        Task<bool> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<List<UserListDto>> GetAllUsersForApprove();
        Task ActiveUser(Guid UserId);
        Task PassiveUser(Guid UserId);
        Task DeleteUser(Guid UserId);
        Task<User> GetUserByEmail(string email);
        Task<UserProfileDto> GetUserProfileAsync();
    }
}
