using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class StudentMasterViewModel
    {
        public int Id { get; set; }
        public int StudentIdtable { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }



        public string Name { get; set; }
        public string ClassName { get; set; }
        [Display(Name = "Exam")]
        public int ExamId { get; set; }
        [Display(Name = "Course")]

        public int CourseId { get; set; }
        [Display(Name = "Section")]

        public string SectionName { get; set; }
        [Display(Name = "Roll Number")]

        public string RegNo { get; set; }

        public int TotalMarks { get; set; }
        public int MarksObtained { get; set; }

        public List<StudentModel> ListOfStudentModel { get; set; }



    }
}