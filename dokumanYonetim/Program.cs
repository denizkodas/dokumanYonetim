using dokumanYonetim.Data;
using dokumanYonetim.Models.Repositories;
using dokumanYonetim.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Veritabanı bağlantısını yapılandırma
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
     options.EnableSensitiveDataLogging();
   });

// Identity yapılandırması (AspNet Identity kullanıyorsanız)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Cookie Authentication ayarları
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Giriş yapılmadığında yönlendirilecek sayfa
    });

// Dependency Injection yapılandırmaları
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// HTTP isteği işlem hattını yapılandırma
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Kimlik doğrulama ve yetkilendirme middleware'ini ekleyin
app.UseAuthentication(); // Bu middleware kimlik doğrulama için gerekli
app.UseAuthorization();

// Varsayılan route'u Login/Index olarak ayarlıyoruz
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
