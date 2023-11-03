using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.Entities
{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string ProfilePicture { get; set; }
        public bool IsActive { get; set; } = false;
        public string Role { get; set; } = "USER";
        public string? PreviousPassword1 { get; set; }
        public string? PreviousPassword2 { get; set; }
        public string? PreviousPassword3 { get; set; }
        public List<Article> Articles { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public List<FollowCategory> FollowedCategories { get; set; }
    }
}
