using digital_School.DBHandleClass;
using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Student.Controllers
{

    public class AssignmentController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Student/Assignment
        public ActionResult assignment()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpGet]
        public ActionResult viewassignment(int? courseid)
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            if (courseid != null)
            {
                TempData["id"] = courseid;
                var data = db.SchoolAssignments.Where(x => x.CourseId == courseid).ToList();
                return View(data);
            }
            return View();
        }
        public ActionResult viewassignmentss()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int studentid = Convert.ToInt32(Session["Student"]);
            int tempclassid;
            int originalclassid;
            string Regno;

            var getstudentid = db.Students.Find(studentid);
            tempclassid = getstudentid.Class_Id;
            Regno = getstudentid.RegNo;
            var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;
            //AB enroll course wala dropdown ajayega 
            CourseDBHandle gc = new CourseDBHandle();

            List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");



            return View();
        }
        [HttpPost]
        public ActionResult viewassignmentss(tblEnrollStudentInCourseValidation testing)
        {
            try
            {
                int studentid = Convert.ToInt32(Session["Student"]);
                int tempclassid;
                int originalclassid;
                string Regno;
                var getstudentid = db.Students.Find(studentid);
                tempclassid = getstudentid.Class_Id;
                Regno = getstudentid.RegNo;
                var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
                originalclassid = classid.Class_Id;
                //AB enroll course wala dropdown ajayega 
                CourseDBHandle gc = new CourseDBHandle();

                List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
                ViewBag.course = new SelectList(list, "courseId", "courseName");


                //var data = db.SchoolAssignments.ToList();
                return RedirectToAction("viewassignment", new { courseid = testing.courseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please try later";
                return View();
            }


        }
        [HttpGet]

        public ActionResult assignmentsubmitted()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpPost]

        public ActionResult assignmentsubmitted(tbl_AssignmentSubmittedValidation SA, int id)
        {
            int studentid = Convert.ToInt32(Session["Student"]);
            int tempclassid;
            int originalclassid;
            int schoolid;
            string Regno;
            var getstudentid = db.Students.Find(studentid);
            tempclassid = getstudentid.Class_Id;
            schoolid = getstudentid.School_Id;
            Regno = getstudentid.RegNo;
            var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;



            if (SA.UserdocFIle == null)
            {
                ModelState.AddModelError("CustomError", "Please Select File");
                return View();
            }
            if (!(SA.UserdocFIle.ContentType == "application/pdf" || SA.UserdocFIle.ContentType == "application/pdf"))
            {
                ModelState.AddModelError("CustomError", "Select Doc or Pdf extention file only ");
                return View();
            }

            try
            {
                SubmitAssignment sassignemnt = new SubmitAssignment();


                string fileName = Guid.NewGuid() + Path.GetExtension(SA.UserdocFIle.FileName);
                SA.UserdocFIle.SaveAs(Path.Combine(Server.MapPath("~/FrontEnd/File_upload/Assignment/"), fileName));
                sassignemnt.UploadUrl = fileName;
                sassignemnt.CourseId = Convert.ToInt32(TempData["id"]);
                sassignemnt.CreatedDate = DateTime.Now;
                sassignemnt.AssignmentId = id;
                sassignemnt.SchoolId = schoolid;
                sassignemnt.UserId = Convert.ToInt32(Session["Student"]);

                db.SubmitAssignments.Add(sassignemnt);

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
    }
}