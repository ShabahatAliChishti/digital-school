using digital_School.DBHandleClass;
using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Teacher.Controllers
{
    public class OnlineTestController : Controller
    {
        // GET: Teacher/OnlineTest
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        [HttpGet]
        public ActionResult addonlinetest()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            int teacherid = Convert.ToInt32(Session["Teacher"]);
            int tempclassid;
            int originalclassid;
            var getteacherid = db.Teachers.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;


            CourseDBHandle gc = new CourseDBHandle();

            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");




            return View();
        }
        [HttpPost]
        public JsonResult addonlinetest(QuestionOptionViewModel QuestionOption)
        {
            int teacherid = Convert.ToInt32(Session["Teacher"]);

            int tempclassid;
            int schoolid;
            int originalclassid;
            var getteacherid = db.Teachers.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;


            CourseDBHandle gc = new CourseDBHandle();

            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");
            OnlineTest q = new OnlineTest();
            q.Role_Id = Convert.ToInt32(Session["RoleId"]);
            q.UserId = Convert.ToInt32(Session["Teacher"]);
            var getteachid = db.Teachers.Find(teacherid);
            schoolid = getteachid.School_Id;
            q.SchoolId = schoolid;
            //OnlineTest test = new OnlineTest();
            q.CourseId = QuestionOption.CourseId;
            q.QuestionName = QuestionOption.QuestionName;
            q.IsActive = true;
            q.ClassId = originalclassid;
            q.CreatedDate = DateTime.Now;
            q.Duration = "1 hour";
            q.IsMultiple = false;
            db.OnlineTests.Add(q);
            db.SaveChanges();

            int questionId = q.QuestionId;
            foreach (var item in QuestionOption.ListOfOptions)
            {
                OnlineTestQuestionOption objoption = new OnlineTestQuestionOption();
                objoption.OptionName = item;
                objoption.QuestionId = questionId;
                db.OnlineTestQuestionOptions.Add(objoption);
                db.SaveChanges();
            }
            OnlineTestAnswer objanswer = new OnlineTestAnswer();
            objanswer.AnswerText = QuestionOption.AnswerText;
            objanswer.RoleId = Convert.ToInt32(Session["RoleId"]);
            objanswer.UserId = Convert.ToInt32(Session["Teacher"]);
            objanswer.SchoolId = schoolid;



            objanswer.QuestionId = questionId;
            db.OnlineTestAnswers.Add(objanswer);
            db.SaveChanges();

            return Json(new { message = "Data Successfully Added", success = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult viewonlinetest()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
    }
}