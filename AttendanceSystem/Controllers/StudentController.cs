using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AttendanceSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;
        string? instructorId;
        int? crsid;
        public StudentController
            (AppDbContext _context,UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.context = _context;
        }
        public IActionResult AddExistingStudentForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchStudent(string Stuname)
        {
            if (string.IsNullOrWhiteSpace(Stuname))
            {
                TempData["Message"] = "Please enter a student name.";
                return View("AddExistingStudentForm");
            }
            
            var student = context.Users
                         .FirstOrDefault(x => x.Name.ToLower().Contains(Stuname.ToLower()));

            if (student == null)
            {
                TempData["Message"] = "The Student Not Found";
                return View("AddExistingStudentForm",student);
            }

            return View("AddExistingStudentForm", student);
        }      
        public IActionResult ShowStudentCoursesView()
        {
            return View();
        }
        public IActionResult ShowStudentCourses(string Stuname)
        {
            if (string.IsNullOrWhiteSpace(Stuname))
            {
                TempData["Message"] = "Please enter a student name.";
                return View("ShowStudentCoursesView");
            }

            var student = context.Users
                         .FirstOrDefault(x => x.Name.ToLower().Contains(Stuname.ToLower()));

            if (student == null)
            {
                TempData["Message"] = "The Student Not Found";
                return View("ShowStudentCoursesView");
            }

            var enrollments = context?.Enrolllments
                .AsNoTracking()
                .Include(x => x.course)
                .ThenInclude(x => x.instructor)
                .ThenInclude(x => x.User)
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Where(x => x.StudId == student.Id)
                .ToList();

            return View("ShowStudentCoursesView", enrollments);
        }
    
    }
}
