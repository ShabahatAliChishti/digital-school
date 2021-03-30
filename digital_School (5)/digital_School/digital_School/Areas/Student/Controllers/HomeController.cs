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

namespace digital_School.Areas.Student.Controllers
{
    public class HomeController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Student/Home
        public ActionResult Index()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }


        [HttpGet]
        public ActionResult ProfileSetting()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            StudentDBHandle sadb = new StudentDBHandle();

            int userid = Convert.ToInt32((Session["Student"]));
            var sab = db.Students.Find(userid);
            Session["imgPath"] = sab.ImagePath;
            Session["DateCreated"] = sab.RegisterationDate;
            Session["Address"] = sab.Address;

            tbl_StudentValidation sa = sadb.GetProfileData(userid);

            return View(sa);
        }
        [HttpPost]
        public ActionResult ProfileSetting(tbl_StudentValidation sa)
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            try
            {
                tbl_StudentValidation student = new tbl_StudentValidation();
                int userid = Convert.ToInt32((Session["Student"]));

                var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);

                if (ModelState.IsValid)
                {
                    if (sa.UserImageFIle != null)
                    {

                        if (l != null)
                        {
                            Session["StudentName"] = sa.Name;
                            l.UserId = userid;
                            l.Name = sa.Name;
                            l.Password = sa.Password;
                            l.Email = sa.Email;
                            l.RoleID = 4;
                            db.Entry(l).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        string filename = Path.GetFileNameWithoutExtension(sa.UserImageFIle.FileName);
                        string extension = Path.GetExtension(sa.UserImageFIle.FileName);
                        filename = DateTime.Now.ToString("yymmssff") + extension;




                        student.Image = "~/FrontEnd/Images/StudentImage/" + filename;
                        student.Name = sa.Name;
                        student.Email = sa.Email;
                        student.Password = sa.Password;
                        student.Id = userid;





                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (sa.UserImageFIle.ContentLength <= 1000000)
                            {
                                db.Entry(student).State = EntityState.Modified;



                                string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                                if (db.SaveChanges() > 0)
                                {
                                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/StudentImage/"), filename);
                                    sa.UserImageFIle.SaveAs(filename);
                                    if (System.IO.File.Exists(oldImgPath))
                                    {
                                        System.IO.File.Delete(oldImgPath);
                                    }


                                    ViewBag.Message = "Data Updated";
                                    return RedirectToAction("ProfileSetting");
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
                        student.Image = Session["imgPath"].ToString();
                        if (l != null)
                        {
                            Session["name"] = sa.Name;
                            l.UserId = userid;
                            l.Name = sa.Name;
                            l.Password = sa.Password;
                            l.Email = sa.Email;
                            l.RoleID = 4;
                            db.Entry(l).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        student.Name = sa.Name;
                        student.Email = sa.Email;
                        student.Password = sa.Password;
                        student.Id = userid;
                        db.Entry(student).State = EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {


                            ViewBag.Message = "Data Updated";
                            return RedirectToAction("ProfileSetting");
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