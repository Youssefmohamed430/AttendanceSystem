using System.Diagnostics;
using AttendanceSystem.Models;
using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;

        public HomeController(AppDbContext _context,ILogger<HomeController> logger)
        {
            _logger = logger;
            this.context = _context;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated )
            {
                return RedirectToAction("LogInForm", "AuthenticationService");
            }
            if (User.IsInRole("Student"))
            {
                var studentId = User.Claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
                if (!string.IsNullOrEmpty(studentId))
                {
                        var notifications = context.Notifications
                        .Where(x => x.StudentId == studentId)
                        .Where(x => x.IsRead == false)
                        .ToList();

                        ViewBag.Notife = notifications;

                    return View();
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
