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
    public class SchoolAndClientComplainController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: SuperAdmin/SchoolAndClientComplain
        [HttpGet]
        public ActionResult virewSchoolComplain()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var data = (from complain in db.Client_SchoolComplain
                        join sc in db.Schools on complain.UserId equals sc.School_Id
                        where complain.RoleId == 2


                        select new tbl_ClientorSchoorComplainValidation
                        {

                            complain_Id = complain.complain_Id,
                            Name = sc.School_Name,
                            complain_description = complain.complain_description,
                            complain_date = complain.complain_date

                        }).ToList();

            //var data = db.Client_SchoolComplain.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult SchoolComplainReply()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }


            return View();
        }
        [HttpPost]
        public ActionResult SchoolComplainReply(tbl_ClientorSchoorComplainValidation sa, int id)
        {
            try
            {
                int userid = Convert.ToInt32((Session["Ad"]));

                var com = db.Client_SchoolComplain.FirstOrDefault(m => m.complain_Id == id);

                if (com != null)
                {
                    com.UserId = userid;
                    com.RoleId = 2;
                    com.complain_Id = id;
                    com.replymsg = sa.replymsg;

                    db.Entry(com).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Data Successfully Submitted";
                    ModelState.Clear();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }
            return View();
        }








        //Start Client Complain
        [HttpGet]
        public ActionResult viewClientComplain()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var data = (from complain in db.Client_SchoolComplain
                        join sc in db.Clients on complain.UserId equals sc.UserId
                        where complain.RoleId == 5


                        select new tbl_ClientorSchoorComplainValidation
                        {

                            complain_Id = complain.complain_Id,
                            Name = sc.UserName,
                            complain_description = complain.complain_description,
                            complain_date = complain.complain_date

                        }).ToList();

            //var data = db.Client_SchoolComplain.ToList();
            return View(data);
        }
        public ActionResult ClientComplainReply()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }


            return View();
        }
        [HttpPost]
        public ActionResult ClientComplainReply(tbl_ClientorSchoorComplainValidation sa, int id)
        {
            try
            {
                int userid = Convert.ToInt32((Session["Ad"]));

                var com = db.Client_SchoolComplain.FirstOrDefault(m => m.complain_Id == id);

                if (com != null)
                {
                    com.UserId = userid;
                    com.RoleId = 5;
                    com.complain_Id = id;
                    com.replymsg = sa.replymsg;

                    db.Entry(com).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.Message = "Data Successfully Submitted";
                    ModelState.Clear();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }
            return View();
        }


    }
}