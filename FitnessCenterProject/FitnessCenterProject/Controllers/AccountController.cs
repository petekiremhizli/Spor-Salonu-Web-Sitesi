using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Uye> _userManager;
        private readonly SignInManager<Uye> _signInManager;

        public AccountController(
            UserManager<Uye> userManager,
            SignInManager<Uye> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ================== LOGIN ==================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                false,
                false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email veya şifre hatalı");
                return View(model);
            }

            // ✅ ROLE GÖRE YÖNLENDİRME
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Adminn");

            if (await _userManager.IsInRoleAsync(user, "Uye"))
                return RedirectToAction("Index", "Uye");

            return RedirectToAction("Index", "Home");
        }


        // ================== SIGN UP ==================

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new Uye
            {
                UserName = model.Email,
                Email = model.Email,
                Ad = model.Ad,
                Soyad = model.Soyad,
                Telefon = model.Telefon
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                // ✅ BURASI KRİTİK
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Uye");
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Uye");

        }


            

        // ================== LOGOUT ==================

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
