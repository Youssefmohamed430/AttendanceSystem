using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Models.Entities
{
    public class Instructor
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "At Least three Letters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "In Valid Email")]
        public string Email { get; set; }
        public required string Role { get; set; }
        public Course? course { get; set; }
    }
}