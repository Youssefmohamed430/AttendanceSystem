using AttendanceSystem.Models.Entities;
using AttendanceSystem.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IRepositery<Instructor> InstRepo;
        private readonly IRepositery<Course> crsRepo;
        public InstructorController
            (IRepositery<Course> crsRepo, IRepositery<Instructor> InstRepo, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.InstRepo = InstRepo;
            this.crsRepo = crsRepo;
        }
        public IActionResult AttendancePage()
        {
            return View();
        }
        public IActionResult GetInstructorStudents()
        {
            return View();
        }
    }
}
