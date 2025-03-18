using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace AttendanceSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CourseController : Controller 
    {
        private readonly AppDbContext context;
        public CourseController
            (AppDbContext _context)
        {
            this.context = _context;
        }
        

        public IActionResult CoursesDetails()
        {
            var coursesDetails = context.Courses
                .Include(x => x.Enrolllments)
                .Include(x => x.instructor)
                .ThenInclude(x => x.User)
                .ToList();

            return View(coursesDetails);
        }
        public IActionResult AddCourseForm()
        {
            return View();
        }
        public IActionResult AddNewCourse(Course course)
        {
            if (ModelState.IsValid)
            {
                Course Newcourse = new Course() { Name = course.Name };
                context.Courses.Add(Newcourse);
                context.SaveChanges();
            }

            return RedirectToAction("CoursesDetails");
        }
    }
}
