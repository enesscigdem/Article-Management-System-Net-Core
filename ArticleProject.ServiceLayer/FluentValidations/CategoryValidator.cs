using ArticleProject.EntityLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.FluentValidations
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithName("Kategori Adı");
            RuleFor(x => x.Description)
              .NotEmpty()
              .MinimumLength(3)
              .MaximumLength(1000)
              .WithName("Kategori Açıklaması");
        }
    }
}
