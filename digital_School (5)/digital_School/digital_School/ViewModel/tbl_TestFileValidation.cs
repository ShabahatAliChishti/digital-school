using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_TestFileValidation
    {


        [Required(ErrorMessage = "Please select file.")]

        public HttpPostedFileBase UserdocFIle { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]

        public int CourseId { get; set; }
        [Required]

        public int ClassId { get; set; }
        [Required]

        public System.DateTime Duration { get; set; }
        [Required(ErrorMessage = "The test URL Field is Required")]

        public string TestUrl { get; set; }
        public int SchoolId { get; set; }
        public System.DateTime Date { get; set; }
        public int UserId { get; set; }

    }
}