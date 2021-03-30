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
    public class TeacherController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Principle/Teacher
        [HttpGet]
        public ActionResult addteacher()
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
        public ActionResult addteacher(tbl_TeacherValidation teacher)
        {
            try
            {
                int schoolid = Convert.ToInt32(Session["school"]);
                ClassDBHandle gc = new ClassDBHandle();

                List<tbl_ClassValidation> list = gc.GetClass(schoolid);
                ViewBag.school = new SelectList(list, "Class_Id", "Name");
                teacher.Reg_No = RegNo(teacher);

                digital_School.Models.Teacher a = new digital_School.Models.Teacher();

                var userWithSameEmail = db.Teachers.Where(m => m.Email == teacher.Email).SingleOrDefault(); //checking if the emailid already exits for any user
                if (userWithSameEmail == null)
                {
                    string filename = Path.GetFileNameWithoutExtension(teacher.UserImageFIle.FileName);
                    string extension = Path.GetExtension(teacher.UserImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;

                    a.Image = "~/FrontEnd/Images/TeacherImage/" + filename;
                    //image ko folder me save krwanay ke leye
                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/TeacherImage/"), filename);
                    teacher.UserImageFIle.SaveAs(filename);
                    a.Class_Id = teacher.Class_Id;
                    a.School_Id = Convert.ToInt32(Session["school"]);
                    a.Name = teacher.Name;
                    a.Reg_No = teacher.Reg_No;
                    a.Address = teacher.Address;
                    a.Email = teacher.Email;
                    a.Contact = teacher.Contact;
                    a.Password = teacher.Password;

                    a.JoiningDate = DateTime.Now;

                    db.Teachers.Add(a);

                    db.SaveChanges();
                }

                else
                {
                    ViewBag.Message = "User with this Email Already Exist";
                    return View();
                }


                int teacherlatestid = a.Id;
                loginTable l = new loginTable();
                l.UserId = teacherlatestid;
                l.Name = a.Name;
                l.Password = a.Password;
                l.RoleID = 3;
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
        public ActionResult viewsteacher()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var id = Convert.ToInt32(Session["school"]);


            var data = db.Teachers.Where(x => x.School_Id == id).ToList();
            return View(data);
        }






        [HttpGet]
        public ActionResult updateTeacher(int id)
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewBag.school = new SelectList(list, "Class_Id", "Name");



            var data = db.Teachers.Where(x => x.Id == id).First();

            Session["imgPath"] = data.Image;
            return View(data);

        }

        [HttpPost]
        [ValidateInput(false)]

        public ActionResult updateTeacher(tbl_TeacherValidation tea, int id)
        {
            int schoolid = Convert.ToInt32(Session["school"]);
            ClassDBHandle gc = new ClassDBHandle();
            List<tbl_ClassValidation> list = gc.GetClass(schoolid);
            ViewBag.school = new SelectList(list, "Class_Id", "Name");

            tea.Reg_No = RegNo(tea);



            try
            {
                digital_School.Models.Teacher a = new digital_School.Models.Teacher();
                //int userid = Convert.ToInt32((Session["Ad"]));

                //var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);



                if (tea.UserImageFIle != null)
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
                    string filename = Path.GetFileNameWithoutExtension(tea.UserImageFIle.FileName);
                    string extension = Path.GetExtension(tea.UserImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;




                    a.Image = "~/FrontEnd/Images/TeacherImage/" + filename;
                    a.Id = id;
                    a.Class_Id = tea.Class_Id;
                    a.School_Id = Convert.ToInt32(Session["school"]);
                    a.Name = tea.Name;
                    a.Reg_No = tea.Reg_No;
                    a.Address = tea.Address;
                    a.Email = tea.Email;
                    a.Contact = tea.Contact;
                    a.Password = tea.Password;

                    a.JoiningDate = DateTime.Now;


                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (tea.UserImageFIle.ContentLength <= 1000000)
                        {
                            db.Entry(a).State = EntityState.Modified;



                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                            if (db.SaveChanges() > 0)
                            {
                                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/TeacherImage/"), filename);
                               tea.UserImageFIle.SaveAs(filename);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }


                                ViewBag.Message = "Data Updated";
                                return RedirectToAction("viewsteacher");
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
                    a.Image = Session["imgPath"].ToString();
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
                    a.Class_Id = tea.Class_Id;
                    a.School_Id = Convert.ToInt32(Session["school"]);
                    a.Name = tea.Name;
                    a.Reg_No = tea.Reg_No;
                    a.Address = tea.Address;
                    a.Email = tea.Email;
                    a.Contact = tea.Contact;
                    a.Password = tea.Password;

                    a.JoiningDate = DateTime.Now;

                    db.Entry(a).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {


                        ViewBag.Message = "Data Updated";
                        return RedirectToAction("viewsteacher");

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
        public JsonResult DeleteTeacher(int teacherID)
        {

            bool resul = false;
            digital_School.Models.Teacher sc = db.Teachers.SingleOrDefault(x => x.Id == teacherID);
            if (sc != null)
            {
                db.Teachers.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }





        private string RegNo(tbl_TeacherValidation teacher)
        {
            int id = db.Teachers.Count(s => (s.Class_Id == teacher.Class_Id)
                    && (teacher.JoiningDate.Year == teacher.JoiningDate.Year)) + 1;
            Tbl_Class aClass = db.Tbl_Class.Where(d => d.Class_Id == teacher.Class_Id).FirstOrDefault();
            string registrationId = aClass.Name + "-" + teacher.JoiningDate.Year + "-";

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