using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_OnlineTestValidation
    {

        [Required(ErrorMessage = "Please Select Course ")]

        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please Enter Question ")]

        public string QuestionName { get; set; }
        public string OptionName { get; set; }
        public string courseName { get; set; }
        [Required(ErrorMessage = "Please Select Class ")]
        public int ClassId { get; set; }

        public int SchoolId { get; set; }
    }
}