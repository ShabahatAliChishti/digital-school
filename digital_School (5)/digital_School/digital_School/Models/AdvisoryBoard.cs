//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace digital_School.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdvisoryBoard
    {
        public int Board_Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Department_ID { get; set; }
        public string Short_Description { get; set; }
        public string Long_Description { get; set; }
        public string Image { get; set; }
    }
}