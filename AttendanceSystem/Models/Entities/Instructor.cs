using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models.Entities
{
    public class Instructor
    {
        public string Id { get; set; }
        public Course? course { get; set; }
        public ApplicationUser? User { get; set; }

    }
}