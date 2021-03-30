using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_AssignmentSubmittedValidation
    {
        public int UploadId { get; set; }
        public int AssignmentId { get; set; }
        public int SchoolId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        [Required]
        public string UploadUrl { get; set; }
        public HttpPostedFileBase UserdocFIle { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}