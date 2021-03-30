using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tblStudentComplainValidation
    {
        public int complain_id { get; set; }


        [Required(ErrorMessage = "Complain is Required")]

        public string complain_description { get; set; }
        [Required(ErrorMessage = "Reply is Required")]

        public string ReplyMsg { get; set; }

        public System.DateTime complain_date { get; set; }
        
        public string StudentName { get; set; }
        public string SchoolName { get; set; }
        public int School_Id { get; set; }
    }
}