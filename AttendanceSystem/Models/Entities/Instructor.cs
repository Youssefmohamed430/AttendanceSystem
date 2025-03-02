using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        public required string Role { get; set; }
        public Course? course { get; set; }
        public ApplicationUser? User { get; set; }

    }
}