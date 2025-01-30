using KonferansPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KonferansPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace KonferansPortal.Controllers
{
    
    public class UyeController : Controller
    {
   
        private SignInManager<Uye> _signInManager;
        private UserManager<Uye> _userManager;
        private readonly AppDbContext _context;

        public UyeController(AppDbContext context, SignInManager<Uye> signInManager, UserManager<Uye> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsEmailInUse(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                return Json($"Email {email} sistemde kayıtlı.");
            }

            return Json(true);
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Şifreler Uyuşmuyor");
            }

            if (ModelState.IsValid)
            {
                var uye = new Uye { UserName = model.Email, Email = model.Email,  Name = model.Name, Surname = model.Surname, Phone = model.Phone };
                var result = await _userManager.CreateAsync(uye, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(uye, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    model.ErrorMessage = "Email Kayıtlı Değil.";
                    return View(model);
                }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    model.ErrorMessage = "Şifre Hatalı.";
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var result = await _context.Uyeler.FirstOrDefaultAsync(u => User.Identity.Name == u.Email);
            if(result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        public async Task<IActionResult> Konferanslarim()
        {
            var result = await _context.Uyeler.Include(u=> u.katilinanKonferanslar).FirstOrDefaultAsync(u => User.Identity.Name == u.Email);
            if (result == null)
            {
                return NotFound();
            }
            return View(result.katilinanKonferanslar);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UpdateProfileViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Phone = user.Phone
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Phone = model.Phone;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!passwordChangeResult.Succeeded)
                    {
                        foreach (var error in passwordChangeResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                await _signInManager.RefreshSignInAsync(user);
                return RedirectToAction("Profile", new { message = "Profile updated successfully." });
            }
            return View(model);
        }
    }
}
