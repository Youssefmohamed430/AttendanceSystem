using AttendanceSystem.Models.Entities;
using AttendanceSystem.Models.Repositories;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IInstructorRepository InstRepo;
        private readonly IRepositery<Student> stuRepo;
        private readonly IRepositery<Enrolllment> enrollmentrepo;
        private readonly string? instructorId;
        public InstructorController
            (IRepositery<Enrolllment> enrollmentrepo,IRepositery<Student> stuRepo,IInstructorRepository InstRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.InstRepo = InstRepo;
            this.stuRepo = stuRepo;
            this.enrollmentrepo = enrollmentrepo;
        }
        [Authorize]
        public IActionResult AttendancePage()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogInForm", "Account");
            }
            string instructorId = TempData.Peek("Instructor")?.ToString();
            var students = InstRepo.GetStudents(instructorId);
            return View(students);
        }
        [Authorize]
        public IActionResult AddStudentForm()
        {
            return View();    
        }
    }
}
