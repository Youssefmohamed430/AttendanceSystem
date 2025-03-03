namespace AttendanceSystem.Models.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public  List<Enrolllment>? Enrolllments { get; set; }
        public Instructor? instructor { get; set; }
    }
}