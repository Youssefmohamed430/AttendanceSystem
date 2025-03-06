using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var instid = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var students = GetStudents(instid.Value);
            return View(students);
        }
        [Authorize]
        public IActionResult AddStudentForm()
        {
            return View();    
        }
        public IQueryable<ApplicationUser> GetStudents(string id)
        {
            var course = User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value;

            var students = context?.Enrolllments
                .AsNoTracking()
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Where(x => x.CrsId == Convert.ToInt32(course))
                .Select(x => x.student.User);

            return students;
        }

    }
}
