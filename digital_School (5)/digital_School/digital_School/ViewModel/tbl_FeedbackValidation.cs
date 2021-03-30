using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_FeedbackValidation
    {
        public int FeedbackId { get; set; }
        [Required(ErrorMessage = "Please select at least one option")]

        public int CourseId { get; set; }

        [Required]
        public string Description { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]
        public System.DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Nullable<int> SchoolId{get; set;}
        public string SchoolName { get; set; }

        public string ReplyMsg { get; set; }




    }
}