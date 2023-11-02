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
    //örneðin kullanýcý giriþ yapmadý ama admin/.. path'ini biliyor. bu durumda kullanýcýyý belirlemiþ olduðumuz bu path'e yönlendireceðiz.
    config.LoginPath = new PathString("/Account/Login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "ArticleProject",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest // http ve https destekler. canlýya çýkýlýrsa always seçilmesi daha mantýklýdýr.
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(1); // bu cookie'nin ne kadar sistemde tutulacaðýný belirliyoruz. örneðin bir siteye giriþ yaptým fromdays(7) tanýmlarsam 7 gün boyunca hiç çýkmayacak.
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied"); // Yetkisiz bir giriþ olduðunda burasý çalýþacak.
});

builder.Services.AddAuthentication("ArticleProject") // "ArticleProject" kimlik doðrulama düzenlemesinin adýdýr.
        .AddCookie("ArticleProject", config =>
        {
            config.LoginPath = "/Account/Login"; // Giriþ yapýlacak sayfanýn yolu
            config.LogoutPath = "/Account/Logout"; // Çýkýþ yapýlacak sayfanýn yolu
            // Diðer yapýlandýrma seçenekleri
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
app.UseAuthentication(); // bu ikisinin sýralamasý önemli ! authentication üstte kalmalý.
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

