using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class StudentMarksModel
    {
        public int studentId { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int TotalMarks { get; set; }
        public int MarksObtained { get; set; }
        public decimal Percentage { get; set; }
    }
}