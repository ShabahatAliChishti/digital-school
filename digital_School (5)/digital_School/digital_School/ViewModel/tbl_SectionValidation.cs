using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_SectionValidation
    {

        public int SectionID { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string SectionName { get; set; }

        public int SchoolId { get; set; }
    }
}