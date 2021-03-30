using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_collaborationValidation
    {
        public int CallobrationId { get; set; }
        [Required(ErrorMessage = "The Name field is Required")]
        public string CallobrationName { get; set; }
        [Required(ErrorMessage = "The Title field is Required")]
        public string CallobrationTitle { get; set; }
        [Required(ErrorMessage = "The Image field is Required")]
        public string ImageVideo { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]
        [Required(ErrorMessage = "The Date field is Required")]
        public System.DateTime Date { get; set; }
        [Required(ErrorMessage = "The Description field is Required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase CollImageFIle { get; set; }

    }
}