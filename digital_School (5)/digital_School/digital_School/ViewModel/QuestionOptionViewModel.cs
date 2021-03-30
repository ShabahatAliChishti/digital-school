using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class QuestionOptionViewModel
    {
        public int CourseId { get; set; }
        public int ClassId { get; set; }

        public string QuestionName { get; set; }
        public string OptionName { get; set; }
        public List<string> ListOfOptions { get; set; }
        public string AnswerText { get; set; }


    }
}