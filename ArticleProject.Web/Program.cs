using ArticleProject.DataLayer.Extensions;
using ArticleProject.ServiceLayer.Extensions;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.LoadDataLayerExtension(builder.Configuration);
builder.Services.LoadServiceLayerExtension();

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddControllersWithViews()
    .AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
    {
        PositionClass = ToastPositions.TopRight,
        TimeOut = 3000
    })
    .AddRazorRuntimeCompilation();

builder.Services.ConfigureApplicationCookie(config =>
{
    //�rne�in kullan�c� giri� yapmad� ama admin/.. path'ini biliyor. bu durumda kullan�c�y� belirlemi� oldu�umuz bu path'e y�nlendirece�iz.
    config.LoginPath = new PathString("/Admin/Auth/Login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "ArticleProject",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest // http ve https destekler. canl�ya ��k�l�rsa always se�ilmesi daha mant�kl�d�r.
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(1); // bu cookie'nin ne kadar sistemde tutulaca��n� belirliyoruz. �rne�in bir siteye giri� yapt�m fromdays(7) tan�mlarsam 7 g�n boyunca hi� ��kmayacak.
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied"); // Yetkisiz bir giri� oldu�unda buras� �al��acak.
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication(); // bu ikisinin s�ralamas� �nemli ! authentication �stte kalmal�.
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "/Admin/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Admin" }
);

app.MapControllerRoute(
    name: "User",
    pattern: "/User/{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "User" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.Run();

