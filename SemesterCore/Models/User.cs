using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SemesterCore.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public ICollection<Module> Modules { get; set; }
        public ICollection<StudySession> StudySessions { get; set; }
        public SemesterInfos SemesterInfos { get; set; }
    }

}
