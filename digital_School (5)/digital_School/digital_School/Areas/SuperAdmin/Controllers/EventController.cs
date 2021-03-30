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
    public class EventController : Controller
    {
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        // GET: SuperAdmin/Event

        [HttpGet]
        public ActionResult viewsevent()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            else
            {
               
                return View(from Event in db.Events.Take(9)
                            select Event);
            }
        }


        [HttpGet]
        public ActionResult addEvent()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            return View();
        }
        [HttpPost]
        public ActionResult addEvent(tbl_eventValidation even)
        {
            try
            {
                Event ev = new Event();
                if (even.UserImageFIle == null)
                {
                    ModelState.AddModelError("NoFile", "Upload File");
                }
                else
                {
                    string filename = Path.GetFileNameWithoutExtension(even.UserImageFIle.FileName);
                    string extension = Path.GetExtension(even.UserImageFIle.FileName);
                    filename = DateTime.Now.ToString("yymmssff") + extension;
                    ev.Event_VenueImage = "~/FrontEnd/Images/EventImage/" + filename;
                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/EventImage/"), filename);
                    even.UserImageFIle.SaveAs(filename);
                    ev.Title = even.Title;
                    ev.Event_Description = even.Event_Description;
                    ev.Event_End_Date = even.Event_End_Date;
                    ev.Event_Start_Date = even.Event_Start_Date;
                    ev.Event_End_Time = even.Event_End_Time;
                    ev.Event_Start_Time = even.Event_Start_Time;
                    ev.Event_Venue = even.Event_Venue;
                    ev.Event_VenueVideo = even.Event_VenueVideo;



                    db.Events.Add(ev);

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
        public ActionResult updateevent(int id)
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            var data = db.Events.Where(x => x.EventId == id).First();
            Session["imgPath"] = data.Event_VenueImage;
            return View(data);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateArticle(tbl_eventValidation sa, int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }

            try
            {
                Event ev = new Event();
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




                    ev.Event_VenueImage = "~/FrontEnd/Images/EventImage/" + filename;
                    ev.EventId = id;
                    ev.Title = sa.Title;
                    ev.Event_Description = sa.Event_Description;
                    ev.Event_End_Date = sa.Event_End_Date;
                    ev.Event_Start_Date = sa.Event_Start_Date;
                    ev.Event_End_Time = sa.Event_End_Time;
                    ev.Event_Start_Time = sa.Event_Start_Time;
                    ev.Event_Venue = sa.Event_Venue;
                    ev.Event_VenueVideo = sa.Event_VenueVideo;




                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                    {
                        if (sa.UserImageFIle.ContentLength <= 1000000)
                        {
                            db.Entry(ev).State = EntityState.Modified;



                            string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                            if (db.SaveChanges() > 0)
                            {
                                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/EventImage/"), filename);
                                sa.UserImageFIle.SaveAs(filename);
                                if (System.IO.File.Exists(oldImgPath))
                                {
                                    System.IO.File.Delete(oldImgPath);
                                }


                                ViewBag.Message = "Data Updated";
                                return RedirectToAction("viewsevent");
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
                    ev.Event_VenueImage = Session["imgPath"].ToString();
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
                    ev.EventId = id;
                    ev.Title = sa.Title;
                    ev.Event_Description = sa.Event_Description;
                    ev.Event_End_Date = sa.Event_End_Date;
                    ev.Event_Start_Date = sa.Event_Start_Date;
                    ev.Event_End_Time = sa.Event_End_Time;
                    ev.Event_Start_Time = sa.Event_Start_Time;
                    ev.Event_Venue = sa.Event_Venue;
                    ev.Event_VenueVideo = sa.Event_VenueVideo;


                    db.Entry(ev).State = EntityState.Modified;

                    if (db.SaveChanges() > 0)
                    {


                        ViewBag.Message = "Data Updated";
                        return RedirectToAction("viewsevent");

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

        public JsonResult DeleteEvent(int eventID)
        {

            bool resul = false;
            Event sc = db.Events.SingleOrDefault(x => x.EventId == eventID);
            if (sc != null)
            {
                db.Events.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }



    }
}