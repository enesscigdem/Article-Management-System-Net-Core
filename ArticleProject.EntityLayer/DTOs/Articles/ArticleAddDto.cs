using ArticleProject.EntityLayer.DTOs.Categories;
using ArticleProject.EntityLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.EntityLayer.DTOs.Articles
{
    public class ArticleAddDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Guid> CategoryIds { get; set; } // Birden fazla kategori seçimi için Guid listesi.
        public Guid AuthorId { get; set; }
        public IFormFile? Image { get; set; }
        public IList<CategoryListDto> Categories { get; set; }
    }
}
