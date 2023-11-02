using ArticleProject.DataLayer.UnitOfWorks;
using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.DTOs.Register;
using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Extensions;
using ArticleProject.ServiceLayer.Services.Abstract;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;
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
            var user = await unitOfWork.GetRepository<User>().GetAsync(u => u.Email == email && u.IsActive == true);
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
                    Directory.CreateDirectory(uploadsFolder);
                string userFolder = Path.Combine(uploadsFolder, "userimage");
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder);
                string uniqueFileName = DateTime.Now.Millisecond + "_" + registerDto.ProfilePicture.FileName;
                string filePath = Path.Combine(userFolder, uniqueFileName);
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
        public async Task<UserProfileDto> GetUserProfileAsync()
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var getUser = await unitOfWork.GetRepository<User>().GetAsync(x => x.UserId == userId);
            var map = mapper.Map<UserProfileDto>(getUser);

            return map;
        }
        public async Task UpdateUserProfileAsync(UserProfileDto userProfileDto)
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await unitOfWork.GetRepository<User>().GetAsync(x => x.UserId == userId);

            if (user != null)
            {
                user.FirstName = userProfileDto.FirstName;
                user.LastName = userProfileDto.LastName;
                user.Email = userProfileDto.Email;
                user.UserName = userProfileDto.UserName;

                if (!string.IsNullOrEmpty(userProfileDto.NewPassword))
                {
                    if (!IsNewPasswordDistinct(user, userProfileDto.NewPassword))
                    {
                        throw new ValidationException("Son 3 şifreniz birbirinden farklı olmalıdır!");
                    }

                    // Update the previous password history
                    user.PreviousPassword3 = user.PreviousPassword2;
                    user.PreviousPassword2 = user.PreviousPassword1;
                    user.PreviousPassword1 = user.Password;

                    user.Password = userProfileDto.NewPassword;
                }

                if (!string.IsNullOrEmpty(userProfileDto.ProfilePicture))
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    string userFolder = Path.Combine(uploadsFolder, "userimage");
                    if (!Directory.Exists(userFolder))
                        Directory.CreateDirectory(userFolder);

                    // Resim byte dizisine dönüştürme
                    byte[] imageBytes = Convert.FromBase64String(userProfileDto.ProfilePicture);

                    // Resmi kaydetme
                    string uniqueFileName = DateTime.Now.Millisecond + "_" + Guid.NewGuid() + ".jpg"; // Resim dosyasının adını benzersiz hale getir
                    string filePath = Path.Combine(userFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await fileStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                    }

                    user.ProfilePicture = userProfileDto.ProfilePicture;
                }

                await unitOfWork.GetRepository<User>().UpdateAsync(user);
                await unitOfWork.SaveAsync();
            }
        }
        public async Task PassiveAccount()
        {
            var userId = Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = await unitOfWork.GetRepository<User>().GetAsync(x => x.UserId == userId);

            if (user != null)
            {
                user.IsActive = false;
                await unitOfWork.GetRepository<User>().UpdateAsync(user);
                await unitOfWork.SaveAsync();
            }
        }
        private async Task<bool> IsEmailAndUsernameUniqueAsync(string email, string username)
        {
            var existingUser = await unitOfWork.GetRepository<User>().GetAsync(u => u.Email == email || u.UserName == username);

            return existingUser == null;
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await unitOfWork.GetRepository<User>().GetAsync(u => u.Email == email);
        }

        private bool IsNewPasswordDistinct(User user, string newPassword)
        {
            // Check if the new password is different from the previous three passwords
            return newPassword != user.Password &&
                newPassword != user.PreviousPassword1 &&
                newPassword != user.PreviousPassword2 &&
                newPassword != user.PreviousPassword3;
        }
    }
}
