using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models.Entities
{
    public class Student 
    {
        public string Id { get; set; }
        [Required]
        public ApplicationUser? User { get; set; }
        public List<Attendance>? Attendances { get; set; }
        public List<Notification>? Notifications { get; set; }
        public List<Enrolllment>? Enrolllments { get; set; }
    }
}
