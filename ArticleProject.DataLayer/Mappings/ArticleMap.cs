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
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasMany(a => a.Comments)
                   .WithOne(c => c.Article)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(new Article
            {
                ArticleId = Guid.NewGuid(),
                Title = "Asp.net Core Deneme Makalesi 1",
                Content = "Lorem Ipsum, Çiçero'nun MÖ 45 yılında yazdığı \"de Finibus Bonorum et Malorum – İyi ve Kötünün Uç Sınırları\" eserindeki 1.30.32 sayılı paragrafında yer alır. Bu eser Rönesans döneminde etik teorileri üzerine bilimsel inceleme konusu haline gelmiştir. Lorem Ipsum 1500'lü yıllardan itibaren aşağıdaki formuyla standartlaşmıştır: Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                CreationDate = DateTime.Now,
                Image = "-",
                Views = 41028,
                Likes = 20,
                IsActive = true,
                AuthorId = Guid.Parse("9614dd78-111c-42ec-8f02-379368493c0a"),
            });
        }
    }
}
