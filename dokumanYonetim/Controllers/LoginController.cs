using dokumanYonetim.Data;
using dokumanYonetim.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace dokumanYonetim.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User u)
        {
            // Veritabanında kullanıcıyı kontrol et
            var datavalue = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == u.UserName && x.Password == u.Password);

            if (datavalue != null)
            {
                // Kullanıcı rolünü al (örneğin, datavalue.Role)
                var role = datavalue.Role;

                if (string.IsNullOrEmpty(role))
                {
                    // Rol bilgisi eksikse varsayılan bir rol belirleyin
                    role = "default";
                }

                // Kullanıcı kimliği bilgilerini içeren Claim listesi oluştur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, datavalue.UserId.ToString()), // UserId yerine NameIdentifier kullanılıyor
                    new Claim(ClaimTypes.Name, datavalue.UserName),
                    new Claim(ClaimTypes.Role, role), // Rol bilgisini kullan
                };

                // Kimlik doğrulama türünü tanımla
                var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(userIdentity);

                // Kullanıcıyı oturum açmış olarak işaretle
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Giriş başarılı olduğunda yönlendirme yap
                return RedirectToAction("Index", "Home");
            }

            // Giriş başarısızsa hata mesajı döndür ve aynı sayfaya yönlendir
            ViewBag.ErrorMessage = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcıyı oturumdan çıkart
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}
