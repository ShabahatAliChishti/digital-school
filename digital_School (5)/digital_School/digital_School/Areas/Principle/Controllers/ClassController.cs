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
    public class ClassController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Principle/Class
        [HttpGet]
        public ActionResult addClass()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult addClass(tbl_ClassValidation cv)
        {
            try
            {

                Tbl_Class c = new Tbl_Class();

                c.SchoolId = Convert.ToInt32(Session["school"]);
                c.Name = cv.Name;
                db.Tbl_Class.Add(c);

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


        public ActionResult viewsClass()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var id = Convert.ToInt32(Session["school"]);


            var data = db.Tbl_Class.Where(x => x.SchoolId == id).ToList();
            return View(data);
        }







        [HttpGet]
        public ActionResult updateClass(int id)
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var data = db.Tbl_Class.Where(x => x.Class_Id == id).First();


            return View(data);

        }

        [HttpPost]

        public ActionResult updateClass(Tbl_Class dat, int id)
        {

            try
            {

                var data = db.Tbl_Class.Where(x => x.Class_Id == id).First();

                if (data != null)
                {

                    data.Name = dat.Name;

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
            return RedirectToAction("viewsClass", "Class");



        }

        [HttpPost]
        public JsonResult DeleteClass(int classID)
        {

            bool resul = false;
            Tbl_Class sc = db.Tbl_Class.SingleOrDefault(x => x.Class_Id == classID);
            if (sc != null)
            {
                db.Tbl_Class.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }


    }
}