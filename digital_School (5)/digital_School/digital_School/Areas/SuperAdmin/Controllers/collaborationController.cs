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
    public class collaborationController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: SuperAdmin/collaboration

        public ActionResult ViewCollaboration()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            var data = db.Collaborations.ToList();


            return View(data);

        }




        [HttpGet]
        public ActionResult Addcollaboration()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            return View();
        }
        [HttpPost]
        public ActionResult Addcollaboration(tbl_collaborationValidation even)
        {
            try
            {
                Collaboration ev = new Collaboration();
                if (even.CollImageFIle == null)
                {
                    ModelState.AddModelError("NoFile", "Upload File");
                }
                else
                {
                    string filename = Path.GetFileNameWithoutExtension(even.CollImageFIle.FileName);
                    string extension = Path.GetExtension(even.CollImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;
                    ev.Image = "~/FrontEnd/Images/CollaborationImage/" + filename;
                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/CollaborationImage/"), filename);
                    even.CollImageFIle.SaveAs(filename);
                    ev.CallobrationName = even.CallobrationName;
                    ev.CallobrationTitle = even.CallobrationTitle;
                    ev.Date = DateTime.Now;
                    ev.Description = even.Description;




                    db.Collaborations.Add(ev);

                    db.SaveChanges();
                    ViewBag.Message = "Data Submitted";
                    ModelState.Clear();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Not Submitted";
                return View();
            }
            return View();




        }

        [HttpGet]
        public ActionResult updateCollaboration(int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }

            var data = db.Collaborations.Where(x => x.CallobrationId == id).First();
            Session["imgPath"] = data.Image;
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateCollaboration(tbl_collaborationValidation sa, int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            try
            {
                Collaboration ev = new Collaboration();
                //int userid = Convert.ToInt32((Session["Ad"]));

                //var l = db.loginTables.FirstOrDefault(t => t.UserId == userid);



                if (sa.CollImageFIle != null)
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
                    string filename = Path.GetFileNameWithoutExtension(sa.CollImageFIle.FileName);
                    string extension = Path.GetExtension(sa.CollImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;




                    ev.Image = "~/FrontEnd/Images/CollaborationImage/" + filename;
                    ev.CallobrationId = id;
                    ev.CallobrationName = sa.CallobrationName;
                    ev.CallobrationTitle = sa.CallobrationTitle;
                    ev.Date = DateTime.Now;
                    ev.Description = sa.Description;



                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (sa.CollImageFIle.ContentLength <= 1000000)
                        {
                            db.Entry(ev).State = EntityState.Modified;



                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                            if (db.SaveChanges() > 0)
                            {
                                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/CollaborationImage/"), filename);
                                sa.CollImageFIle.SaveAs(filename);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }


                                ViewBag.Message = "Data Updated";
                                return RedirectToAction("ViewCollaboration");
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
                    ev.Image = Session["imgPath"].ToString();
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
                    ev.CallobrationId = id;
                    ev.CallobrationName = sa.CallobrationName;
                    ev.CallobrationTitle = sa.CallobrationTitle;
                    ev.Date = DateTime.Now;
                    ev.Description = sa.Description;

                    db.Entry(ev).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {


                        ViewBag.Message = "Data Updated";
                        return RedirectToAction("ViewCollaboration");

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
        public JsonResult DeleteCollaboration(int CalloborateID)
        {

            bool resul = false;
            Collaboration sc = db.Collaborations.SingleOrDefault(x => x.CallobrationId == CalloborateID);
            if (sc != null)
            {
                db.Collaborations.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }







    }
}