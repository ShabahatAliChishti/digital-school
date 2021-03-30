using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.SuperAdmin.Controllers
{
    public class SchoolArticleApprovalController : Controller
    {
        // GET: SuperAdmin/SchoolArticleApproval

        public ActionResult schoolapprovalarticle()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("login", "Home", new { area = "" });
            }
            Digital_SchoolEntities2 entities = new Digital_SchoolEntities2();
 
            List<Article> article = entities.Articles.Where(x => x.SchoolId != null).ToList();
            List<Status> status = entities.Status.ToList();
            List<School> school = entities.Schools.ToList();
            List<ArticleType> articletype = entities.ArticleTypes.ToList();
            var query = from c in article
                        join s in status on c.statusId equals s.statusId into table1
                        join t in articletype on c.Article_TypeId equals t.Article_TypeId into table2
                        join sc in school on c.SchoolId equals sc.School_Id into table3
                        from s in table1.DefaultIfEmpty()
                        from t in table2.DefaultIfEmpty()
                        from sc in table3.DefaultIfEmpty()


                        select new ViewStatus { schoolarticle = c, statustype = s, Article_Typename = t, School_Name = sc };
            return View(query);

        }
        [HttpGet]
        public ActionResult ApproveArticle(int id)
        {
            //var Statustype = new SelectList(db.Status.ToList(), "statusId", "statustype");
            //ViewData["StatusType"] = Statustype;
            using (Digital_SchoolEntities2 entities = new Digital_SchoolEntities2())
            {
                //Response.Write(submit);
                return View(entities.Articles.Single(x => x.ArticleId == id));
            }


        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult ApproveArticle(int id, string submit)
        {
            switch (submit)
            {
                case "Approve":
                    using (Digital_SchoolEntities2 entities = new Digital_SchoolEntities2())
                    {

                        var item = entities.Articles.Single(x => x.ArticleId == id);



                        item.statusId = 2;

                        entities.Articles.Attach(item);
                        entities.Entry(item).Property(X => X.statusId).IsModified = true;
                        entities.SaveChanges();
                        return RedirectToAction("schoolapprovalarticle");
                        break;

                    }

                case "Reject":
                    using (Digital_SchoolEntities2 entities = new Digital_SchoolEntities2())
                    {

                        var item = entities.Articles.Single(x => x.ArticleId == id);



                        item.statusId = 3;

                        entities.Articles.Attach(item);
                        entities.Entry(item).Property(X => X.statusId).IsModified = true;
                        entities.SaveChanges();
                        return RedirectToAction("schoolapprovalarticle");
                        break;
                    }
                default:
                    throw new Exception();
                    break;
            }
            return View();
        }


    }
}