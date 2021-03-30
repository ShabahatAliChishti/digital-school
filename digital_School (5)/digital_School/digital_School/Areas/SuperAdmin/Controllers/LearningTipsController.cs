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
    public class LearningTipsController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: SuperAdmin/LearningTips
        [HttpGet]
        public ActionResult AddLearningTips()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddLearningTips(tbl_learningTipsValidation tips)
        {
            try
            {
                TechnicTip tech = new TechnicTip();




                tech.Role_Id = Convert.ToInt32(Session["RoleId"]);
                tech.Title = tips.Title;
                tech.VedioPath = tips.VedioPath;
                tech.shortDes = tips.shortDes;
                tech.statusId = 2;
                tech.longDes = tips.longDes;
                tech.UserId = Convert.ToInt32(Session["Ad"]);
                tech.CreatedDate = DateTime.Now;

                db.TechnicTips.Add(tech);

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
        public ActionResult viewLearning()
        {

            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var data = db.TechnicTips.ToList();
            return View(data);

        }
        [HttpPost]

        public JsonResult DeletelearningTips(int tipsid)
        {

            bool resul = false;
            TechnicTip sc = db.TechnicTips.SingleOrDefault(x => x.tipsId == tipsid);
            if (sc != null)
            {
                db.TechnicTips.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult updateLearningTip(int id)
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }


            var data = db.TechnicTips.Where(x => x.tipsId == id).First();


            return View(data);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateLearningTip(TechnicTip dat, int id)
        {

            try
            {

                var data = db.TechnicTips.Where(x => x.tipsId == id).First();

                if (data != null)
                {

                    data.Title = dat.Title;
                    data.VedioPath = dat.VedioPath;
                    data.shortDes = dat.shortDes;
                    data.longDes = dat.longDes;

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
            return RedirectToAction("viewLearning", "LearningTips");



        }




    }
}