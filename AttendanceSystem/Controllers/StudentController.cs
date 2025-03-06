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

                    await userManager.AddClaimAsync(user, new Claim("StudentId", user.Id));

                    Student student = new Student() { Id = user.Id };
                    context.Students.Add(student);
                    this.crsid = Convert.ToInt32(User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value);
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
            return RedirectToAction("GetStudentDetails");
        }
        public IActionResult GetStudentDetails(string id)
        {
            this.instructorId = User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var students = GetStudents(instructorId);

            return View(students);
        }
        public IQueryable<Enrolllment>? GetStudents(string id)
        {
            this.crsid = Convert.ToInt32(User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value);

            var students = context?.Enrolllments
                .AsNoTracking()
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Where(x => x.CrsId == crsid);

            return students;
        }
        public IActionResult IsStudentPresent(string stuid,bool ispresent)
        {
            this.crsid = Convert.ToInt32(User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value);

            var IsPresentToday = context.Students
                .Include(x => x.Attendances)
                .FirstOrDefault(x => x.Id == stuid)?.Attendances?.Any(x =>
                x.Date == DateOnly.FromDateTime(DateTime.Now)
                && x.CrsId == crsid);

            if(IsPresentToday == false)
            {
                Attendance attendance = new Attendance()
                {
                    StudId = stuid,
                    IsPresent = ispresent,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    CrsId = (int)crsid
                };

                context.Attendances.Add(attendance);

                TempData["Message"] = "Attendance has been registered";
            }
            else
            {
                TempData["Message"] = "Attendance has already been registered";
                return RedirectToAction("AttendancePage", "Instructor");
            }

            if(ispresent)
            {
                var enroll = context.Enrolllments.FirstOrDefault(x => 
                x.StudId == stuid && x.CrsId == crsid);

                enroll.CrsAttendanceRate++;

            }
            context.SaveChanges();

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
            this.crsid = Convert.ToInt32(User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value);

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
        public IActionResult Profile(string Id)
        {
            var enrollments = context?.Enrolllments
                .AsNoTracking()
                .Include(x => x.course)
                .ThenInclude(x => x.instructor)
                .ThenInclude(x => x.User)
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Where(x => x.StudId == Id)
                .ToList();

            ProfileViewModell ProfileViewModel = new ProfileViewModell()
            {
                Name = enrollments[0].student.User.Name,
                Email = enrollments[0].student.User.Email,
                Phone = enrollments[0].student.User.PhoneNumber,
                UserName = enrollments[0].student.User.UserName,
                enrolllments = enrollments
            };

            return View(ProfileViewModel);
        }

        public IActionResult SaveUpdates(string id ,ProfileViewModell profStudView)
        {
            var studentUser = context?.Users?.FirstOrDefault(x => x.Id == id);
            
            studentUser.Name = profStudView.Name;
            studentUser.Email = profStudView.Email;
            studentUser.PhoneNumber = profStudView.Phone;
            studentUser.UserName = profStudView.UserName;

            context?.SaveChanges();

            var enrollments = context?.Enrolllments
                .AsNoTracking()
                .Include(x => x.course)
                .ThenInclude(x => x.instructor)
                .ThenInclude(x => x.User)
                .Where(x => x.StudId == id)
                .ToList();

            profStudView.enrolllments = enrollments;

            return View("Profile", profStudView);
        }
    }
}
