using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace AttendanceSystem.Controllers
{
    public class InstructorController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext context;
        private int crsid;
        public InstructorController
            (ILogger<HomeController> logger,AppDbContext _context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
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
        public IQueryable<UserDTO> GetStudents(string id)
        {

            var course = User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value;

            var students = context?.Enrolllments
                .AsNoTracking()
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Include(x => x.student)
                .ThenInclude(x => x.Attendances)
                .Where(x => x.CrsId == Convert.ToInt32(course))
                .Select(x => new UserDTO
                {
                    Name = x.student.User.Name,
                    Id = x.student.User.Id,
                    HasAttend = x.student.Attendances
                    .Any(a => a.CrsId == Convert.ToInt32(course)
                    && a.Date == DateOnly.FromDateTime(DateTime.Now))
                });

            return students;
        }

    }
}
