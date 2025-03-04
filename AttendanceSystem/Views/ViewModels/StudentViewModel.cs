using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Views.ViewModels
{
    public class StudentViewModel
    {

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
        [MinLength(10, ErrorMessage = "At Least ten Letters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username must contain only letters and numbers")]
        public string UserName { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "At Least ten Letters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "password must contain only letters and numbers")]
        public string password { get; set; }
    }
}
