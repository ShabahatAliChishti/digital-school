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
    public class DepartmentController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: SuperAdmin/Department
        [HttpGet]
        public ActionResult addDeartment()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            return View();

        }
        [HttpPost]
        public ActionResult addDeartment(tbl_addDepartment dep)
        {
            try
            {
                Department department = new Department();
                department.Department_Name = dep.Department_Name;
                db.Departments.Add(department);
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
        [HttpGet]
        public ActionResult ViewTeam()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            var data = db.Teams.ToList();

            return View(data);
        }
        [HttpGet]
        public ActionResult addTeam()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            List<Department> list = db.Departments.ToList();
            ViewBag.departmentList = new SelectList(list, "Department_ID", "Department_Name");


            return View();
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult addTeam(tbl_addTeam te)
        {

            List<Department> list = db.Departments.ToList();
            ViewBag.departmentList = new SelectList(list, "Department_ID", "Department_Name");

            Team tea = new Team();

            string filename = Path.GetFileNameWithoutExtension(te.UserImageFIle.FileName);
            string extension = Path.GetExtension(te.UserImageFIle.FileName);
            filename = DateTime.Now.ToString("yymmssff") + extension;
            tea.Image = "~/FrontEnd/Images/DepartmentImage/" + filename;
            //image ko folder me save krwanay ke leye
            filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/DepartmentImage/"), filename);
            te.UserImageFIle.SaveAs(filename);
            tea.Designation = te.Designation;
            tea.Long_Description = te.Long_Description;
            tea.Short_Description = te.Short_Description;
            tea.Department_ID = te.Department_ID;
            tea.Name = te.Name;



            db.Teams.Add(tea);

            db.SaveChanges();

            ModelState.Clear();
            ViewBag.Message = "Data Submitted";


            return View();
        }

        [HttpPost]
        public JsonResult DeleteTeam(int TeamID)
        {

            bool resul = false;
            Team sc = db.Teams.SingleOrDefault(x => x.Team_Id == TeamID);
            if (sc != null)
            {
                db.Teams.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult updateTeam(int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            List<Department> list = db.Departments.ToList();
            ViewBag.departmentList = new SelectList(list, "Department_ID", "Department_Name");


            //int userid = Convert.ToInt32((Session["Ad"]));
            //var sab = db.KidsStories.Find(userid);

            var data = db.Teams.Where(x => x.Team_Id == id).First();
            Session["imgPath"] = data.Image;
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateTeam(tbl_addTeam sa, int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            List<Department> list = db.Departments.ToList();
            ViewBag.departmentList = new SelectList(list, "Department_ID", "Department_Name");

            try
            {
                Team tea = new Team();

                //int userid = Convert.ToInt32((Session["Ad"]));

                //var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);



                if (sa.UserImageFIle != null)
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
                    string filename = Path.GetFileNameWithoutExtension(sa.UserImageFIle.FileName);
                    string extension = Path.GetExtension(sa.UserImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;




                    tea.Image = "~/FrontEnd/Images/DepartmentImage/" + filename;
                    tea.Team_Id= id;
                    tea.Designation = sa.Designation;
                    tea.Long_Description = sa.Long_Description;
                    tea.Short_Description = sa.Short_Description;
                    tea.Department_ID = sa.Department_ID;
                    tea.Name = sa.Name;


                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (sa.UserImageFIle.ContentLength <= 1000000)
                        {
                            db.Entry(tea).State = EntityState.Modified;



                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                            if (db.SaveChanges() > 0)
                            {
                                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/DepartmentImage/"), filename);
                                sa.UserImageFIle.SaveAs(filename);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }


                                ViewBag.Message = "Data Updated";
                                return RedirectToAction("ViewTeam");
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
                    tea.Image = Session["imgPath"].ToString();
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



                    tea.Team_Id = id;
                    tea.Designation = sa.Designation;
                    tea.Long_Description = sa.Long_Description;
                    tea.Short_Description = sa.Short_Description;
                    tea.Department_ID = sa.Department_ID;
                    tea.Name = sa.Name;

                    db.Entry(tea).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {


                        ViewBag.Message = "Data Updated";
                        return RedirectToAction("ViewTeam");

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