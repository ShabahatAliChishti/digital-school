using digital_School.DBHandleClass;
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
    public class CourseController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Principle/Course
        [HttpGet]
        public ActionResult AddCourse()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewBag.school = new SelectList(list, "Class_Id", "Name");

            return View();
        }
        [HttpPost]
        public ActionResult AddCourse(tbl_coursevalidation cou)
        {
            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewBag.school = new SelectList(list, "Class_Id", "Name");
            Course cc = new Course();

            try
            {


                cou.Role_Id = Convert.ToInt32(Session["RoleID"]);

                cc.User_Id = schoolid;
                cc.Role_Id = cou.Role_Id;
                cc.CreatedDate = DateTime.Now;
                cc.courseDescription = cou.courseDescription;
                cc.courseName = cou.courseName;
                cc.courseType = cou.courseType;
                cc.Class_Id = cou.Class_Id;
                cc.Code = cou.Code;
                //cc.longDes = cou.longDes;
                //cc.duration = cou.duration;
                db.Courses.Add(cc);


                db.SaveChanges();
                ViewBag.Message = "Data Submitted";
                ModelState.Clear();
            }

            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }

            return View();

        }

        public ActionResult courseAsssignation()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            //Tbl_Class c = new Tbl_Class();
            //ViewBag.schoolclass = db.Tbl_Class.ToList();


            int schoolid = Convert.ToInt32(Session["school"]);

            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);



            ViewData["class_name"] = new SelectList(list, "Class_Id", "Name");


            return View();
        }
        [HttpPost]

        public ActionResult courseAsssignation(CourseAssignToTeacher c)
        {

            int schoolid = Convert.ToInt32(Session["school"]);

            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);


            ViewData["class_name"] = new SelectList(list, "Class_Id", "Name");


            return View();
        }




        public ActionResult coursestatistic()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewData["class_name"] = new SelectList(list, "Class_Id", "Name");
            return View();
        }

        public ActionResult Enrollment()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            //ViewBag.CourseId = new SelectList(db.Courses, "courseId", "Code");
            //ViewBag.StudentList = db.Students.ToList();
            int schoolid = Convert.ToInt32(Session["school"]);
            int RoleID = 2;
            StudentDBHandle gc = new StudentDBHandle();

            List<tbl_StudentValidation> list = gc.GetSchoolStudent(schoolid);

            ViewData["student_name"] = new SelectList(list, "RegNo", "RegNo");


            CourseDBHandle gc1 = new CourseDBHandle();

            List<tbl_OnlineTestValidation> courselist = gc1.GetSchoolCourse(schoolid, RoleID);

            ViewData["course_name"] = new SelectList(courselist, "courseId", "courseName");
            return View();



        }

        [HttpPost]
        public ActionResult Action(string RegNo)
        {

            var query = from s in db.Students
                        join c in db.Tbl_Class on s.Class_Id equals c.Class_Id
                        join cs in db.Sections on s.Section_Id equals cs.SectionID
                        where s.RegNo == RegNo
                        select new { Id = s.RegNo, Name = s.Name, Email = s.Email, Class = c.Name, Section = cs.SectionName };
            //var query = from c in db.Students where c.Id == Id select new { Id = c.Id, Name = c.Name, Email = c.Email };
            return Json(query, JsonRequestBehavior.AllowGet);
        }


        public JsonResult IsEnrolled(string regNo, int courseId)
        {
            var enrollCourses = db.UserEnrollInCourses.Where(s => s.RegistrationId == regNo && s.CourseId == courseId);
            int itm = enrollCourses.Count();
            if (itm == 0)
            {
                return Json(false);
            }
            return Json(true);
        }

        public ActionResult EnrollCoursetoStudent(UserEnrollInCourse enrollCourse)
        {
            var enrollCourses = db.UserEnrollInCourses.Where(s => s.RegistrationId == enrollCourse.RegistrationId && s.CourseId == enrollCourse.CourseId).ToList();
            int itm = enrollCourses.Count();
            if (itm == 1)
            {
                int schoolid = Convert.ToInt32(Session["school"]);
                var id = enrollCourses[0].EnrollmentId;
                var date = DateTime.Now;
                enrollCourse.CourseId = id;
                enrollCourse.Enrolldate = date;
                enrollCourse.UserId = schoolid;
                enrollCourse.RoleId = 2;
                enrollCourse.IsUserActive = true;
                db.UserEnrollInCourses.Add(enrollCourse);
                db.SaveChanges();

            }
            else
            {
                int schoolid = Convert.ToInt32(Session["school"]);
                var date = DateTime.Now;
                enrollCourse.UserId = schoolid;
                enrollCourse.RoleId = 2;
                enrollCourse.IsUserActive = true;
                enrollCourse.Enrolldate = date;
                db.UserEnrollInCourses.Add(enrollCourse);
                db.SaveChanges();

            }

            return Json(true);
        }

        public JsonResult GetTeachersByDeptId(int deptId)
        {
            var teachers = db.Teachers.Where(t => t.Class_Id == deptId).Select(Class_Id => new { Class_Id.Id, Class_Id.Name }).ToList();

            return Json(teachers, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCoursesByDeptId(int deptId)
        {
            var courses = db.Courses.Where(t => t.Class_Id == deptId).Select(x => new { x.courseId, x.Code }).ToList();
            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourseById(int courseId)
        {


            var courses = db.Courses.Where(t => t.courseId == courseId).Select(x => new { x.courseId, x.courseName });
            return Json(courses, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveAssignCourse(CourseAssignToTeacher assignCourse)
        {
            var asignCoursesList = db.CourseAssignToTeachers.Where(t => t.courseId == assignCourse.courseId && t.Course.Status == true).ToList();
            if (asignCoursesList.Count > 0)
            {

                return Json(false);
            }
            else

            {

                int schoolid = Convert.ToInt32(Session["school"]);
                assignCourse.School_Id = schoolid;

                db.CourseAssignToTeachers.Add(assignCourse);

                db.SaveChanges();


                var teacher = db.Teachers.FirstOrDefault(t => t.Id == assignCourse.TeacherId);


                var course = db.Courses.FirstOrDefault(t => t.courseId == assignCourse.courseId);
                if (course != null)
                {
                    course.Status = true;
                    course.AssignTo = teacher.Name;
                    db.Entry(course).State = EntityState.Modified;

                    //db.Courses.Add(course);
                    db.SaveChanges();
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }

                //return Json(false);
            }
        }


        public ActionResult CourseInfo(int Class_Id)
        {
            try
            {
                var courses = db.Courses.Where(t => t.Class_Id == Class_Id).Select(x => new { x.courseId, x.Code, x.AssignTo, x.courseName }).ToList();
                return Json(courses, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Try later";
                return View();
            }
        }


        public ActionResult UnassignCourses()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }

        public JsonResult UnassignAllCourses(bool name)
        {
            int schoolid = Convert.ToInt32(Session["school"]);

            var courses = db.Courses.Where(c => c.Status == true && c.User_Id == schoolid).ToList();
            if (courses.Count == 0)
            {
                return Json(false);
            }
            else
            {
                foreach (var course in courses)
                {
                    course.Status = false;
                    course.AssignTo = "";
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Json(true);

            }
        }



    }
}
    
