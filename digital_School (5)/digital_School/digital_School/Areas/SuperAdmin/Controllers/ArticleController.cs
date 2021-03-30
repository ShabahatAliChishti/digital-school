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

    public class ArticleController : Controller
    {

        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        [HttpGet]
        public ActionResult viewArticle()
        {

            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var user = Convert.ToInt32(Session["Ad"]);
            var data = db.Articles.Where(x => x.UserId == user  && x.Role_Id == 1 ).ToList();

            return View(data);

        }




        // GET: SuperAdmin/Article
        public ActionResult AddArticle()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            List<ArticleType> list = db.ArticleTypes.ToList();
            ViewBag.articletypelist = new SelectList(list, "Article_TypeId", "Article_Typename ");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddArticle(tbl_ArticleValidation article)
        {
            try
            {
                Article a = new Article();

                List<ArticleType> list = db.ArticleTypes.ToList();
                ViewBag.articletypelist = new SelectList(list, "Article_TypeId", "Article_Typename");


                string filename = Path.GetFileNameWithoutExtension(article.UserImageFIle.FileName);
                string extension = Path.GetExtension(article.UserImageFIle.FileName);
                filename = DateTime.Now.ToString("yymmssff") + extension;
                a.imgPath = "~/FrontEnd/Images/Adminimg/" + filename;
                //image ko folder me save krwanay ke leye
                filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/ArticleImage/"), filename);
                article.UserImageFIle.SaveAs(filename);
                a.Article_TypeId = article.Article_TypeId;
                a.Role_Id = Convert.ToInt32(Session["RoleId"]);
                a.Title = article.Title;
                a.shortDes = article.shortDes;
                a.statusId = 2;
                a.longDes = article.longDes;
                a.UserId = Convert.ToInt32(Session["Ad"]);
                a.CreatedDate = DateTime.Now;

                db.Articles.Add(a);

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


        public ActionResult AddArticleType()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }

            return View();
        }
        [HttpPost]
        public ActionResult AddArticleType(tbl_articletypeValidation art)
        {
            try
            {
                ArticleType arttype = new ArticleType();
                arttype.Article_Typename = art.Article_Typename;
                db.ArticleTypes.Add(arttype);
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
        public ActionResult updateArticle(int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            List<ArticleType> list = db.ArticleTypes.ToList();
            ViewBag.articletypelist = new SelectList(list, "Article_TypeId", "Article_Typename ");

            
            //int userid = Convert.ToInt32((Session["Ad"]));
            //var sab = db.KidsStories.Find(userid);

            var data = db.Articles.Where(x => x.ArticleId == id).First();
            Session["imgPath"] = data.imgPath;
            return View(data);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult updateArticle(tbl_ArticleValidation sa, int id)

        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });

            }
            List<ArticleType> list = db.ArticleTypes.ToList();
            ViewBag.articletypelist = new SelectList(list, "Article_TypeId", "Article_Typename ");

            try
            {
                Article art = new Article();
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




                        art.imgPath = "~/FrontEnd/Images/ArticleImage/" + filename;
                    art.ArticleId = id;        
                    art.Article_TypeId = sa.Article_TypeId;
                        art.Role_Id = Convert.ToInt32(Session["RoleId"]);
                        art.UserId = Convert.ToInt32(Session["Ad"]);
                        art.Title = sa.Title;
                        art.shortDes = sa.shortDes;
                        art.statusId = 2;
                        art.longDes = sa.longDes;

                        art.CreatedDate = DateTime.Now;


                        if (extension.ToLower() == ".jpg" || extension.ToLower() == ".jpeg" || extension.ToLower() == ".png")
                        {
                            if (sa.UserImageFIle.ContentLength <= 1000000)
                            {
                                db.Entry(art).State = EntityState.Modified;



                                string oldImgPath = Request.MapPath(Session["imgPath"].ToString());

                                if (db.SaveChanges() > 0)
                                {
                                    filename = Path.Combine(Server.MapPath("~/FrontEnd/Images/ArticleImage/"), filename);
                                    sa.UserImageFIle.SaveAs(filename);
                                    if (System.IO.File.Exists(oldImgPath))
                                    {
                                        System.IO.File.Delete(oldImgPath);
                                    }


                                    ViewBag.Message = "Data Updated";
                                    return RedirectToAction("viewArticle");
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
                        art.imgPath = Session["imgPath"].ToString();
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
                    art.ArticleId = id;
                    art.Article_TypeId = sa.Article_TypeId;
                        art.Role_Id = Convert.ToInt32(Session["RoleId"]);
                        art.Title = sa.Title;
                        art.shortDes = sa.shortDes;
                        art.statusId = 2;
                        art.longDes = sa.longDes;
                        art.UserId = Convert.ToInt32(Session["Ad"]);
                        art.CreatedDate = DateTime.Now;

                        db.Entry(art).State = EntityState.Modified;

                        if (db.SaveChanges() > 0)
                        {


                            ViewBag.Message = "Data Updated";
                            return RedirectToAction("viewArticle");

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
        public JsonResult DeleteArticle(int ArtID)
        {

            bool resul = false;
            Article sc = db.Articles.SingleOrDefault(x => x.ArticleId == ArtID);
            if (sc != null)
            {
                db.Articles.Remove(sc);
                db.SaveChanges();
                resul = true;
            }

            return Json(resul, JsonRequestBehavior.AllowGet);
        }


    }
}