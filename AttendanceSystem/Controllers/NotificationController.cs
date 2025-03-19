using AttendanceSystem.Models.Data;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AppDbContext context;

        public NotificationController(AppDbContext _context)
        {
            this.context = _context;
        }
        public IActionResult MarkAsRead(int Id)
        {
            var notife = context.Notifications.FirstOrDefault(x => x.Id == Id);

            if (notife != null)
            {
                notife.IsRead = true;
                context.SaveChanges();
            }

            return RedirectToAction("Index","Home");
        }
    }
}
