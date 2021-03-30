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

namespace digital_School.Areas.SuperAdmin.Controllers
{
    public class HomeController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: SuperAdmin/Home
        public ActionResult Index()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            return View();
        }
        [HttpGet]
        public ActionResult ProfileSetting()

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            SuperAdminDBHandle sadb = new SuperAdminDBHandle();

            int userid = Convert.ToInt32((Session["Ad"]));
            var sab = db.SuperAdmins.Find(userid);
            Session["imgPath"] = sab.ad_imageurl;
            SuperAdminProfileDataModel sa = sadb.GetProfileData(userid);

            return View(sa);
        }
        [HttpPost]
        public ActionResult ProfileSetting(SuperAdminProfileDataModel sa)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            try
            {
                digital_School.Models.SuperAdmin superadmin = new digital_School.Models.SuperAdmin();
                int userid = Convert.ToInt32((Session["Ad"]));

                var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);

                if (ModelState.IsValid)
                {
                    if (sa.UserImageFile != null)
                    {

                        if (l != null)
                        {
                            Session["name"] = sa.ad_name;
                            l.UserId = userid;
                            l.Name = sa.ad_name;
                            l.Password = sa.password;
                            l.Email = sa.ad_email;
                            l.RoleID = 1;
                            db.Entry(l).State = EntityState.Modified;

                            db.SaveChanges();
                        }
                        string filename = Path.GetFileNameWithoutExtension(sa.UserImageFile.FileName);
                        string extension = Path.GetExtension(sa.UserImageFile.FileName);
                        filename = DateTime.Now.ToString("yymmssff") + extension;




                        superadmin.ad_imageurl = "~/FrontEnd/Images/SuperAdminImage/" + filename;
                        superadmin.ad_name = sa.ad_name;
                        superadmin.ad_email = sa.ad_email;
                        superadmin.ad_password = sa.password;
                        superadmin.ad_Id = userid;





                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (sa.UserImageFile.ContentLength <= 1000000)
                            {
                                db.Entry(superadmin).State = EntityState.Modified;



                                string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                                if (db.SaveChanges() > 0)
                                {
                                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/SuperAdminImage/"), filename);
                                    sa.UserImageFile.SaveAs(filename);
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
                        superadmin.ad_imageurl = Session["imgPath"].ToString();
                        if (l != null)
                        {
                            Session["name"] = sa.ad_name;
                            l.UserId = userid;
                            l.Name = sa.ad_name;
                            l.Password = sa.password;
                            l.Email = sa.ad_email;
                            l.RoleID = 1;
                            db.Entry(l).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        superadmin.ad_name = sa.ad_name;
                        superadmin.ad_email = sa.ad_email;
                        superadmin.ad_password = sa.password;
                        superadmin.ad_Id = userid;
                        db.Entry(superadmin).State = EntityState.Modified;

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