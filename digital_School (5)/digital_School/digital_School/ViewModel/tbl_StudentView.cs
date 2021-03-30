using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_StudentView
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public int StudentIdtable { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int Exam_Id { get; set; }
        public string RegNo { get; set; }

        public List<StudentMarksModel> ListofStudentMarks { get; set; }

    }
}