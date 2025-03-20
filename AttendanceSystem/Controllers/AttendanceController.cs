using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendanceSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppDbContext context;
        int? crsid;

        public AttendanceController
            (AppDbContext _context, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.context = _context;
        }
        public IActionResult IsStudentPresent(string stuid, bool ispresent)
        {
            string InstName = User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            this.crsid = Convert.ToInt32(User?.Claims?.FirstOrDefault(x => x.Type == "CrsId")?.Value);
            string CrsName = context?.Courses?.FirstOrDefault(x => x.Id == crsid)?.Name;

            string message = "Your absence was recorded by " + InstName + " in " + CrsName + "'s course.";

            Attendance attendance = new Attendance()
            {
                StudId = stuid,
                IsPresent = ispresent,
                Date = DateOnly.FromDateTime(DateTime.Now),
                CrsId = (int)crsid
            };

            context.Attendances.Add(attendance);

            TempData["Message"] = "Attendance has been registered";


            if (ispresent)
            {
                var enroll = context.Enrolllments.FirstOrDefault(x =>
                x.StudId == stuid && x.CrsId == crsid);

                enroll.CrsAttendanceRate++;
                message = "You have been registered by " + InstName + " for " + CrsName + "'s course.";
            }

            Notification notification = new Notification()
            {
                StudentId = stuid,
                Message = message,
            };

            context.Notifications.Add(notification);
            context.SaveChanges();

            return RedirectToAction("AttendancePage", "Instructor");
        }

    }
}
