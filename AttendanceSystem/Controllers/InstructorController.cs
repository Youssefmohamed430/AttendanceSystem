using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Models.Repositories;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;

        public InstructorController
            (AppDbContext _context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = _context;
        }
        [Authorize]
        public IActionResult AttendancePage()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogInForm", "Account");
            }
            string? instructorId = TempData.Peek("Instructor")?.ToString();
            var students = GetStudents(instructorId);
            return View(students);
        }
        [Authorize]
        public IActionResult AddStudentForm()
        {
            return View();    
        }
        public List<ApplicationUser?>? GetStudents(string id)
        {
            var course = context?.Instructors.FirstOrDefault(x => x.Id == id)?.CrsId;

            var students = context?.Enrolllments
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Where(x => x.CrsId == course)
                .Select(x => x.student.User)
                .ToList();

            return students;
        }
    }
}
