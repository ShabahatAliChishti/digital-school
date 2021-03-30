using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_eventValidation
    {
        public int EventId { get; set; }
        [Required(ErrorMessage = "The Title field is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Event Start Time Title field is required")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: HH:mm tt}")]
        public System.DateTime Event_Start_Time { get; set; }

        [Required(ErrorMessage = "The Event End Time field is required")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: HH:mm tt}")]
        public System.DateTime Event_End_Time { get; set; }

        [Required(ErrorMessage = "The Event Start Date field is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        public System.DateTime Event_Start_Date { get; set; }

        [Required(ErrorMessage = "The Event End Date field is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/DD/YYYY}")]
        //[MyDate(ErrorMessage = "Back date entry not allowed")]
        //[DateGreaterThanAttribute(otherPropertyName = "Event_Start_Date", ErrorMessage = "End date must be greater than start date")]
        public System.DateTime Event_End_Date { get; set; }
        [Required(ErrorMessage = "The Event Description field is required")]
        public string Event_Description { get; set; }
        [Required(ErrorMessage = "The Event Venue field is required")]
        public string Event_Venue { get; set; }
        [Required(ErrorMessage = "The Venue Image field is required")]
        public string Event_VenueImage { get; set; }
        [Required(ErrorMessage = "Please selecct Image.")]
        public HttpPostedFileBase UserImageFIle { get; set; }
        //[RegularExpression(@"(https ?://(www\.)?youtube\.com/.*v=\w+.*)|(https?://youtu\.be/\w+.*)|(.*src=.https?://(www\.)?youtube\.com/v/\w+.*)|(.*src=.https?://(www\.)?youtube\.com/embed/\w+.*)", ErrorMessage = "Not a valid URL")]
        [RegularExpression(@"^(https|http):\/\/(?:www\.)?youtube.com\/embed\/[A-z0-9]+", ErrorMessage = "Only Allow Embed URL like this, https://www.youtube.com/embed/jxRRFS_iRc0")]
        public string Event_VenueVideo { get; set; }
            
        public HttpPostedFileBase Event_VideoFIle { get; set; }



    }
}