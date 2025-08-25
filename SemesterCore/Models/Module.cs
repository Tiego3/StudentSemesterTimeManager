using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterCore.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Credits { get; set; }
        public int ClassHoursPerWeek { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<StudySession> StudySessions { get; set; }
    }
}
