using AttendanceSystem.Models.Entities;
using AttendanceSystem.Models.Repositories;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AttendanceSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IRepositery<Instructor> InstRepo;
        private readonly IRepositery<Course> crsRepo;
        public AccountController
            (IRepositery<Course> crsRepo, IRepositery<Instructor> InstRepo,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.InstRepo = InstRepo;
            this.crsRepo = crsRepo;
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
                        await signInManager.SignInAsync(user, loginmodel.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Username OR Password wrong");
            }
            return View("Login", loginmodel);
        }
        public IActionResult SignInForm()
        {
            var courses = crsRepo.GetAll();

            if (courses == null)
            {
                courses = new List<Course>(); 
            }

            ViewBag.Courses = new SelectList(courses, "Id", "Name");

            return View();
        }
        public async Task<IActionResult> SaveAccount(InstructorViewModel InstModel) 
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.Email = InstModel.Email;
                user.Name = InstModel.Name;
                user.PasswordHash = InstModel.password;
                user.UserName = InstModel.UserName;

                IdentityResult result = 
                    await userManager.CreateAsync(user,InstModel.password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Instructor");
                    Instructor instructor = new Instructor()
                    { Id = user.Id , CrsId = InstModel.Courseid };
                    InstRepo.Add(instructor);
                    InstRepo.Save();
                    //Cookie 
                    await signInManager.SignInAsync(user,false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("SignInForm", InstModel);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

    }
}
