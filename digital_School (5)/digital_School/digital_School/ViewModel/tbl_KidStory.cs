using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_KidStory
    {
        public int StoryId { get; set; }
        [Required(ErrorMessage = "Please Enter Title.")]

        public string StoryTitle { get; set; }
        public string imgPath { get; set; }
        [Required(ErrorMessage = "Please Enter Short Description.")]

        public string shortDes { get; set; }
        [Required(ErrorMessage = "Please select file.")]

        public HttpPostedFileBase UserImageFIle { get; set; }
        [Required(ErrorMessage = "Please Enter Long Description.")]

        public string longDes { get; set; }
        public Nullable<int> views { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]
        [Required(ErrorMessage = "Date Field Is Required")]

        public System.DateTime CreatedDate { get; set; }
        [Required]
        public int StoryTypeId { get; set; }
        public int Role_Id { get; set; }
        public int UserId { get; set; }

    }
}