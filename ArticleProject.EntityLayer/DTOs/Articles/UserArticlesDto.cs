using ArticleProject.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.DTOs.Articles
{
    public class UserArticlesDto
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

    }
}
