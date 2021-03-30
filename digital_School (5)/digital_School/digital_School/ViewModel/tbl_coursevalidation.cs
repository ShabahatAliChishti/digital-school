using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_coursevalidation
    {
        [Required(ErrorMessage = "Please Select Course ")]

        public int courseId { get; set; }
        public Nullable<int> Class_Id { get; set; }
        [Required(ErrorMessage = "The Code field is required")]

        public string Code { get; set; }

        [Required(ErrorMessage = "The Course Description field is required")]
        public string courseDescription { get; set; }
        [Required(ErrorMessage = "The Course Name field is required")]
        public string courseName { get; set; }

        public int User_Id { get; set; }


        [RegularExpression(@"^(https|http):\/\/(?:www\.)?youtube.com\/embed\/[A-z0-9]+", ErrorMessage = "Only Allow Embed URL like this, https://www.youtube.com/embed/jxRRFS_iRc0")]
        public string VideoLink { get; set; }
        public int Role_Id { get; set; }
        public System.DateTime CreatedDate { get; set; }

        public string imageLink { get; set; }
        [Required(ErrorMessage = "The Duration field is required")]

        public Nullable<System.DateTime> duration { get; set; }
        [Required(ErrorMessage = "The Long  Description field is required")]

        public string longDes { get; set; }
        [Required(ErrorMessage = "The Course Type field is required")]

        public string courseType { get; set; }
        [Required(ErrorMessage = "Please select file")]

        public HttpPostedFileBase CourseImageFIle { get; set; }


        public HttpPostedFileBase UserVedioFIle { get; set; }
    }
}