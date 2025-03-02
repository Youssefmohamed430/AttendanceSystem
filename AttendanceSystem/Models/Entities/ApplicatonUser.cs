using Microsoft.AspNetCore.Identity;

namespace AttendanceSystem.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public Student? student { get; set; }
        public Instructor? instructor { get; set; }
    }
}
