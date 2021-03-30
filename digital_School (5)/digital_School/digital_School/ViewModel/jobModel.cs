using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.ViewModel
{
    public class jobModel
    {
        public HttpPostedFileBase ImageFile { get; set; }
        public int jobId { get; set; }
        public string jobTitle { get; set; }
        public string shortDescription { get; set; }

        [AllowHtml]
        public string longDescription { get; set; }
        public string imageUrl { get; set; }

        [DataType(DataType.Date)]
        public string date { get; set; }
        public int views { get; set; }
    }
}