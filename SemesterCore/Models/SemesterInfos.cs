using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterCore.Models
{
    public class SemesterInfos
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int NumberOfWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public User User { get; set; }
    }
}
