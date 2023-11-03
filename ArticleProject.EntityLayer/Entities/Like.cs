using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.Entities
{
    public class Like
    {
        public Guid LikeId { get; set; }

        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
