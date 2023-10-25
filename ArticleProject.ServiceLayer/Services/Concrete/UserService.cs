using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Register;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment webHostEnvironment;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteUser(Guid UserId)
        {
            var user = await unitOfWork.GetRepository<User>().GetByGuidAsync(UserId);

            await unitOfWork.GetRepository<User>().DeleteAsync(user);
            await unitOfWork.SaveAsync();
        }
        public async Task ActiveUser(Guid UserId)
        {
            var user = await unitOfWork.GetRepository<User>().GetByGuidAsync(UserId);
            user.IsActive = true;
            await unitOfWork.GetRepository<User>().UpdateAsync(user);
            await unitOfWork.SaveAsync();
        }
        public async Task PassiveUser(Guid UserId)
        {
            var user = await unitOfWork.GetRepository<User>().GetByGuidAsync(UserId);
            user.IsActive = false;
            await unitOfWork.GetRepository<User>().UpdateAsync(user);
            await unitOfWork.SaveAsync();
        }

        public async Task<List<UserListDto>> GetAllUsersForApprove()
        {
            var users = await unitOfWork.GetRepository<User>().GetAllAsync();
            var map = mapper.Map<List<UserListDto>>(users);

            return map;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await unitOfWork.GetRepository<User>().GetAsync(u => u.Email == email);
            if (user != null && password == user.Password)
                return true;
            return false;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var isUnique = await IsEmailAndUsernameUniqueAsync(registerDto.Email, registerDto.UserName);

            if (!isUnique)
                return false;

            var user = mapper.Map<User>(registerDto);

            // Handle profile picture
            if (registerDto.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string uniqueFileName = DateTime.Now.Millisecond + "_" + registerDto.ProfilePicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await registerDto.ProfilePicture.CopyToAsync(stream);
                }

                user.ProfilePicture = uniqueFileName; // Profil resmi dosyasının yolunu saklar

                await unitOfWork.GetRepository<User>().AddAsync(user);
                await unitOfWork.SaveAsync();

                return true;
            }
            return false;
        }


        private async Task<bool> IsEmailAndUsernameUniqueAsync(string email, string username)
        {
            var existingUser = await unitOfWork.GetRepository<User>().GetAsync(u => u.Email == email || u.UserName == username);

            return existingUser == null;
        }


    }
}
