using digital_School.DBHandleClass;
using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Teacher.Controllers
{
    public class AnnouncementController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: Teacher/Announcement
        public ActionResult viewAnnouncment()
        {
            int teacherid = Convert.ToInt32(Session["Teacher"]);


            int schoolid;


            var getteacherid = db.Teachers.Find(teacherid);

            schoolid = getteacherid.School_Id;



            var query = from sas in db.Announcements
                        join ss in db.Schools on sas.SchoolId equals ss.School_Id into table1
                        from ss in table1.DefaultIfEmpty()
                        where sas.SchoolId == schoolid
                        select new tbl_Announcementvalidation { SchoolName = ss.School_Name, Announcement_Title = sas.Announcement_Title, Announcement_Id = sas.Announcement_Id, Announcement_Body = sas.Announcement_Body };

            ViewBag.query = query;

            return View();
        }
    }
}