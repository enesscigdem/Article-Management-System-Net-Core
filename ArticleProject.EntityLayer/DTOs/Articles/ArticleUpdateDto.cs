using ArticleProject.EntityLayer.DTOs.Categories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ArticleProject.EntityLayer.DTOs.Articles
{
    public class ArticleUpdateDto
    {
        public Guid ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public string? Image { get; set; }
        public IList<CategoryListDto> Categories { get; set; }
    }
}
