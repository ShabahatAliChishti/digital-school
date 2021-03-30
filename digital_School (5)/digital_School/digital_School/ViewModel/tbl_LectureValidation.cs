using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_LectureValidation
    {
        [Required]
        public int LectureId { get; set; }
        [Required(ErrorMessage = "The Lecture Name  Field is Required")]

        public string Lecturename { get; set; }
        [Required]

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        [Required]

        public int ClassId { get; set; }
        [Required]

        public string LectureUrl { get; set; }
        [Required]

        public int SchoolId { get; set; }
        [Required]

        public System.DateTime Date { get; set; }
        [Required]

        public int UserId { get; set; }

        public string VideoLink { get; set; }

        [Required(ErrorMessage = "Please select file.")]

        public HttpPostedFileBase UserdocFIle { get; set; }

        [Required]
        public string Lecture_Description { get; set; }

    }
}