using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_AssignmentValidation
    {
        [Required]
        public int AssignmentId { get; set; }
        [Required(ErrorMessage = "The Assignment Name Field is Required")]

        public string AssignmentName { get; set; }
        [Required]

        public int ClassId { get; set; }
        [Required]

        public int CourseId { get; set; }
        [Required]

        public string AssignmentUrl { get; set; }
        [Required]

        public System.DateTime Duration { get; set; }
        [Required]

        public int SchoolId { get; set; }
        [Required]

        public System.DateTime Date { get; set; }
        [Required(ErrorMessage = "Please select file.")]

        public HttpPostedFileBase UserdocFIle { get; set; }


    }
}