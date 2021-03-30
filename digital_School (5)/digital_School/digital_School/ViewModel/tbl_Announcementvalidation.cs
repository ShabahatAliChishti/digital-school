using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_Announcementvalidation
    {

        public int Announcement_Id { get; set; }
        [Required]

        public string Announcement_Title { get; set; }
        [Required]

        public string Announcement_Body { get; set; }
        [Required]

        public System.DateTime CreatedDate { get; set; }
        public string RoleName { get; set; }

        public string SchoolName { get; set; }
    }
}