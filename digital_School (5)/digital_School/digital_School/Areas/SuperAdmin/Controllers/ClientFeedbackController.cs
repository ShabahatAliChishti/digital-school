using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.SuperAdmin.Controllers
{
    public class ClientFeedbackController : Controller
    {
        // GET: SuperAdmin/ClientFeedback
        Digital_SchoolEntities2 db = new Digital_SchoolEntities2();
        [HttpGet]
        public ActionResult virewClientFeedback()
        {
            if (Session["Ad"] == null)
            {
                return RedirectToAction("login", "Home", new { area = "" });
            }

            var data = (from fedback in db.UserFeedbacks
                        join sc in db.Clients on fedback.UserId equals sc.UserId
                        where fedback.RoleId == 5


                        select new tbl_clienteFeedback
                        {
                            clientname = sc.UserName,
                            Description = fedback.Description,
                            Rating = fedback.Rating,
                            Date = fedback.CreatedDate,


                        }).ToList();

            //var data = db.Client_SchoolComplain.ToList();
            return View(data);
        }

    }
}