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
    public class HomeController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Teacher/Home
        public ActionResult Index()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }

        public ActionResult ProfileSetting()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            TeacherDBHandle sadb = new TeacherDBHandle();

            int userid = Convert.ToInt32((Session["Teacher"]));
            var sab = db.Teachers.Find(userid);
            Session["imgPath"] = sab.Image;
            tbl_TeacherValidation sa = sadb.GetProfileData(userid);
            return View(sa);
        }
        [HttpPost]
        public ActionResult ProfileSetting(tbl_TeacherValidation sa)
        {
            try
            {
                digital_School.Models.Teacher teacher = new digital_School.Models.Teacher();
                int userid = Convert.ToInt32((Session["Teacher"]));

                var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);

                if (ModelState.IsValid)
                {
                    if (sa.UserImageFIle != null)
                    {

                        if (l != null)
                        {
                            Session["TeacherName"] = sa.Name;
                            l.UserId = userid;
                            l.Name = sa.Name;
                            l.Password = sa.Password;
                            l.Email = sa.Email;
                            l.RoleID = 3;
                            db.Entry(l).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        string filename = Path.GetFileNameWithoutExtension(sa.UserImageFIle.FileName);
                        string extension = Path.GetExtension(sa.UserImageFIle.FileName);
                        filename = DateTime.Now.ToString("yymmssff") + extension;




                        teacher.Image = "~/FrontEnd/Images/TeacherImage/" + filename;
                        teacher.Name = sa.Name;
                        teacher.Email = sa.Email;
                        teacher.Password = sa.Password;
                        teacher.Id = userid;





                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (sa.UserImageFIle.ContentLength <= 1000000)
                            {
                                db.Entry(teacher).State = EntityState.Modified;



                                string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                                if (db.SaveChanges() > 0)
                                {
                                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/TeacherImage/"), filename);
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
                        teacher.Image = Session["imgPath"].ToString();
                        if (l != null)
                        {
                            Session["TeacherName"] = sa.Name;
                            l.UserId = userid;
                            l.Name = sa.Name;
                            l.Password = sa.Password;
                            l.Email = sa.Email;
                            l.RoleID = 3;
                            db.Entry(l).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        teacher.Name = sa.Name;
                        teacher.Email = sa.Email;
                        teacher.Password = sa.Password;
                        teacher.Id = userid;
                        db.Entry(teacher).State = EntityState.Modified;

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
