using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_kidTalentValidation
    {
        public int telentId { get; set; }
        [Required(ErrorMessage = "The Title field is required")]

        public string Title { get; set; }

        [RegularExpression(@"^(https|http):\/\/(?:www\.)?youtube.com\/embed\/[A-z0-9]+", ErrorMessage = "Only Allow Embed URL like this, https://www.youtube.com/embed/jxRRFS_iRc0")]

        public string VedioPath { get; set; }
        public string shortDes { get; set; }
        public string LongDes { get; set; }
        public Nullable<int> statusId { get; set; }
        public Nullable<int> views { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        [Required(ErrorMessage = "Date Field Is Required")]

        public System.DateTime CreatedDate { get; set; }
        public int Role_Id { get; set; }
        public int UserId { get; set; }
        public Nullable<int> SchoolId { get; set; }

    }
}