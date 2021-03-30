using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_addTeam
    {
        public int Team_Id { get; set; }
        [Required(ErrorMessage = "Please select file.")]
        public HttpPostedFileBase UserImageFIle { get; set; }


        [Required(ErrorMessage = "The Name field is required")]


        public string Name { get; set; }
        [Required(ErrorMessage = "The Long Designation field is required")]
        public string Designation { get; set; }

        public int Department_ID { get; set; }
        [Required(ErrorMessage = "The Long Location field is required")]
        public string Short_Description { get; set; }
        [Required(ErrorMessage = "The Long Description field is required")]
        public string Long_Description { get; set; }
        [Required(ErrorMessage = "The Image field is required")]

        public string Image { get; set; }


    }
}