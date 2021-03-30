using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_clienteFeedback
    {
        public int FeedbackId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Rating { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]

        public System.DateTime Date { get; set; }
        public int UserId { get; set; }

        public string clientname { get; set; }
    }
}