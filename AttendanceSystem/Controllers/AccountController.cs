using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace AttendanceSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public readonly AppDbContext context;
        public AccountController
            (AppDbContext _context,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = _context;
        }
        
        public IActionResult SignInForm()
        {
            var courses = context?.Courses.ToList();

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
                    context.Instructors.Add(instructor);
                    context.SaveChanges();
                    //Cookie 
                    await userManager.AddClaimAsync(user,new Claim("CrsId", InstModel.Courseid.ToString()));
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("SignInForm", InstModel);
        }
       

    }
}
