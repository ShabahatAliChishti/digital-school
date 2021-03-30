using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.SuperAdmin.Controllers
{
    public class SchoolController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: SuperAdmin/School
        public ActionResult AddSchool()
        {

            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddSchool(tbl_schoolValidation school)
        {
            try
            {

                digital_School.Models.School a = new digital_School.Models.School();

                var userWithSameEmail = db.Schools.Where(m => m.School_Email == school.School_Email).SingleOrDefault(); //checking if the emailid already exits for any user
                if (userWithSameEmail == null)
                {
                    string filename = Path.GetFileNameWithoutExtension(school.UserImageFIle.FileName);
                    string extension = Path.GetExtension(school.UserImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;

                    a.School_Image = "~/FrontEnd/Images/SchoolImage/" + filename;
                    //image ko folder me save krwanay ke leye
                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/SchoolImage/"), filename);
                    school.UserImageFIle.SaveAs(filename);
                    a.School_Name = school.School_Name;
                    a.School_Address = school.School_Address;
                    a.School_Email = school.School_Email;
                    a.School_Contactno = school.School_Contactno;
                    a.Password = school.Password;

                    a.CreatedOn = DateTime.Now;

                    db.Schools.Add(a);
                    db.SaveChanges();
                }

                else
                {
                    ViewBag.Message = "User with this Email Already Exist";
                    return View();
                }


                int schoollatestid = a.School_Id;
                loginTable l = new loginTable();
                l.UserId = schoollatestid;
                l.Name = a.School_Name;
                l.Password = a.Password;
                l.RoleID = 2;
                l.Email = a.School_Email;
                db.loginTables.Add(l);
                db.SaveChanges();
                ModelState.Clear();
                ViewBag.Message = "Data Submitted";

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }
        }
        public ActionResult viewschool()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var data = db.Schools.ToList();


            return View(data);
        }
    }
}