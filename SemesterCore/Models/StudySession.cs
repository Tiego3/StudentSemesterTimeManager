using System;

namespace SemesterCore.Models
{
    public class StudySession
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public Module Module { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
    }
}
