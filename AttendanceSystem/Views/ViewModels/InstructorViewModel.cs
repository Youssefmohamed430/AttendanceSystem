using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Views.ViewModels
{
    public class InstructorViewModel
    {

        [Required]
        [MinLength(3, ErrorMessage = "At Least three Letters")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "In Valid Email")]
        public string Email { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = "At Least ten Letters")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username must contain only letters and numbers")]
        public string UserName { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "At Least Eight Letters")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "password must contain only letters")]
        public string password { get; set; }
        [Required]
        [Compare("password",ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string Confirmpassword { get; set; }
        public int Courseid { get; set; }
    }
}
