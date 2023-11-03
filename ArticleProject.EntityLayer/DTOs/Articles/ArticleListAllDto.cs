using ArticleProject.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.DTOs.Articles
{
    public class ArticleListAllDto
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string? Image { get; set; }
        public int Views { get; set; } = 0;
        public int Likes { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        public List<Category> Categories { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
