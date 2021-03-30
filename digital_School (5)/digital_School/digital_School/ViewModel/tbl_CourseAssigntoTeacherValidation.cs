using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_CourseAssigntoTeacherValidation
    {
        public int Assign_Id { get; set; }
        [Required(ErrorMessage = "Please Select Class ")]

        public int Class_Id { get; set; }
        [Required]

        public int UserId { get; set; }
        [Required(ErrorMessage = "Please Select Course ")]

        public int courseId { get; set; }
        public string courseName { get; set; }
        public bool IsActive { get; set; }
        public int School_Id { get; set; }
    }
}