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
    public class FeedbackController : Controller
    {
        // GET: Student/Feedback
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        [HttpGet]
        public ActionResult addfeedback()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            try
            {
                int studentid = Convert.ToInt32(Session["Student"]);
                int tempclassid;
                int schoolid;
                int originalclassid;
                string Regno;
                var getstudentid = db.Students.Find(studentid);
                tempclassid = getstudentid.Class_Id;
                schoolid = getstudentid.School_Id;
                Regno = getstudentid.RegNo;
                var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
                originalclassid = classid.Class_Id;

                CourseDBHandle gc = new CourseDBHandle();

                List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
                ViewBag.course = new SelectList(list, "courseId", "courseName");
                //ViewBag.Product_CategoryID = new SelectList(db.Courses, "courseId", "courseName");
                return View();

            }
            catch (Exception ex)
            {
                ViewBag.Message = "There is some error in processing";
                return View();
            }

        }
        [HttpPost]
        public ActionResult addfeedback(tbl_FeedbackValidation fv)
        {
            try
            {
                int studentid = Convert.ToInt32(Session["Student"]);
                int tempclassid;
                int schoolid;
                int originalclassid;
                string Regno;
                var getstudentid = db.Students.Find(studentid);
                tempclassid = getstudentid.Class_Id;
                schoolid = getstudentid.School_Id;
                Regno = getstudentid.RegNo;
                var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
                originalclassid = classid.Class_Id;

                fv.SchoolId = schoolid;
                CourseDBHandle gc = new CourseDBHandle();

                List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
                ViewBag.course = new SelectList(list, "courseId", "courseName");
                UserFeedback f = new UserFeedback();
                f.CourseId = fv.CourseId;
                f.Description = fv.Description;
                f.CreatedDate = DateTime.Now;
                f.UserId = Convert.ToInt32(Session["Student"]);
                f.RoleId = 4;
                f.SchoolId = fv.SchoolId;

                db.UserFeedbacks.Add(f);
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

        public ActionResult viewfeedback()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            int studentid = Convert.ToInt32(Session["Student"]);


            int schoolid;


            var getstudentid = db.Students.Find(studentid);

            schoolid = getstudentid.School_Id;





            var data = (from com in db.UserFeedbacks
                        join sc in db.Schools on com.SchoolId equals sc.School_Id
                        where com.SchoolId == schoolid
                        select new tbl_FeedbackValidation
                        {
                            FeedbackId = com.FeedbackId,
                            SchoolName = sc.School_Name,

                            Description= com.Description,
                            


                        }).ToList();


            return View(data);
        }
    }
}