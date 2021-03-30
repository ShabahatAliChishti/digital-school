using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Principle.Controllers
{
    public class AnnouncementController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Principle/Announcement


        public ActionResult ViewAnnouncement()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var data = db.Announcements.ToList();
            return View(data);
        }


        [HttpGet]
        public ActionResult addannouncement()
        {
            if (Session["school"] == null)
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


                int userid = Convert.ToInt32(Session["school"]);



                a.Announcement_Title = anouncement.Announcement_Title;
                a.Announcement_Body = anouncement.Announcement_Body;
                a.RoleId = 2;
                a.UserId = userid;
                a.SchoolId = userid;


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
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var data = db.Announcements.Where(x => x.Announcement_Id == id).First();


            return View(data);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateAnnouncement(Announcement dat, int id)
        {

            try
            {

                var data = db.Announcements.Where(x => x.Announcement_Id == id).First();

                if (data != null)
                {



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
            return RedirectToAction("ViewAnnouncement","Announcement");



        }
        [HttpPost]
        public JsonResult DeleteAnouncement(int announcemeID)
        {

            bool resul = false;
            Announcement sc = db.Announcements.SingleOrDefault(x => x.Announcement_Id == announcemeID);
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