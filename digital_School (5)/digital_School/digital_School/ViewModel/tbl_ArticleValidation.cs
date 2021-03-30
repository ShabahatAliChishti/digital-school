using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_ArticleValidation
    {
        public int ArticleId { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.png|.jpg|.gif)$", ErrorMessage = "Only Image files allowed.")]
        public HttpPostedFileBase UserImageFIle { get; set; }

        [Required(ErrorMessage = "The Title field is required")]


        public string Title { get; set; }
        [Required(ErrorMessage = "The Image field is required")]
        public string imgPath { get; set; }
        [Required(ErrorMessage = "The Short Description field is required")]

        public string shortDes { get; set; }
        [Required(ErrorMessage = "The Long Description field is required")]

        public string longDes { get; set; }

        public int Role_Id { get; set; }
        public int UserId { get; set; }

        public int statusId { get; set; }
        [Required(ErrorMessage = "Select Article Type ")]
        public int Article_TypeId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy-MM-dd}")]
        [Required(ErrorMessage = "Date Field Is Required")]
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> views { get; set; }
    }
}