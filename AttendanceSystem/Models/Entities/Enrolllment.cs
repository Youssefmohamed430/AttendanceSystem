namespace AttendanceSystem.Models.Entities
{
    public class Enrolllment
    {
        public required string StudId { get; set; }
        public required int CrsId { get; set; }
        public int? CrsAttendanceRate { get; set; }
        public  Student? student { get; set; }
        public  Course? course { get; set; }
    }
}