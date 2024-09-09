using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagement.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
             
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.Username == username && a.Password == password);
            if (admin != null)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username)
            };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminScheme");

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync("AdminScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

                TempData["SuccessMessage"] = "Giriş başarılı!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
            return View();
        }



        public IActionResult Register()
        {
             
            return View();
        }

        [HttpPost]
        public IActionResult Register(Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Admins.Add(admin);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Kayıt başarılı!";
                return RedirectToAction("Index", "Home");
            }
            return View(admin);
        }



        public async Task<IActionResult> Logout()
        {
            // Oturumu kapat
            await HttpContext.SignOutAsync("AdminScheme");
            //pencereyi kapattığında oturumu kapat
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
