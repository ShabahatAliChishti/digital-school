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
    public class ManualTestController : Controller
    {
        // GET: Teacher/ManualTest
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        [HttpGet]
        public ActionResult addtestfile()
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
        public ActionResult addtestfile(tbl_TestFileValidation tfile)
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
            CourseDBHandle gc = new CourseDBHandle();
            tfile.SchoolId = schoolid;
            tfile.ClassId = originalclassid;
            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");


            if (tfile.UserdocFIle == null)
            {
                ModelState.AddModelError("CustomError", "Please Select File");
                return View();
            }
            if (!(tfile.UserdocFIle.ContentType == "application/msword" || tfile.UserdocFIle.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document" || tfile.UserdocFIle.ContentType == "application/pdf"))
            {
                ViewBag.Message= "Select  doc or Docx Pdf extention file only ";
                return View();
            }

            try
            {
                ManualTest mantestfile = new ManualTest();

                string fileName = Guid.NewGuid() + Path.GetExtension(tfile.UserdocFIle.FileName);
                tfile.UserdocFIle.SaveAs(Path.Combine(Server.MapPath("~/FrontEnd/File_Upload/ManualTest/"), fileName));


                mantestfile.TestUrl = fileName;
                mantestfile.CourseId = tfile.CourseId;
                mantestfile.ClassId = tfile.ClassId;
                mantestfile.CreatedDate = DateTime.Now;
                mantestfile.Duration = tfile.Duration;
                mantestfile.SchoolId = tfile.SchoolId;
                mantestfile.UserId = Convert.ToInt32(Session["Teacher"]);




                db.ManualTests.Add(mantestfile);
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
        public ActionResult viewtestfile()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int teacherid = Convert.ToInt32(Session["Teacher"]);
            var data = db.ManualTests.Where(x => x.UserId == teacherid).ToList();
            return View(data);

        }
        public ActionResult updateManualTest(int id)
        {

            using (Digital_SchoolEntities2 entities = new Digital_SchoolEntities2())
            {
                //Response.Write(submit);
                var test = db.ManualTests.Find(id);
                Session["url"] = test.TestUrl;
                return View(entities.ManualTests.Single(x => x.TestId == id));
            }

        }
        [HttpPost]


        public ActionResult updateManualTest(ManualTest mt, HttpPostedFileBase file)
        {
            try
            {

                    if (!(file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessing" || file.ContentType == "application/msword" || file.ContentType == "application/pdf"))
                {
                    ViewBag.Message= "Select Doc or Docx or  Pdf extention file only";
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



                        file.SaveAs(Path.Combine(Server.MapPath("~/FrontEnd/File_Upload/ManualTest/"), fileName));
                        mt.TestUrl = fileName;
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
                            return RedirectToAction("viewtestfile");
                        }
                        else
                        {
                            ViewBag.Message = "Not Valid File Type";
                            return RedirectToAction("viewtestfile");
                        }
                    }







                    //}
                    else
                    {
                        mt.TestUrl = Session["url"].ToString();
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
                            return RedirectToAction("viewtestfile");
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