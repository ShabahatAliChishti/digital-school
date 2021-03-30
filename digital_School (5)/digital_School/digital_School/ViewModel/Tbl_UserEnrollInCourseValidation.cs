using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class Tbl_UserEnrollInCourseValidation
    {

        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public System.DateTime Enrolldate { get; set; }
        public Nullable<bool> IsUserActive { get; set; }
        public int UserId { get; set; }
        public string RegistrationId { get; set; }
    }
}