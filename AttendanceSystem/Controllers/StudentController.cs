using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Models.Repositories;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;
        public StudentController
            (AppDbContext _context,UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.context = _context;
        }
        public async Task<IActionResult> AddStudent(StudentViewModel studmodel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = studmodel.UserName;
                user.Email = studmodel.Email;
                user.Name = studmodel.Name;
                user.PasswordHash = studmodel.password;
                user.PhoneNumber = studmodel.Phone;

                IdentityResult result =
                    await userManager.CreateAsync(user, studmodel.password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Student");
                    Student student = new Student() { Id = user.Id };
                    context.Students.Add(student);
                    string instructorId = TempData.Peek("Instructor")?.ToString();
                    int? crsid = context?.Instructors.FirstOrDefault(x => x.Id == instructorId)?.CrsId;
                    Enrolllment enrolllment = new Enrolllment()
                    {
                        StudId = user.Id,
                        CrsId = (int)crsid,
                        CrsAttendanceRate = 0
                    };
                    context.Enrolllments.Add(enrolllment);
                    context.SaveChanges();
                    return RedirectToAction("AttendancePage", "Instructor");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return RedirectToAction("AddStudentForm","Instructor", studmodel);
        }
        public IActionResult RemoveStudent(string id)
        {
            var StudentUser = context.Users.FirstOrDefault(x => x.Id == id);
            context.Users.Remove(StudentUser);
            context.SaveChanges();
            return RedirectToAction("AttendancePage", "Instructor");
        }
        public IActionResult GetStudentDetails(string id)
        {
            string? instructorId = TempData.Peek("Instructor")?.ToString();
            var students = GetStudents(instructorId);
            return View(students);
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
        public IActionResult IsStudentPresent(string stuid,bool ispresent)
        {
            var attendances = context.Students
                .Include(x => x.Attendances)
                .FirstOrDefault(x => x.Id == stuid)?.Attendances;

            var IsPresentToday = 
                attendances?.Any(x => x.Date == DateOnly.FromDateTime(DateTime.Now));
            if(IsPresentToday == false)
            {
                Attendance attendance = new Attendance()
                {
                    StudId = stuid,
                    IsPresent = ispresent,
                    Date = DateOnly.FromDateTime(DateTime.Now)
                };
                context.Attendances.Add(attendance);
                context.SaveChanges();
                TempData["Message"] = "Attendance has been registered";
            }
            else
            {
                TempData["Message"] = "Attendance has already been registered";
            }
            return RedirectToAction("AttendancePage", "Instructor");
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

        public IActionResult AddExistingStudent(string stuid)
        {
            string instructorId = TempData.Peek("Instructor")?.ToString();
            int? crsid = context?.Instructors.FirstOrDefault(x => x.Id == instructorId)?.CrsId;
            Enrolllment enrolllment = new Enrolllment()
            {
                StudId = stuid,
                CrsId = (int)crsid,
                CrsAttendanceRate = 0
            };
            context.Enrolllments.Add(enrolllment);
            context.SaveChanges();
            return RedirectToAction("GetStudentDetails");
        }
    }
}
