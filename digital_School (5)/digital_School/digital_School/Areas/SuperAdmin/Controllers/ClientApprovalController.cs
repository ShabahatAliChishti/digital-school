using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.SuperAdmin.Controllers
{
    public class ClientApprovalController : Controller
    {
        // GET: SuperAdmin/ClientApproval
        public ActionResult Clientapprovalarticle()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("login", "Home", new { area = "" });
            }
            Digital_SchoolEntities2 entities = new Digital_SchoolEntities2();
            List<Article> article = entities.Articles.Where(x => x.SchoolId != null).ToList();
            List<Status> status = entities.Status.ToList();
            List<digital_School.Models.Client> client = entities.Clients.ToList();
            List<ArticleType> articletype = entities.ArticleTypes.ToList();
            var query = from c in article
                        join s in status on c.statusId equals s.statusId into table1
                        join t in articletype on c.Article_TypeId equals t.Article_TypeId into table2
                        join sc in client on c.UserId equals sc.UserId into table3
                        from s in table1.DefaultIfEmpty()
                        from t in table2.DefaultIfEmpty()
                        from sc in table3.DefaultIfEmpty()


                        select new ViewStatus { Clientarticle = c, statustype = s, Article_Typename = t, CreateDate = sc };
            return View(query);

        }

        [HttpGet]
        public ActionResult ApproveArticleClient(int id)
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

        public ActionResult ApproveArticleClient(int id, string submit)
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
                        return RedirectToAction("Clientapprovalarticle");
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
                        return RedirectToAction("Clientapprovalarticle");
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