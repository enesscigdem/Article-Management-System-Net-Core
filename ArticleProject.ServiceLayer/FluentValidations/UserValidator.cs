using ArticleProject.EntityLayer.DTOs.Users;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.AutoMapper.Users;
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

            RuleFor(x => x.Email)
              .NotEmpty()
              .MinimumLength(3)
              .MaximumLength(100)
              .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
              .WithName("Email");

            RuleFor(x => x.UserName)
               .NotEmpty()
               .MinimumLength(3)
               .MaximumLength(100)
               .WithName("Kullanıcı Adı");

            RuleFor(x => x.Password)
               .NotEmpty()
               .MinimumLength(3)
               .MaximumLength(100)
               .WithName("Şifre");
        }
    }
}
