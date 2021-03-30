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
    public class ComplainController : Controller
    {
        // GET: Principle/Complain
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        [HttpGet]
        public ActionResult addComplainform()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult addComplainform(tblClientorSchoorComplainValidation complain)
        {
            complain.RoleId = Convert.ToInt32(Session["RoleId"]);


            complain.UserId = Convert.ToInt32(Session["school"]);


            Client_SchoolComplain cscomplain = new Client_SchoolComplain();
            cscomplain.complain_description = complain.complain_description;
            cscomplain.complain_date = DateTime.Now;
            cscomplain.RoleId = complain.RoleId;
            cscomplain.UserId = complain.UserId;
            db.Client_SchoolComplain.Add(cscomplain);
            db.SaveChanges();

            ViewBag.Message = "Data Submitted";

            ModelState.Clear();

            return View();
        }



        public ActionResult viewComplainform()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var UserId = Convert.ToInt32(Session["school"]);


            var data = (from com in db.Client_SchoolComplain
                        join sc in db.Schools on com.UserId equals sc.School_Id
                        where com.UserId == UserId
                        select new tblClientorSchoorComplainValidation
                        {
                            complain_Id = com.complain_Id,
                            ScholName = sc.School_Name,
                            UserId = UserId,
                            complain_description = com.complain_description,
                            replymsg = com.replymsg,
                            complain_date = com.complain_date,


                        }).ToList();

            //var data = db.Client_SchoolComplain.ToList();



            return View(data);
        }




        public ActionResult detailcomplainform()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }


        public ActionResult viewStudentComplainform()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            int schoolid = Convert.ToInt32(Session["school"]);
            var data = (from complain in db.StudentComplains
                        join sc in db.Students on complain.UserId equals sc.Id
                        join scoo in db.Schools on complain.School_Id equals scoo.School_Id
                        where complain.School_Id == schoolid
                        select new tblStudentComplainValidation()
                        {
                            complain_id = complain.complain_id,
                            StudentName = sc.Name,
                            SchoolName = scoo.School_Name,
                            complain_description = complain.complain_description,
                            ReplyMsg = complain.ReplyMsg,
                            complain_date = complain.complain_date

                        }).ToList();



            return View(data);
        }

        [HttpGet]
        public ActionResult viewStudentComplainReply()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }


            return View();
        }
        [HttpPost]
        public ActionResult viewStudentComplainReply(tblStudentComplainValidation sa, int id)
        {
            try
            {


                int user = Convert.ToInt32((Session["school"]));

                var com = db.StudentComplains.FirstOrDefault(m => m.complain_id == id);

                if (com != null)
                {
                    
                    com.ReplyMsg = sa.ReplyMsg;

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
            return RedirectToAction("viewStudentComplainform");
        }



    }
}