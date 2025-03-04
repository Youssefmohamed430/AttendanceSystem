using AttendanceSystem.Models.Data;
using AttendanceSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Models.Repositories
{
    public class InstructorRepositery : IInstructorRepository
    {
        public readonly AppDbContext context;
        public InstructorRepositery(AppDbContext _context)
        {
            this.context = _context;
        }

        public List<ApplicationUser?>? GetStudents(string id)
        {
            var course = context?.Instructors.FirstOrDefault(x  => x.Id == id)?.CrsId;
            
            var students = context?.Enrolllments
                .Include(x => x.student)
                .ThenInclude(x => x.User)
                .Where(x => x.CrsId == course)
                .Select(x => x.student.User)
                .ToList();

            return students;
        }
        public int? GetCourseIdToInstructor(string id)
        {
            int? course = context?.Instructors.FirstOrDefault(x => x.Id == id)?.CrsId;
            return course;
        }
    }
}

public interface IInstructorRepository
{
    List<ApplicationUser?>? GetStudents(string id);
    int? GetCourseIdToInstructor(string id);
}