using ArticleProject.EntityLayer.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleProject.ServiceLayer.FluentValidations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50)
                .WithName("İsim");

            RuleFor(x => x.LastName)
               .NotEmpty()
               .MinimumLength(3)
               .MaximumLength(50)
               .WithName("Soyisim");
        }
    }
}
