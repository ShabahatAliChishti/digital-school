using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class tbl_onlinetestAnswer
    {
        public bool IsLast { get; set; }
        public int QuestionId { get; set; }

        public int OptionId { get; set; }
        public string Questionname { get; set; }
        public List<tbl_onlinetestoption> ListOfQuizOption { get; set; }
    }
}