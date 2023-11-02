using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.AutoMapper.Articles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleListDto>().ReverseMap();    
            CreateMap<Article, UserArticlesDto>().ReverseMap();    
            CreateMap<Article, ArticleAddDto>().ReverseMap();    
            CreateMap<Article, ArticleUpdateDto>().ReverseMap();    
            CreateMap<ArticleAddDto, ArticleUpdateDto>().ReverseMap();    
            CreateMap<Article, ArticleListAllDto>().ReverseMap();    
        }
    }
}
