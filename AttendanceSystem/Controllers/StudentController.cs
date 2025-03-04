using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using AttendanceSystem.Models.Repositories;
using AttendanceSystem.Views.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    public class StudentController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IInstructorRepository InstRepo;
        private readonly IRepositery<Student> stuRepo;
        private readonly IRepositery<Enrolllment> enrollmentrepo;
        private readonly AppDbContext context;
        public StudentController
            (AppDbContext _context,IRepositery<Enrolllment> enrollmentrepo, IRepositery<Student> stuRepo, IInstructorRepository InstRepo, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.InstRepo = InstRepo;
            this.stuRepo = stuRepo;
            this.enrollmentrepo = enrollmentrepo;
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
                    stuRepo.Add(student);
                    stuRepo.Save();
                    string instructorId = TempData.Peek("Instructor")?.ToString();
                    int? crsid = InstRepo.GetCourseIdToInstructor(instructorId);
                    Enrolllment enrolllment = new Enrolllment()
                    {
                        StudId = user.Id,
                        CrsId = (int)crsid,
                        CrsAttendanceRate = 0
                    };
                    enrollmentrepo.Add(enrolllment);
                    enrollmentrepo.Save();
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
    }
}
