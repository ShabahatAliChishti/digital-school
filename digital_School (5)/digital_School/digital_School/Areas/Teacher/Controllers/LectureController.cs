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
    public class LectureController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: Teacher/Lecture
        [HttpGet]
        public ActionResult uploadLecture()
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
        public ActionResult uploadLecture(tbl_LectureValidation lec)
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

            lec.SchoolId = schoolid;
            lec.ClassId = originalclassid;
            CourseDBHandle gc = new CourseDBHandle();

            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");


            if (lec.UserdocFIle == null)
            {
                ModelState.AddModelError("CustomError", "Please Select File");
                return View();
            }
            if (!(lec.UserdocFIle.ContentType == "application/pdf" || lec.UserdocFIle.ContentType == "application/pdf"))
            {
                ModelState.AddModelError("CustomError", "Select Doc or Pdf extention file only ");
                return View();
            }

            try
            {
                Lecture teaclecture = new Lecture();

                string fileName = Guid.NewGuid() + Path.GetExtension(lec.UserdocFIle.FileName);
                lec.UserdocFIle.SaveAs(Path.Combine(Server.MapPath("~/FrontEnd/File_upload/Lecture/"), fileName));

                teaclecture.LectureUrl = fileName;
                teaclecture.Lecturename = lec.Lecturename;
                teaclecture.CourseId = lec.CourseId;
                teaclecture.ClassId = lec.ClassId;
                teaclecture.CreatedDate = DateTime.Now;
                teaclecture.Lecture_Description = lec.Lecture_Description;
                teaclecture.SchoolId = lec.SchoolId;
                teaclecture.VideoLink = lec.VideoLink;
                teaclecture.UserId = Convert.ToInt32(Session["Teacher"]);

                db.Lectures.Add(teaclecture);

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



        public ActionResult viewuploadLecture()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int teacherid = Convert.ToInt32(Session["Teacher"]);


            int schoolid;


            var getteacherid = db.Teachers.Find(teacherid);

            schoolid = getteacherid.School_Id;



            var query = (from sas in db.Lectures
                         join co in db.Courses on sas.CourseId equals co.courseId
                         where sas.SchoolId == schoolid
                         select new tbl_LectureValidation
                         {
                             LectureId = sas.LectureId,
                            CourseName = co.courseName,

                             Lecturename = sas.Lecturename,
                             Date = sas.CreatedDate,
                             VideoLink = sas.VideoLink,
                             LectureUrl = sas.LectureUrl
                         }).ToList();



            return View(query);
        }
        [HttpGet]
        public ActionResult updateLacture(int id)
        {

            using (Digital_SchoolEntities2 entities = new Digital_SchoolEntities2())
            {
                //Response.Write(submit);
                var test = db.Lectures.Find(id);
                Session["url"] = test.LectureUrl;
                return View(entities.Lectures.Single(x => x.LectureId == id));
            }

        }
        [HttpPost]


        public ActionResult updateLacture(Lecture mt, HttpPostedFileBase file)
        {
            try
            {

                if (!(file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessing" || file.ContentType == "application/msword" || file.ContentType == "application/pdf"))
                {
                    ViewBag.Message = "Select Doc or Docx or  Pdf extention file only";
                    return View();
                }
                else
                {
                    int teacherid = Convert.ToInt32(Session["Teacher"]);

                    //var model = db.ManualTests.Find(mt.TestId);
                    string oldfilePath = Session["url"].ToString();
                    if (file != null && file.ContentLength > 0)
                    {
                        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);



                        file.SaveAs(Path.Combine(Server.MapPath("~/FrontEnd/File_upload/Lecture/"), fileName));
                        mt.LectureUrl = fileName;
                        string fullPath = Request.MapPath(oldfilePath);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        mt.CreatedDate = DateTime.Now;
                        db.Entry(mt).State = System.Data.Entity.EntityState.Modified;
                        if (db.SaveChanges() > 0)

                        {
                            ViewBag.Message = "Data Updated";
                            return RedirectToAction("viewuploadLecture");
                        }
                        else
                        {
                            ViewBag.Message = "Not Valid File Type";
                            return RedirectToAction("viewuploadLecture");
                        }
                    }







                    //}
                    else
                    {
                        mt.LectureUrl = Session["url"].ToString();
                        mt.UserId = teacherid;
                        //mt.CourseId = model.CourseId;
                        //mt.ClassId = model.ClassId;
                        //mt.SchoolId = model.SchoolId;
                        mt.CreatedDate = DateTime.Now;


                        //db.Entry(mt).EntityState = EntityState.Detached;
                        db.Entry(mt).State = EntityState.Modified;
                        if (db.SaveChanges() > 0)
                        {



                            ViewBag.Message = "Data Updated";
                            return RedirectToAction("viewuploadLecture");
                        }



                    }


                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Updated";
                return View();
            }

            return View();
        }
    }
}