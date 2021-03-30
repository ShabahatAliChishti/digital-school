using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_kidStoryTypeValidation
    {
        public int KidsStoryTypeId { get; set; }
        [Required(ErrorMessage ="Kid Story Name field  Is Required")]
        public string KidsStoryName { get; set; }
    }
}