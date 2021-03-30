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
    public class TestController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Student/Test


        [HttpGet]
        public ActionResult testsubmit()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult testsubmit(tbl_uploadtestValidation tf, int id)
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



            if (tf.UserdocFIle == null)
            {
                ModelState.AddModelError("CustomError", "Please Select File");
                return View();
            }
            if (!(tf.UserdocFIle.ContentType == "application/pdf" || tf.UserdocFIle.ContentType == "application/pdf"))
            {
                ModelState.AddModelError("CustomError", "Select Doc or Pdf extention file only ");
                return View();
            }

            try
            {
                SubmitManualTest stest = new SubmitManualTest();


                string fileName = Guid.NewGuid() + Path.GetExtension(tf.UserdocFIle.FileName);
                tf.UserdocFIle.SaveAs(Path.Combine(Server.MapPath("~/FrontEnd/File_upload/ManualTest/"), fileName));

                stest.Uploadurl = fileName;
                stest.CourseId = Convert.ToInt32(TempData["id"]);
                stest.CreatedDate = DateTime.Now;
                stest.TestId = id;
                stest.SchoolId = schoolid;
                stest.UserId = Convert.ToInt32(Session["Student"]);

                db.SubmitManualTests.Add(stest);

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

        public ActionResult viewonlinetest()
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
        public ActionResult viewonlinetest(int courseId)
        {
            loginTable objuser = new loginTable();

            //Session["SName"] = userName;
            Session["CourseId"] = courseId;

            return View("ShowQuestion");

        }

        public PartialViewResult UserQuestionAnswer(CandidateAnswer objCandidateAnswer)
        {

            bool IsLast = false;


            if (objCandidateAnswer.AnswerText != null)
            {
                List<CandidateAnswer> objCandidateAnswers = Session["CadQuestionAnswer"] as List<CandidateAnswer>;

                if (objCandidateAnswers == null)
                {
                    objCandidateAnswers = new List<CandidateAnswer>();
                }

                objCandidateAnswers.Add(objCandidateAnswer);
                Session["CadQuestionAnswer"] = objCandidateAnswers;
            }
            int pageSize = 1;
            int pageNumber = 0;
            int CourseId = Convert.ToInt32(Session["CourseId"]);

            if (Session["CadQuestionAnswer"] == null)
            {
                pageNumber = pageNumber + 1;
            }
            else
            {
                List<CandidateAnswer> canAnswer = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
                pageNumber = canAnswer.Count + 1;

            }

            List<OnlineTest> listonlinetest = new List<OnlineTest>();
            listonlinetest = db.OnlineTests.Where(model => model.CourseId == CourseId).ToList();
            if (pageNumber == listonlinetest.Count)
            {
                IsLast = true;
            }


            tbl_onlinetestAnswer objAnswer = new tbl_onlinetestAnswer();
            OnlineTest objquestion = new OnlineTest();
            objquestion = listonlinetest.Skip((pageNumber - 1) * pageSize).Take(pageSize).FirstOrDefault();

            objAnswer.IsLast = IsLast;
            objAnswer.QuestionId = objquestion.QuestionId;
            objAnswer.Questionname = objquestion.QuestionName;
            objAnswer.ListOfQuizOption = (from obj in db.OnlineTestQuestionOptions
                                          where obj.QuestionId == objquestion.QuestionId
                                          select new tbl_onlinetestoption()
                                          {
                                              OptionName = obj.OptionName,
                                              OptionId = obj.OptionId
                                          }).ToList();


            return PartialView("quizquestionoption", objAnswer);
        }

        public JsonResult SaveCandidateAnswer(CandidateAnswer objcandidateAnswer)
        {
            List<CandidateAnswer> canAnswer = Session["CadQuestionAnswer"] as List<CandidateAnswer>;

            if (objcandidateAnswer.AnswerText != null)
            {
                List<CandidateAnswer> objCandidateAnswers = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
                if (objCandidateAnswers == null)
                {
                    objCandidateAnswers = new List<CandidateAnswer>();
                }
                objCandidateAnswers.Add(objcandidateAnswer);
                Session["CadQuestionAnswer"] = objCandidateAnswers;


            }



            foreach (var item in canAnswer)
            {
                OnlineTestResult result = new OnlineTestResult();
                result.AnswerText = item.AnswerText;
                result.QuestionId = item.QuestionId;

                result.UserId = Convert.ToInt32(Session["std"]);
                result.Role_Id = Convert.ToInt32(Session["RoleId"]);

                db.OnlineTestResults.Add(result);
                db.SaveChanges();
            }
            return Json(new { message = "Data Successfully Added", success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFinalResult()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            List<CandidateAnswer> listofQuestionAnswers;
            listofQuestionAnswers = Session["CadQuestionAnswer"] as List<CandidateAnswer>;
            var UserResult = (from result in listofQuestionAnswers
                              join objAnswer in db.OnlineTestAnswers on result.QuestionId equals objAnswer.QuestionId
                              join objQuestion in db.OnlineTests on result.QuestionId equals objQuestion.QuestionId

                              select new ResultModel()
                              {

                                  QuestionName = objQuestion.QuestionName,
                                  Answer = objAnswer.AnswerText,
                                  AnswerbyUser = result.AnswerText,
                                  Status = objAnswer.AnswerText == result.AnswerText ? "Correct" : "Wrong"

                              }).ToList();
            Session.Abandon();
            return View(UserResult);
        }
        public ActionResult viewuploadedtest(int? courseid)
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            if (courseid != null)
            {
                TempData["id"] = courseid;
                var data = db.ManualTests.Where(x => x.CourseId == courseid).ToList();
                return View(data);
            }
            return View();
        }

        public ActionResult viewtest()
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
        public ActionResult viewtest(tblEnrollStudentInCourseValidation testing)
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

                CourseDBHandle gc = new CourseDBHandle();

                List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
                ViewBag.course = new SelectList(list, "courseId", "courseName");


                return RedirectToAction("viewuploadedtest", new { courseid = testing.courseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please try later";
                return View();
            }



        }
    }
}