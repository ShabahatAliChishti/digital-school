using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Student.Controllers
{
    public class AnnouncementController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Student/Announcement
        public ActionResult viewAnnouncment()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int studentid = Convert.ToInt32(Session["Student"]);


            int schoolid;


            var getstudentid = db.Students.Find(studentid);

            schoolid = getstudentid.School_Id;



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