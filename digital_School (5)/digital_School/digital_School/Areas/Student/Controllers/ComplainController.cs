using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Student.Controllers
{
    public class ComplainController : Controller
    {
        // GET: Student/Complain
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        [HttpGet]
        public ActionResult addComplainform()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult addComplainform(tblStudentComplainValidation complain)
        {
            int studentid = Convert.ToInt32(Session["Student"]);


            int schoolid;


            var getstudentid = db.Students.Find(studentid);

            schoolid = getstudentid.School_Id;


            complain.School_Id = schoolid;





            StudentComplain stdcomplain = new StudentComplain();
            stdcomplain.complain_description = complain.complain_description;
            stdcomplain.complain_date = DateTime.Now;
            stdcomplain.School_Id = complain.School_Id;
            stdcomplain.UserId = studentid;

            db.StudentComplains.Add(stdcomplain);
            db.SaveChanges();

            ViewBag.Message = "Data Submitted";

            ModelState.Clear();


            return View();
        }
        public ActionResult viewComplainform()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int studentid = Convert.ToInt32(Session["Student"]);


            int schoolid;


            var getstudentid = db.Students.Find(studentid);

            schoolid = getstudentid.School_Id;





            var data = (from com in db.StudentComplains
                        join sc in db.Schools on com.School_Id equals sc.School_Id
                        where com.School_Id == schoolid
                        select new tblStudentComplainValidation
                        {
                            complain_id = com.complain_id,
                            SchoolName = sc.School_Name,

                            complain_description = com.complain_description,
                            ReplyMsg = com.ReplyMsg,


                        }).ToList();


            return View(data);
        }
    }
}