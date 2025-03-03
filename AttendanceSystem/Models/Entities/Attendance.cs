namespace AttendanceSystem.Models.Entities
{
    public class Attendance
    {
        public int Id { get; set; }
        public required DateOnly Date { get; set; }
        public required bool IsPresent { get; set; }
        public required string StudId { get; set; }
        public Student? student { get; set; }
    }
}