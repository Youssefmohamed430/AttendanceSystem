namespace AttendanceSystem.Models.Entities
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Phone { get; set; }
        public int? CrsAttendanceRate { get; set; }
        public bool? HasAttend { get; set; }
    }
}
