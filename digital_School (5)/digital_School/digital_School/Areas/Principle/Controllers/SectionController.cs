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
    public class SectionController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Principle/Section
        [HttpGet]
        public ActionResult addSection()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult addSection(tbl_SectionValidation sv)
        {
            try
            {
                Section s = new Section();

                s.SchoolId = Convert.ToInt32(Session["school"]);
                s.SectionName = sv.SectionName;
                db.Sections.Add(s);

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
        public ActionResult viewsSection()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var id = Convert.ToInt32(Session["school"]);


            var data = db.Sections.Where(x => x.SchoolId == id).ToList();
            return View(data);
        }


        [HttpGet]
        public ActionResult updateSection(int id)
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var data = db.Sections.Where(x => x.SectionID == id).First();


            return View(data);

        }

        [HttpPost]

        public ActionResult updateSection(Section dat, int id)
        {

            try
            {

                var data = db.Sections.Where(x => x.SectionID == id).First();

                if (data != null)
                {

                    data.SectionName = dat.SectionName;

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
            return RedirectToAction("viewsSection", "Section");



        }

        [HttpPost]
        public JsonResult DeleteSection(int secID)
        {

            bool resul = false;
            Section sc = db.Sections.SingleOrDefault(x => x.SectionID == secID);
            if (sc != null)
            {
                db.Sections.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }


    }
}