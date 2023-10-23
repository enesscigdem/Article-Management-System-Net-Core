using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.Entities
{
    public class Follow
    {
        public Guid FollowId { get; set; }

        public Guid FollowerId { get; set; }
        public User Follower { get; set; }

        public Guid FollowingId { get; set; }
        public User Following { get; set; }
    }
}
