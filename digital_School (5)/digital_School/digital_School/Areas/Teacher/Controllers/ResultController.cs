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
    public class ResultController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: Teacher/Result
        public ActionResult GetStudentRegno()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int teacherid = Convert.ToInt32(Session["Teacher"]);
            CourseDBHandle gc = new CourseDBHandle();

            List<Tbl_UserEnrollInCourseValidation> Regnolist = gc.GetStudentRegisterationNoSimilarCourse(teacherid);
            ViewBag.Regno = new SelectList(Regnolist, "EnrollmentId", "RegistrationId");
            return View();
        }
        [HttpPost]
        public ActionResult GetStudentRegno(FormCollection formcollection)
        {
            try
            {
                int teacherid = Convert.ToInt32(Session["Teacher"]);
                CourseDBHandle gc = new CourseDBHandle();

                List<Tbl_UserEnrollInCourseValidation> Regnolist = gc.GetStudentRegisterationNoSimilarCourse(teacherid);
                ViewBag.Regno = new SelectList(Regnolist, "EnrollmentId", "RegistrationId");
                var text = formcollection["hidText"];
                return RedirectToAction("result", new { RegNo = text });

            }
            catch (Exception ex)
            {
                ViewBag.Message = "There is some problem in processing";
                return View();
            }
        }



        [HttpGet]
        public ActionResult result(string RegNo)
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            int teacherid = Convert.ToInt32(Session["Teacher"]);

            Session["Regno"] = RegNo;


            CourseDBHandle gc = new CourseDBHandle();

            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherStudentSimilarCourse(teacherid, RegNo);
            ViewBag.course = new SelectList(list, "courseId", "courseName");
            List<Exam> examlist = db.Exams.ToList();
            ViewBag.exam = new SelectList(examlist, "ExamId", "ExamName");

            var query = (from s in db.Students
                         join c in db.Tbl_Class on s.Class_Id equals c.Class_Id
                         join cs in db.Sections on s.Section_Id equals cs.SectionID
                         where s.RegNo == RegNo
                         select new StudentMasterViewModel()
                         {
                             RegNo = s.RegNo,
                             StudentIdtable = s.Id,
                             ClassId = s.Class_Id,
                             SectionId = s.Section_Id,
                             Name = s.Name,
                             ClassName = c.Name,
                             SectionName = cs.SectionName



                         }).FirstOrDefault();
            //ViewData["query"] = query;
            //StudentMasterViewModel objstudentMasterViewModel = new StudentMasterViewModel();

            //List<StudentModel> listOfStudentModel =
            //(
            //    from objStu in db.StudentMasters
            //    join objExam in db.Exams on objStu.Exam_Id equals objExam.ExamId
            //    join objclass in db.Tbl_Class on objStu.Id equals objclass.Class_Id
            //    join objReg in db.Students on objStu.Id equals objReg.Id
            //    select new StudentModel()
            //    {
            //        StudentId=objStu.Id,
            //        ClassName = objclass.Name,
            //        RegNumer = objReg.RegNo,
            //        ExamName = objExam.ExamName,
            //        Name = objStu.Name,


            //    }
            //).ToList();


            return View(query);




        }

        [HttpPost]
        public ActionResult result(tbl_StudentView objstudentViewModel)
        {

            int teacherid = Convert.ToInt32(Session["Teacher"]);
            int schoolid;
            var data = db.Teachers.Find(teacherid);
            schoolid = data.School_Id;


            string RegNo = Session["Regno"].ToString();



            CourseDBHandle gc = new CourseDBHandle();
            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherStudentSimilarCourse(teacherid, RegNo);
            ViewBag.course = new SelectList(list, "courseId", "courseName");

            StudentMaster objStudentMaster = new StudentMaster()
            {

                Name = objstudentViewModel.Name,
                ClassName = objstudentViewModel.ClassName,
                CreatedBy = teacherid,
                SectionName = objstudentViewModel.SectionName,
                Exam_Id = Convert.ToInt32(objstudentViewModel.Exam_Id),
                RegNo = objstudentViewModel.RegNo

            };
            db.StudentMasters.Add(objStudentMaster);
            db.SaveChanges();

            foreach (var item in objstudentViewModel.ListofStudentMarks)
            {
                StudentResult objstudentResult = new StudentResult()
                {

                    CourseId = item.CourseId,
                    TotalMarks = item.TotalMarks,
                    MarksObtained = item.MarksObtained,
                    SchoolId = schoolid,
                    Percentage = item.Percentage,
                    Exam_Id = Convert.ToInt32(objstudentViewModel.Exam_Id),
                    ClassId = objstudentViewModel.ClassId,
                    CreatedDate = DateTime.Now,
                    StudentMasterId = objStudentMaster.Id,
                    StudentId = objstudentViewModel.StudentIdtable,
                    SectionId = objstudentViewModel.SectionId
                };
                db.StudentResults.Add(objstudentResult);
                db.SaveChanges();
            }


            return Json(new { message = "Data Successfully Added", status = true }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult GetStudentMarks(int studentId)
        {
            List<StudentMarksModel> listofStudentMarksModel = (from obj in db.StudentResults
                                                               join objcourse in db.Courses on obj.CourseId equals objcourse.courseId
                                                               where obj.StudentResultId == studentId
                                                               select new StudentMarksModel()
                                                               {

                                                                   CourseName = objcourse.courseName,

                                                                   studentId = studentId,
                                                                   MarksObtained = obj.MarksObtained,
                                                                   TotalMarks = obj.TotalMarks,
                                                                   Percentage = obj.Percentage
                                                               }



                                                               ).ToList();

            return PartialView("_StudentMarksPartial", listofStudentMarksModel);
        }


        public JsonResult LoadStudent()
        {
            int teacherid = Convert.ToInt32(Session["Teacher"]);


            List<StudentModel> listOfStudentModel =
              (
                  from objStu in db.StudentMasters
                  join objExam in db.Exams on objStu.Exam_Id equals objExam.ExamId
                  where objStu.CreatedBy == teacherid
                  //join objclass in db.Tbl_Class on objStu.Id equals objclass.Class_Id
                  //join objReg in db.Students on objStu.Id equals objReg.Id
                  select new StudentModel()
                  {
                      ClassName = objStu.Name,
                      RegNumer = objStu.RegNo,
                      ExamName = objExam.ExamName,
                      StudentId = objStu.Id,
                      Name = objStu.Name,

                  }
              ).ToList();


            return Json(listOfStudentModel, JsonRequestBehavior.AllowGet);
        }


    }
}