using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.SuperAdmin.Controllers
{
    public class AnnouncementController : Controller
    {
        // GET: SuperAdmin/Announcement
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        [HttpGet]
        public ActionResult viewAnouncement()
        {
            var data = (from anno in db.Announcements
                        join lo in db.loginTables on anno.RoleId equals lo.RoleID
                        where anno.RoleId == 1

                        select new tbl_Announcementvalidation
                        {
                            RoleName = lo.Name,
                            Announcement_Id = anno.Announcement_Id,
                            Announcement_Title = anno.Announcement_Title,
                            Announcement_Body = anno.Announcement_Body,
                        }).ToList();

            return View(data);
        }



        [HttpGet]
        public ActionResult addannouncement()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult addannouncement(tbl_Announcementvalidation anouncement)
        {

            try
            {
                Announcement a = new Announcement();


                int userid = Convert.ToInt32(Session["Ad"]);


                a.Announcement_Title = anouncement.Announcement_Title;
                a.Announcement_Body = anouncement.Announcement_Body;
                a.RoleId = 1;
                a.UserId = userid;


                a.CreatedDate = DateTime.Now;

                db.Announcements.Add(a);

                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Data Submitted";


            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }

            return View();
        }

        [HttpGet]
       public ActionResult updateAnnouncement(int id)
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var data = db.Announcements.Where(x => x.Announcement_Id == id).First();
            return View(data);

        }
        [HttpPost]
        public ActionResult updateAnnouncement(Announcement anno , int id)
        {
        
            try
            {
                int userid = Convert.ToInt32(Session["Ad"]);

                var data = db.Announcements.Where(x => x.Announcement_Id == id).First();
                if (data != null)
                {
                    data.Announcement_Id = id;
                    data.Announcement_Title = anno.Announcement_Title;
                    data.Announcement_Body = anno.Announcement_Body;
                    data.CreatedDate = DateTime.Now;
                   data.RoleId = 1;
                    data.UserId = userid;

                    db.Entry(data).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Data Successfully Updated";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }

            return RedirectToAction("viewAnouncement", "Announcement");

        }
        [HttpPost]
        public JsonResult DeleteAnnouncement(int AnouID)
        {

            bool resul = false;
            Announcement sc = db.Announcements.SingleOrDefault(x => x.Announcement_Id == AnouID);
            if (sc != null)
            {
                db.Announcements.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }

    }
}