using SemesterCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterCore.Logic
{
    public static class Calculations
    {
        public static double GetSelfStudyHoursPerWeek(Module module, SemesterInfos semester)
        {
            return module.Credits * 10.0 / semester.NumberOfWeeks - module.ClassHoursPerWeek;
        }
    }
}
