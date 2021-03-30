using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace digital_School.ViewModel
{
    public class tbl_schoolValidation
    {

        [Required(ErrorMessage ="The School Name field is required ")]
        public string School_Name { get; set; }
        [Required(ErrorMessage = "The School Image field is required ")]
        public string School_Image { get; set; }
        [Required(ErrorMessage = "The Contact Number field is required ")]
        public string School_Contactno { get; set; }
        [Required(ErrorMessage = "The School Address field is required ")]
        public string School_Address { get; set; }
        [Required(ErrorMessage = "The Date field is required ")]
        public System.DateTime CreatedOn { get; set; }
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string School_Email { get; set; }
        //public int UserId { get; set; }
        //[Required(ErrorMessage = "The Password field is required ")]
        [Required(ErrorMessage = "Password is required.")]
        [MembershipPassword(
        MinRequiredNonAlphanumericCharacters = 1,
        MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
        ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).",
        MinRequiredPasswordLength = 6
        )]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Select File")]

        public HttpPostedFileBase UserImageFIle { get; set; }

    }
    //[MetadataType(typeof(tbl_schoolValidation))]
    //public partial class School
    //{

    //}
}