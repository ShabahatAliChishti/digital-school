using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_uploadtestValidation
    {
        public int UploadId { get; set; }
        public int TestId { get; set; }
        public int SchoolId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Please Choose File")]
        public string Uploadurl { get; set; }
        public System.DateTime Date { get; set; }
        public HttpPostedFileBase UserdocFIle { get; set; }
    }
}