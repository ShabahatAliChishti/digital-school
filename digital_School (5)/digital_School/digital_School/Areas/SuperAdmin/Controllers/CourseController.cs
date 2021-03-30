using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.SuperAdmin.Controllers
{
    public class CourseController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: SuperAdmin/Course
        [HttpGet]
        public ActionResult ViewCourse()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            var data = db.Courses.ToList();
            return View(data);
        }

        [HttpGet]
        public ActionResult addCourse()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult addCourse(tbl_coursevalidation cou)
        {
            try
            {
                Course cc = new Course();




                string filename = Path.GetFileNameWithoutExtension(cou.CourseImageFIle.FileName);
                string extension = Path.GetExtension(cou.CourseImageFIle.FileName);
                filename = DateTime.Now.ToString("yymmssff") + extension;
                cc.imageLink = "~/FrontEnd/Images/CourseImage/" + filename;

                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/CourseImage/"), filename);
                cou.CourseImageFIle.SaveAs(filename);

                cou.Role_Id = Convert.ToInt32(Session["RoleId"]);
                cou.User_Id = Convert.ToInt32(Session["Ad"]);
                cc.User_Id = cou.User_Id;
                cc.Role_Id = cou.Role_Id;
                cc.CreatedDate = DateTime.Now;
                cc.courseDescription = cou.courseDescription;
                cc.courseName = cou.courseName;
                cc.courseType = cou.courseType;
                cc.Code = cou.Code;
                cc.VideoLink = cou.VideoLink;
                cc.longDes = cou.longDes;
                cc.duration = cou.duration;
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
        public ActionResult updatecourse(int id)
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            var data = db.Courses.Where(x => x.courseId == id).First();
            Session["imgPath"] = data.imageLink;
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updatecourse(tbl_coursevalidation sa, int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }

            try
            {
                Course cc = new Course();

                //int userid = Convert.ToInt32((Session["Ad"]));

                //var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);



                if (sa.CourseImageFIle != null)
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
                    string filename = Path.GetFileNameWithoutExtension(sa.CourseImageFIle.FileName);
                    string extension = Path.GetExtension(sa.CourseImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;




                    cc.imageLink = "~/FrontEnd/Images/CourseImage/" + filename;
                   
                    
                    sa.Role_Id = Convert.ToInt32(Session["RoleId"]);
                    sa.User_Id = Convert.ToInt32(Session["Ad"]);
                    cc.courseId = id;
                    cc.User_Id = sa.User_Id;
                    cc.Role_Id = sa.Role_Id;
                    cc.CreatedDate = DateTime.Now;
                    cc.courseDescription = sa.courseDescription;
                    cc.courseName = sa.courseName;
                    cc.courseType = sa.courseType;
                    cc.Code = sa.Code;
                    cc.VideoLink = sa.VideoLink;
                    cc.longDes = sa.longDes;
                    cc.duration = sa.duration;



                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (sa.CourseImageFIle.ContentLength <= 1000000)
                        {
                            db.Entry(cc).State = EntityState.Modified;



                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                            if (db.SaveChanges() > 0)
                            {
                                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/CourseImage/"), filename);
                                sa.CourseImageFIle.SaveAs(filename);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }


                                ViewBag.Message = "Data Updated";
                                return RedirectToAction("ViewCourse");
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
                    cc.imageLink = Session["imgPath"].ToString();
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

                    sa.Role_Id = Convert.ToInt32(Session["RoleId"]);
                    sa.User_Id = Convert.ToInt32(Session["Ad"]);
                    cc.courseId = id;
                    cc.User_Id = sa.User_Id;
                    cc.Role_Id = sa.Role_Id;
                    cc.CreatedDate = DateTime.Now;
                    cc.courseDescription = sa.courseDescription;
                    cc.courseName = sa.courseName;
                    cc.courseType = sa.courseType;
                    cc.Code = sa.Code;
                    cc.VideoLink = sa.VideoLink;
                    cc.longDes = sa.longDes;
                    cc.duration = sa.duration;


                    db.Entry(cc).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {


                        ViewBag.Message = "Data Updated";
                        return RedirectToAction("ViewCourse");

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
        public JsonResult DeleteCourse(int courseID)
        {

            bool resul = false;
           Course sc = db.Courses.SingleOrDefault(x => x.courseId == courseID);
            if (sc != null)
            {
                db.Courses.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }

    }
}