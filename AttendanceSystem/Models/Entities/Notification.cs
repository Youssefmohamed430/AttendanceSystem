﻿namespace AttendanceSystem.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public Student student { get; set; }
        public string Message { get; set; }
        public bool? IsRead { get; set; } = false;
        public DateTime DateSent { get; set; } = DateTime.Now;
    }
}
