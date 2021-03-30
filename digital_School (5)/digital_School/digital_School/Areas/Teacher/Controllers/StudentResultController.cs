using digital_School.DBHandleClass;
using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Teacher.Controllers
{
    public class StudentResultController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: Teacher/StudentResult
        [HttpGet]
        public ActionResult addstudentresultform()
        {
            var Class = new SelectList(db.Tbl_Class.ToList(), "Class_Id", "Name");
            ViewData["Classes"] = Class;
            var Course = new SelectList(db.Courses.ToList(), "courseId", "courseName");
            ViewData["Courses"] = Course;
            var StudentRegno = new SelectList(db.Students.ToList(), "Id", "RegNo");
            ViewData["StudentRegno"] = StudentRegno;

            return View();
        }
        [HttpPost]
        public ActionResult addstudentresultform(StudentResult r)
        {
            var Class = new SelectList(db.Tbl_Class.ToList(), "Class_Id", "Name");
            ViewData["Classes"] = Class;
            var Course = new SelectList(db.Courses.ToList(), "courseId", "courseName");
            ViewData["Courses"] = Course;
            var StudentRegno = new SelectList(db.Students.ToList(), "Id", "RegNo");
            ViewData["StudentRegno"] = StudentRegno;
            return View();
        }
    }
}