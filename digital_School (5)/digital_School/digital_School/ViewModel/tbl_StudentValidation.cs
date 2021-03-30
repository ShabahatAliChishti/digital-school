using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace digital_School.ViewModel
{
    public class tbl_StudentValidation
    {
        public int Id { get; set; }
        [Required]
        public string RegNo { get; set; }
        [Required]

        public string Name { get; set; }
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string Email { get; set; }
        [Required]

        public string ContactNo { get; set; }
        [Required]

        public DateTime RegisterationDate { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]

        public int Class_Id { get; set; }
        [Required]

        public int School_Id { get; set; }
        [Required]

        public int User_Id { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MembershipPassword(
            MinRequiredNonAlphanumericCharacters = 1,
            MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
            ErrorMessage = "Your password must be 6 characters long and contain at least one symbol (!, @, #, etc).",
            MinRequiredPasswordLength = 6
            )]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required]

        public string Image { get; set; }

        [Required]

        public int Section_Id { get; set; }
        [Required]

        public HttpPostedFileBase UserImageFIle { get; set; }


    }
}