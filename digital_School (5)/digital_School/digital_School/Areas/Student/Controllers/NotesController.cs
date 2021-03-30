using digital_School.DBHandleClass;
using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Student.Controllers
{

    public class NotesController : Controller
    {
        // GET: Student/Lecture
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        [HttpGet]
        public ActionResult notesview(int? courseid)
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            if (courseid != null)
            {
                TempData["id"] = courseid;
                var data = db.Lectures.Where(x => x.CourseId == courseid).ToList();
                return View(data);
            }
            return View();
        }

        public ActionResult notesviewer()
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

            CourseDBHandle gc = new CourseDBHandle();

            List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");



            return View();
        }
        [HttpPost]
        public ActionResult notesviewer(tblEnrollStudentInCourseValidation testing)
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

            CourseDBHandle gc = new CourseDBHandle();

            List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");


            return RedirectToAction("notesview", new { courseid = testing.courseId });


        }
    }
}