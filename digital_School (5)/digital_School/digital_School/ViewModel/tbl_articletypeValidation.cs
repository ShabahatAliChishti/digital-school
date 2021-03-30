using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_articletypeValidation
    {
        public int Article_TypeId { get; set; }
        [Required(ErrorMessage ="Article Type is Required")]
        public string Article_Typename { get; set; }

    }
}