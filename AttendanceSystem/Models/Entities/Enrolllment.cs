namespace AttendanceSystem.Models.Entities
{
    public class Enrolllment
    {
        public required int StudId { get; set; }
        public required int CrsId { get; set; }
        public decimal? CrsAttendanceRate { get; set; }
        public  Student? student { get; set; }
        public  Course? course { get; set; }
    }
}