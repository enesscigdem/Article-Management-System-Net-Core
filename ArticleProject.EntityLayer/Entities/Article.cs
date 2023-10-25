using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.Entities
{
    public class Article
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string Image { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public Guid AuthorId { get; set; }
        public User Author { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
