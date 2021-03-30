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
    public class HomeController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Principle/Home
        public ActionResult Index()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }


        [HttpGet]
        public ActionResult ProfileSetting()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            SchoolDBHandle sadb = new SchoolDBHandle();

            int userid = Convert.ToInt32((Session["school"]));
            var sab = db.Schools.Find(userid);
            Session["imgPath"] = sab.School_Image;
            Session["DateCreated"] = sab.CreatedOn;
            tbl_schoolValidation sa = sadb.GetProfileData(userid);

            return View(sa);
        }

        [HttpPost]
        public ActionResult ProfileSetting(tbl_schoolValidation sa)

        {
            School school = new School();
            try
            {
                //loginTable l = new loginTable();
                int userid = Convert.ToInt32((Session["school"]));

                var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);

                //if (ModelState.IsValid)
                //{

                    if (sa.UserImageFIle != null)
                    {
                        if (l != null)
                        {
                            Session["schoolName"] = sa.School_Name;
                            l.UserId = userid;
                            l.Name = sa.School_Name;
                            l.Password = sa.Password;
                            l.Email = sa.School_Email;
                            l.RoleID = 2;
                            db.Entry(l).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        string filename = Path.GetFileNameWithoutExtension(sa.UserImageFIle.FileName);
                        string extension = Path.GetExtension(sa.UserImageFIle.FileName);
                        filename = DateTime.Now.ToString("yymmssff") + extension;



                        school.School_Address = sa.School_Address;
                        school.School_Image = "~/FrontEnd/Images/SchoolImage/" + filename;
                        school.School_Name = sa.School_Name;
                    school.School_Contactno = sa.School_Contactno;

                    school.School_Email = sa.School_Email;
                        school.Password = sa.Password;
                        school.School_Id = userid;
                    school.CreatedOn = Convert.ToDateTime(Session["DateCreated"]);





                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (sa.UserImageFIle.ContentLength <= 1000000)
                            {
                                db.Entry(school).State = EntityState.Modified;



                                string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                                if (db.SaveChanges() > 0)
                                {
                                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/SchoolImage/"), filename);
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
                        school.School_Image = Session["imgPath"].ToString();
                        if (l != null)
                        {
                            Session["schoolName"] = sa.School_Name;
                            l.UserId = userid;
                            l.Name = sa.School_Name;
                            l.Password = sa.Password;
                            l.Email = sa.School_Email;

                            l.RoleID = 2;
                            db.Entry(l).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        school.School_Name = sa.School_Name;
                        school.School_Email = sa.School_Email;
                        school.Password = sa.Password;
                       school.CreatedOn = Convert.ToDateTime(Session["DateCreated"]);
                    school.School_Contactno = sa.School_Contactno;
                    school.School_Address = sa.School_Address;
                        school.School_Id = userid;
                        db.Entry(school).State = EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {


                            ViewBag.Message = "Data Updated";
                            return RedirectToAction("ProfileSetting");
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