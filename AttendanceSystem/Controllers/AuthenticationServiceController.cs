using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    public class AuthenticationServiceController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public readonly AppDbContext context;
        public AuthenticationServiceController
            (AppDbContext _context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = _context;
        }
        public IActionResult LogInForm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogInCheck(LogInViewModel loginmodel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByNameAsync(loginmodel.UserName);
                if (user != null)
                {
                    bool found =
                        await userManager.CheckPasswordAsync(user, loginmodel.Password);
                    if (found)
                    {
                        var crsid = context?.Instructors?.FirstOrDefault(x => x.Id == user.Id)?.CrsId;
                        await signInManager.SignInAsync(user, loginmodel.RememberMe);
                        
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Username or Password wrong");
            }
            return View("LogInForm", loginmodel);
        }
        public async Task<IActionResult> Signout()
        {
            await signInManager.SignOutAsync();

            HttpContext.Session.Clear();

            return RedirectToAction("LogInForm", "AuthenticationService");
        }
    }
}
