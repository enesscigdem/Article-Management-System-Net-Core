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
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {
                UserId = Guid.NewGuid(),
                FirstName = "Enes",
                LastName = "Çiğdem",
                Email = "enescigdeem@gmail.com",
                UserName = "enescigdeem",
                Password = "123456",
                ProfilePicture = "-",
                IsActive = true,
                Role = "ADMIN"
            }, 
            new User
            {
                UserId = Guid.Parse("9614dd78-111c-42ec-8f02-379368493c0a"),
                FirstName = "Buse",
                LastName = "Çınar",
                Email = "busecinar@gmail.com",
                UserName = "busecinar",
                Password = "123456",
                ProfilePicture = "-",
                IsActive = true,
                Role = "USER"
            });
        }
    }
}
