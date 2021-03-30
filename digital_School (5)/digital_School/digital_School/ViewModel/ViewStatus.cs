using digital_School.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace digital_School.ViewModel
{
    public class ViewStatus
    {
        public Client CreateDate { get; set; }
        public Article schoolarticle { get; set; }
        public KidsStory schoolkidstory { get; set; }
        public KidsStoryType KidsStoryName { get; set; }

        public ArticleType Article_Typename { get; set; }
        public School School_Name { get; set; }

        public Article Clientarticle { get; set; }
        public Status statustype { get; set; }
    }
}