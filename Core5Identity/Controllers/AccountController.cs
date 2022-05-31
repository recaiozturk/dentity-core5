using Core5Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Core5Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }
 
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user= await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    //Daha önceki cookie bilgilerini silelim
                    await signInManager.SignOutAsync();

                    //ilk false:kullanıcı kalıcı oluşturulmasın
                    var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        //returnurl ye gitsin boş ise anasayfaya gitsin
                        return Redirect(returnUrl ?? "/");
                    }

                }

                ModelState.AddModelError("Email", "Invalid Email of Password");
            }

            
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager?.SignOutAsync();
            return RedirectToAction("Index", "Home");   
        }
    }
}
