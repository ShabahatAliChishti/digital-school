using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tblClientorSchoorComplainValidation
    {

        public int complain_Id { get; set; }
        [Required(ErrorMessage = "Complain is Required")]
        public string complain_description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddThh:mm:ss}")]

        public System.DateTime complain_date { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Reply is Required")]
        public string replymsg { get; set; }
        public int RoleId { get; set; }

        public string ScholName { get; set; }
        public string Name { get; set; }
    }
}