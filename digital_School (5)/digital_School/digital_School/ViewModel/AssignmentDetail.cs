using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using digital_School.Models;

namespace digital_School.ViewModel
{
    public class AssignmentDetail
    {
        public SubmitAssignment sub { get; set; }

        public SchoolAssignment sc { get; set; }

        public School s { get; set; }
        public Course c { get; set; }

        public Student st { get; set; }
    }
}