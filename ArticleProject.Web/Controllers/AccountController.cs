using ArticleProject.EntityLayer.DTOs.Login;
using ArticleProject.EntityLayer.DTOs.Register;
using ArticleProject.EntityLayer.Entities;
using ArticleProject.ServiceLayer.Services.Abstract;
using ArticleProject.ServiceLayer.Services.Concrete;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticleProject.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IValidator<User> validator;
        private readonly IMapper mapper;

        public AccountController(IUserService userService, IValidator<User> validator, IMapper mapper)
        {
            _userService = userService;
            this.validator = validator;
            this.mapper = mapper;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (await _userService.LoginAsync(userLoginDto.Email, userLoginDto.Password))
            {
                var user = await _userService.GetUserByEmail(userLoginDto.Email);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                };

                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("ArticleProject", principal); // CookieAuthentication burada tanımladığınız isim olmalı

                return RedirectToAction("ArticleList", "Article", new { area = "User" });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var map = mapper.Map<User>(registerDto);
            var result = await validator.ValidateAsync(map);
            if (result.IsValid)
            {
                bool registrationResult = await _userService.RegisterAsync(registerDto);

                if (registrationResult)
                    return RedirectToAction("Login", "Account", new { area = "" });
                else
                {
                    ModelState.AddModelError(string.Empty, "Bu kullanıcı adı veya Email'e sahip bir üyelik vardır. Bilgilerinizi değiştiriniz.");
                    return View();
                }
            }
            result.AddToModelState(this.ModelState);
            return View();
        }
    }
}
