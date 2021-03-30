using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Controllers
{
    public class AccountController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        // GET: Account
        public ActionResult Login()
        {

            if (Session["Ad"] != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "SuperAdmin" });

            }
            else if (Session["school"] != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "Principle" });

            }
            else if (Session["Teacher"] != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "Teacher" });

            }
            else if (Session["Student"] != null)
            {
                return RedirectToAction("Index", "Home", new { Area = "Student" });

            }
            else if (Session["Cliente"] != null)
            {
                return RedirectToAction("dashboard", "Home", new { Area = "Cliente" });

            }
            return View();
        }



        [HttpPost]
        public ActionResult Login(tbl_LoginTableValidation k)
        {
            loginTable u = new loginTable();


            if (k != null)
            {
                u.Email = k.Email;
                u.Password = k.Password;

                var x = db.loginTables.Where(a => a.Email == u.Email && a.Password == u.Password).SingleOrDefault();
                if (x == null)
                {
                    ViewBag.ErrorMessage = "Login Failed";
                    return View();

                }
                else
                {
                    if (x.RoleID == 1)
                    {

                        var data = db.SuperAdmins.Find(x.UserId);
                        Session["imgPath"] = data.ad_imageurl;
                        Session["Ad"] = x.UserId;
                        Session["name"] = x.Name;
                        Session["RoleId"] = x.RoleID;
                        return RedirectToAction("Index", "Home", new { Area = "SuperAdmin" });
                    }
                    else if (x.RoleID == 2)
                    {
                        var data = db.Schools.Find(x.UserId);
                        Session["imgPath"] = data.School_Image;
                        Session["school"] = x.UserId;
                        Session["schoolName"] = x.Name;
                        Session["RoleId"] = x.RoleID;
                        return RedirectToAction("Index", "Home", new { Area = "Principle" });
                    }
                    else if (x.RoleID == 3)
                    {
                        var data = db.Teachers.Find(x.UserId);
                        Session["imgPath"] = data.Image;
                        Session["Teacher"] = x.UserId;
                        Session["TeacherName"] = x.Name;
                        Session["RoleId"] = x.RoleID;
                        return RedirectToAction("Index", "Home", new { Area = "Teacher" });
                    }
                    else if (x.RoleID == 4)
                    {
                        var data = db.Students.Find(x.UserId);
                        Session["imgPath"] = data.ImagePath;
                        Session["Student"] = x.UserId;
                        Session["StudentName"] = x.Name;
                        Session["RoleId"] = x.RoleID;
                        return RedirectToAction("Index", "Home", new { Area = "Student" });
                    }

                    else if (x.RoleID == 5)
                    {
                        var data = db.Clients.Find(x.UserId);
                        Session["imgPath"] = data.Image;
                        Session["Cliente"] = x.UserId;
                        Session["ClientName"] = x.Name;
                        Session["RoleId"] = x.RoleID;
                        return RedirectToAction("dashboard", "Home", new { Area = "Cliente" });
                    }
                }

            }
            else
            {
                return View("Index");

            }



            return View();



        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]

        public ActionResult register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult register(tbl_ClientValidation uv, string tabButton, HttpPostedFileBase imgInp)
        {
            try
            {
                Client c = new Client();
                int latestclientid = 0;

                if (tabButton.Equals("next"))
                {

                    c.UserName = uv.UserName;
                    //c.FatherName = uv.FatherName;
                    c.CreatedOn = DateTime.Now;
                    c.Email = uv.Email;
                    c.Password = uv.Password;
                    c.ConfirmPassword = uv.ConfirmPassword;
                    c.Cnic = uv.Cnic;
                    c.Contact_No = uv.Contact_No;
                    c.status = 1;

                    c.Gender = uv.Gender;
                    db.Clients.Add(c);
                    db.SaveChanges();
                    latestclientid = c.UserId;
                    Session["Latestclientid"] = latestclientid;
                    //ViewBag.Message = "Data Submitted";


                    Session["btn"] = "1";
                }
                else if (tabButton.Equals("finish"))
                {

                    if (imgInp != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(imgInp.FileName);
                        string extension = Path.GetExtension(imgInp.FileName);
                        filename = DateTime.Now.ToString("yymmssff") + extension;
                        //c.Image = "~/Content/img/users/" + filename;

                        filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/ClientReg/"), filename);
                        imgInp.SaveAs(filename);

                        int id = Convert.ToInt32(Session["Latestclientid"]);
                        var item = db.Clients.Single(x => x.UserId == id);




                        item.Image = "~/FrontEnd/Images/ClientReg/" + filename;
                        db.Clients.Attach(item);
                        db.Entry(item).Property(X => X.Image).IsModified = true;
                        db.SaveChanges();
                        ModelState.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }
            return View();
        }


    }
}