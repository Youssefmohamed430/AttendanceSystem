using AttendanceSystem.Models.Entities;

namespace AttendanceSystem.Views.ViewModels
{
    public class ProfileViewModell : StudentViewModel
    {
        public List<Enrolllment> enrolllments { get; set; }
    }
}
