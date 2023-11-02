using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.Entities
{
    public class FollowCategory
    {
        [Key] // Define UserId as the primary key
        public Guid UserId { get; set; }

        [Key] // Define CategoryId as the primary key
        public Guid CategoryId { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
    }
}
