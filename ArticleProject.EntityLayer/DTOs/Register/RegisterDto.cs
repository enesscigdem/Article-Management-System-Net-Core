using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.DTOs.Register
{
    public class RegisterDto
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [Display(Name = "Profil Resmi")]
        public IFormFile ProfilePicture { get; set; }
        public bool IsActive { get; set; } = false;
        public string Role { get; set; } = "USER";
    }
}
