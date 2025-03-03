using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AttendanceSystem.Views.ViewModels
{
    public class LogInViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe {  get; set; }
    }
}
