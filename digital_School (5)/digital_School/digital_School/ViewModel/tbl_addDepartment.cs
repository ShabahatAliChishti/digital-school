using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_addDepartment
    {
        public int Department_ID { get; set; }

        [Required(ErrorMessage = "Deparmtnet name is required")]
        public string Department_Name { get; set; }
    }
}