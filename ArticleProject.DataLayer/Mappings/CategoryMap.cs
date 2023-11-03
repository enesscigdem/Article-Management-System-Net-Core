using ArticleProject.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.DataLayer.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasData(new Category
            {
                CategoryId = Guid.Parse("3ced153f-93fb-4415-a5e8-2f97d6ae5d73"),
                CategoryName = "Magazin",
                Description = "Magazin Açıklaması",
                IsActive = false,
            },
          new Category
          {
              CategoryId = Guid.NewGuid(),
              CategoryName = "Spor",
              Description = "Spor Açıklaması",
              IsActive = false,
          }, 
          new Category
          {
              CategoryId = Guid.NewGuid(),
              CategoryName = "Gündem",
              Description = "Gündem Açıklaması",
              IsActive = false,
          }, 
          new Category
          {
              CategoryId = Guid.NewGuid(),
              CategoryName = "Haber",
              Description = "Haber Açıklaması",
              IsActive = false,
          }, 
          new Category
          {
              CategoryId = Guid.NewGuid(),
              CategoryName = "Teknoloji",
              Description = "Teknoloji Açıklaması",
              IsActive = false,
          });
        }
    }
}
