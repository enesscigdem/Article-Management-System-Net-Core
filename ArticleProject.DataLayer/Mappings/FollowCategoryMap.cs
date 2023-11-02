using ArticleProject.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.DataLayer.Mappings
{
    public class FollowCategoryMap : IEntityTypeConfiguration<FollowCategory>
    {
        public void Configure(EntityTypeBuilder<FollowCategory> builder)
        {
            builder.HasKey(fc => new { fc.UserId, fc.CategoryId });

            // Define the relationships with User and Category entities
            builder.HasOne(fc => fc.User)
                .WithMany(u => u.FollowedCategories)
                .HasForeignKey(fc => fc.UserId);
        }
    }
}
