using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models.Entities
{
    public class Student 
    {
        public int Id { get; set; }
        [Required]
        public string Role { get; set; }
        public ApplicationUser? User { get; set; }
        public List<Attendance>? Attendances { get; set; }
        public List<Enrolllment>? Enrolllments { get; set; }
    }
}
