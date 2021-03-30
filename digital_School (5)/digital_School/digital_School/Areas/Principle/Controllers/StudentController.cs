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

namespace digital_School.Areas.Principle.Controllers
{
    public class StudentController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Principle/Student
        [HttpGet]
        public ActionResult addstudent()
        {

            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewBag.schoolclass = new SelectList(list, "Class_Id", "Name");
            SectionDBHandle section = new SectionDBHandle();
            List<tbl_SectionValidation> sectionlist = section.GetSection(schoolid);
            ViewBag.schoolsection = new SelectList(sectionlist, "SectionID", "SectionName");

            return View();
        }

        [HttpPost]
        public ActionResult addstudent(tbl_StudentValidation s)
        {

            try
            {
                int schoolid = Convert.ToInt32(Session["school"]);
                ClassDBHandle gc = new ClassDBHandle();

                List<tbl_ClassValidation> list = gc.GetClass(schoolid);
                ViewBag.schoolclass = new SelectList(list, "Class_Id", "Name");
                SectionDBHandle section = new SectionDBHandle();
                List<tbl_SectionValidation> sectionlist = section.GetSection(schoolid);
                ViewBag.schoolsection = new SelectList(sectionlist, "SectionID", "SectionName");


                digital_School.Models.Student a = new digital_School.Models.Student();
                var userWithSameEmail = db.Students.Where(m => m.Email == s.Email).SingleOrDefault(); //checking if the emailid already exits for any user
                if (userWithSameEmail == null)
                {

                    s.RegNo = StudentRegNo(s, s.Name);

                    string filename = Path.GetFileNameWithoutExtension(s.UserImageFIle.FileName);
                    string extension = Path.GetExtension(s.UserImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;

                    a.ImagePath = "~/FrontEnd/Images/StudentImage/" + filename;
                    //image ko folder me save krwanay ke leye
                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/StudentImage/"), filename);
                    s.UserImageFIle.SaveAs(filename);
                    a.Class_Id = s.Class_Id;
                    a.Section_Id = s.Section_Id;
                    a.School_Id = Convert.ToInt32(Session["school"]);
                    a.Name = s.Name;
                    a.RegNo = s.RegNo;
                    a.Address = s.Address;
                    a.Email = s.Email;
                    a.ContactNo = s.ContactNo;
                    a.Password = s.Password;

                    a.RegisterationDate = DateTime.Now;

                    db.Students.Add(a);
                    db.SaveChanges();
                }

                else
                {
                    ViewBag.Message = "User with this Email Already Exist";
                    return View();
                }

                int studentlatestid = a.Id;
                loginTable l = new loginTable();
                l.UserId = studentlatestid;
                l.Name = a.Name;
                l.Password = a.Password;
                l.RoleID = 4;
                l.Email = a.Email;
                db.loginTables.Add(l);
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

        public ActionResult viewstudent()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var id = Convert.ToInt32(Session["school"]);


            var data = db.Students.Where(x => x.School_Id == id).ToList();
            return View(data);
        }





        [HttpGet]
        public ActionResult updateStudent(int id)
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("login", "Home", new { area = "" });
            }
            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewBag.schoolclass = new SelectList(list, "Class_Id", "Name");
            SectionDBHandle section = new SectionDBHandle();
            List<tbl_SectionValidation> sectionlist = section.GetSection(schoolid);
            ViewBag.schoolsection = new SelectList(sectionlist, "SectionID", "SectionName");


            var data = db.Students.Where(x => x.Id == id).First();

            Session["imgPath"] = data.ImagePath;
            return View(data);

        }

        [HttpPost]

        public ActionResult updateStudent(tbl_StudentValidation stu, int id)
        {
            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewBag.schoolclass = new SelectList(list, "Class_Id", "Name");
            SectionDBHandle section = new SectionDBHandle();
            List<tbl_SectionValidation> sectionlist = section.GetSection(schoolid);
            ViewBag.schoolsection = new SelectList(sectionlist, "SectionID", "SectionName");
            stu.RegNo = StudentRegNo(stu , stu.Name);


            try
            {
                digital_School.Models.Student a = new digital_School.Models.Student();
                //int userid = Convert.ToInt32((Session["Ad"]));

                //var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);



                if (stu.UserImageFIle != null)
                {

                    //if (l != null)
                    //{

                    //    l.UserId = userid;
                    //    l.Name = sa.ad_name;
                    //    l.Password = sa.password;
                    //    l.Email = sa.ad_email;
                    //    l.RoleID = 1;
                    //    db.Entry(l).State = EntityState.Modified;

                    //    db.SaveChanges();
                    //}
                    string filename = Path.GetFileNameWithoutExtension(stu.UserImageFIle.FileName);
                    string extension = Path.GetExtension(stu.UserImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;




                    a.ImagePath = "~/FrontEnd/Images/StudentImage/" + filename;
                    a.Id = id;
                    a.Class_Id = stu.Class_Id;
                    a.Section_Id = stu.Section_Id;
                    a.School_Id = Convert.ToInt32(Session["school"]);
                    a.Name = stu.Name;
                    a.RegNo = stu.RegNo;
                    a.Address = stu.Address;
                    a.Email = stu.Email;
                    a.ContactNo = stu.ContactNo;
                    a.Password = stu.Password;
                    a.RegisterationDate = DateTime.Now;


                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (stu.UserImageFIle.ContentLength <= 1000000)
                        {
                            db.Entry(a).State = EntityState.Modified;



                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                            if (db.SaveChanges() > 0)
                            {
                                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/StudentImage/"), filename);
                                stu.UserImageFIle.SaveAs(filename);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }


                                ViewBag.Message = "Data Updated";
                                return RedirectToAction("viewstudent");
                            }
                        }
                        else
                        {
                            ViewBag.msg = "File Size must be Equal or less than 1mb";
                        }
                    }
                    else
                    {
                        ViewBag.msg = "Inavlid File Type";
                    }
                }

                //}
                else
                {
                    a.ImagePath = Session["imgPath"].ToString();
                    //if (l != null)
                    //{
                    //    Session["name"] = sa.ad_name;
                    //    l.UserId = userid;
                    //    l.Name = sa.ad_name;
                    //    l.Password = sa.password;
                    //    l.Email = sa.ad_email;
                    //    l.RoleID = 1;
                    //    db.Entry(l).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}
                    //superadmin.ad_name = sa.ad_name;

                    //superadmin.ad_email = sa.ad_email;
                    //superadmin.ad_password = sa.password;
                    //superadmin.ad_Id = userid;
                    a.Id = id;
                    a.Class_Id = stu.Class_Id;
                    a.Section_Id = stu.Section_Id;
                    a.School_Id = Convert.ToInt32(Session["school"]);
                    a.Name = stu.Name;
                    a.RegNo = stu.RegNo;
                    a.Address = stu.Address;
                    a.Email = stu.Email;
                    a.ContactNo = stu.ContactNo;
                    a.Password = stu.Password;
                    ////a.RegisterationDate = DateTime.Now;
                    db.Entry(a).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {


                        ViewBag.Message = "Data Updated";
                        return RedirectToAction("viewstudent");

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



        [HttpPost]
        public JsonResult DeleteStudent(int StudentId)
        {

            bool resul = false;
            digital_School.Models.Student sc = db.Students.SingleOrDefault(x => x.Id == StudentId);
            if (sc != null)
            {
                db.Students.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }





        private string StudentRegNo(tbl_StudentValidation student, string name)
        {
            int id = db.Students.Count(s => (s.Section_Id == student.Section_Id)
                    && (student.RegisterationDate.Year == student.RegisterationDate.Year)) + 1;
            //edu_course.Models.Student st= db.Students.Where(d => d.Id == student.Id).FirstOrDefault();
            string registrationId = name + "-" + student.RegisterationDate.Year + "-";

            string addZero = "";
            int len = 3 - id.ToString().Length;
            for (int i = 0; i < len; i++)
            {
                addZero = "0" + addZero;
            }

            return registrationId + addZero + id;
        }
    
}
}