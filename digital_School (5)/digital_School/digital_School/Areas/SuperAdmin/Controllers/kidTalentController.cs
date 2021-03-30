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
    public class kidTalentController : Controller
    {
        // GET: SuperAdmin/kidTalent
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        public ActionResult viewKidTalent()
        {

            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }

            var data = db.KidTalents.ToList();
            return View(data);

        }

        [HttpGet]
        public ActionResult AddkidTalent()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddkidTalent(tbl_kidTalentValidation kidtal)
        {
            try
            {
                KidTalent kid = new KidTalent();



                kid.Role_Id = Convert.ToInt32(Session["RoleId"]);
                kid.Title = kidtal.Title;
                kid.VedioPath = kidtal.VedioPath;
                kid.shortDes = kidtal.shortDes;
                kid.statusId = 2;
                kid.LongDes = kidtal.LongDes;
                kid.UserId = Convert.ToInt32(Session["Ad"]);
                kid.CreatedDate = DateTime.Now;

                db.KidTalents.Add(kid);

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
       
        [HttpPost]

        public JsonResult DeletekidTalent(int telentid)
        {

            bool resul = false;
            KidTalent sc = db.KidTalents.SingleOrDefault(x => x.telentId == telentid);
            if (sc != null)
            {
                db.KidTalents.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult updatekidTalent(int id)
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }


            var data = db.KidTalents.Where(x => x.telentId == id).First();


            return View(data);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updatekidTalent(KidTalent dat, int id)
        {

            try
            {

                var data = db.KidTalents.Where(x => x.telentId == id).First();

                if (data != null)
                {

                    data.Title = dat.Title;
                    data.VedioPath = dat.VedioPath;
                    data.shortDes = dat.shortDes;
                    data.LongDes = dat.LongDes;

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
            return RedirectToAction("viewKidTalent", "kidTalent");



        }


    }
}