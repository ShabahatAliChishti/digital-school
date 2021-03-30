using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class StudentModel
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string ExamName { get; set; }

        public string ClassName { get; set; }
        public string RegNumer { get; set; }
        public List<StudentMarksModel> ListOfStudentMarks { get; set; }
        public List<StudentModel> ListOfStudentModel { get; set; }

    }
}