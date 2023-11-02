﻿using ArticleProject.EntityLayer.DTOs.Articles;
using ArticleProject.EntityLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.FluentValidations
{
    public class ArticleValidator : AbstractValidator<ArticleAddDto>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithName("Başlık");

            RuleFor(x => x.Content)
              .NotEmpty()
              .MinimumLength(3)
              .MaximumLength(1000)
              .WithName("İçerik");

            RuleFor(x => x.CategoryIds)
                .Must(categories => categories != null && categories.Any())
                .WithMessage("En az bir kategori seçmelisiniz.")
                .WithName("Kategoriler");

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Resim yüklemelisiniz.")
                .WithName("Resim");
        }
    }
}
