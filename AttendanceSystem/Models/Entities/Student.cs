using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models.Entities
{
    public class Student
    {
        public  int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "At Least three Letters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone must contain numbers only")]
        public string Phone { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "In Valid Email")]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
        public List<Attendance>? Attendances { get; set; }
        public List<Enrolllment>? Enrolllments { get; set; }
    }
}
