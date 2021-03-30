using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class ClassSchedule
    {
        public string Code { get; set; }
        public string courseName { get; set; }
        public string Schedule { get; set; }

        public ClassSchedule(string code, string CourseName, string schedule)
        {
            Code = code;
            courseName = CourseName;
            Schedule = schedule;
        }
    }
}