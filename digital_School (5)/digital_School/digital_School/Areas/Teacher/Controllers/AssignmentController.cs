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
    public class AssignmentController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: Teacher/Assignment
        [HttpGet]
        public ActionResult assignment()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int teacherid = Convert.ToInt32(Session["Teacher"]);

            int tempclassid;
            int schoolid;
            int originalclassid;

            var getteacherid = db.Teachers.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            schoolid = getteacherid.School_Id;
            var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;
            CourseDBHandle gc = new CourseDBHandle();

            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");
            return View();
        }
        [HttpPost]

        public ActionResult assignment(tbl_AssignmentValidation a)
        {

            int teacherid = Convert.ToInt32(Session["Teacher"]);

            int tempclassid;
            int schoolid;
            int originalclassid;

            var getteacherid = db.Teachers.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            schoolid = getteacherid.School_Id;
            var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;

            a.SchoolId = schoolid;
            a.ClassId = originalclassid;
            CourseDBHandle gc = new CourseDBHandle();

            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");


            if (a.UserdocFIle == null)
            {
                ModelState.AddModelError("CustomError", "Please Select File");
                return View();
            }
            if (!(a.UserdocFIle.ContentType == "application/pdf" || a.UserdocFIle.ContentType == "application/pdf"))
            {
                ModelState.AddModelError("CustomError", "Select Doc or Pdf extention file only ");
                return View();
            }

            try
            {
                SchoolAssignment scassignemnt = new SchoolAssignment();


                string fileName = Guid.NewGuid() + Path.GetExtension(a.UserdocFIle.FileName);
                a.UserdocFIle.SaveAs(Path.Combine(Server.MapPath("~/FrontEnd/File_Upload/Assignment/"), fileName));

                scassignemnt.AssignmentUrl = fileName;
                scassignemnt.AssignmentName = a.AssignmentName;
                scassignemnt.CourseId = a.CourseId;
                scassignemnt.ClassId = a.ClassId;
                scassignemnt.CreatedDate = DateTime.Now;
                scassignemnt.Duration = a.Duration;
                scassignemnt.SchoolId = a.SchoolId;
                scassignemnt.UserId = Convert.ToInt32(Session["Teacher"]);

                db.SchoolAssignments.Add(scassignemnt);

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







        public ActionResult viewassignment(int? courseid)
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            Digital_SchoolEntities2 entities = new Digital_SchoolEntities2();
            List<SubmitAssignment> sa = entities.SubmitAssignments.ToList();
            List<School> s = entities.Schools.ToList();
            List<SchoolAssignment> sca = entities.SchoolAssignments.ToList();
            List<Course> c = entities.Courses.ToList();
            var query = from sas in sa
                        join ss in s on sas.SchoolId equals ss.School_Id into table1
                        from ss in table1.DefaultIfEmpty()
                        join sss in sca on sas.AssignmentId equals sss.AssignmentId into table2
                        from sss in table2.DefaultIfEmpty()
                        join ssss in c on sas.CourseId equals ssss.courseId into table3
                        from ssss in table3.DefaultIfEmpty()
                        select new AssignmentDetail { sub = sas, s = ss, sc = sss, c = ssss };
            return View(query);

        }

    }
}