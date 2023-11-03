using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
