using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AttendanceSystem.Controllers
{
    public class CRUDStudentController :  Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;
        string? instructorId;
        int? crsid;
        public CRUDStudentController
            (AppDbContext _context, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.context = _context;
        }

        /* ===========>>>> Create Operations <<<<=========== */
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
                    Notification notification = new Notification()
                    {
                        StudentId = user.Id,
                        Message = "You have been enrolled in a course"
                    };
                    context.Notifications.Add(notification);
                    context.SaveChanges();
                    return RedirectToAction("AttendancePage", "Instructor");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return RedirectToAction("AddStudentForm", "Instructor", studmodel);
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
        
        /* ===========>>>> Read Operations <<<<=========== */
        public IActionResult GetStudentDetails(string id)
        {
            this.instructorId = User?.Claims?
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var students = GetStudents(instructorId);

            return View(students);
        }
        public IQueryable<UserDTO>? GetStudents(string id)
        {
            this.crsid = Convert.ToInt32(User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value);

            var students = context?.Enrolllments
                .AsNoTracking()
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Where(x => x.CrsId == crsid)
                .Select(x => new UserDTO
                {
                    Id = x.StudId,
                    Name = x.student.User.Name,
                    Email = x.student.User.Email,
                    CrsAttendanceRate = x.CrsAttendanceRate,
                    Phone = x.student.User.PhoneNumber,
                });

            Console.WriteLine(students.ToQueryString());

            return students;
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

            var notifications = context.Notifications
                         .Where(x => x.StudentId == Id)
                         .Where(x => x.IsRead == false)
                         .ToList();

            ViewBag.Notife = notifications;

            return View(ProfileViewModel);
        }
        
        /* ===========>>>> Update Operations <<<<=========== */
        public IActionResult SaveUpdates(string id, ProfileViewModell profStudView)
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

        /* ===========>>>> Delete Operations <<<<=========== */
        public IActionResult RemoveStudent(string id)
        {
            this.crsid = Convert.ToInt32(User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value);

            var studentEnrollment = context.Enrolllments
                .FirstOrDefault(x => x.StudId == id && x.CrsId == crsid);

            context.Enrolllments.Remove(studentEnrollment);

            context.Attendances.Where(x => x.CrsId == crsid && x.StudId == id).ExecuteDelete();

            context.SaveChanges();

            return RedirectToAction("GetStudentDetails");
        }

    }
}
