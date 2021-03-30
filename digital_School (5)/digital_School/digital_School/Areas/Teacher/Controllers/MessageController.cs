using digital_School.DBHandleClass;
using digital_School.Models;
using digital_School.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Teacher.Controllers
{
    public class MessageController : Controller
    {
        // GET: Teacher/Message
        Digital_SchoolEntities2 dbContext = new Digital_SchoolEntities2();
        public ActionResult viewdiscussion()
        {
            if (Session["Teacher"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int teacherid = Convert.ToInt32(Session["Teacher"]);
            int tempclassid;
            int originalclassid;
            var getteacherid = dbContext.Teachers.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            var classid = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;


            CourseDBHandle gc = new CourseDBHandle();

            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");



            return View();
        }
        [HttpPost]
        public ActionResult viewdiscussion(tbl_CourseAssigntoTeacherValidation testing)
        {
            try
            {
                int teacherid = Convert.ToInt32(Session["Teacher"]);
                int tempclassid;
                int originalclassid;
                var getteacherid = dbContext.Teachers.Find(teacherid);
                tempclassid = getteacherid.Class_Id;
                var classid = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
                originalclassid = classid.Class_Id;


                CourseDBHandle gc = new CourseDBHandle();

                List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
                ViewBag.course = new SelectList(list, "courseId", "courseName");
                return RedirectToAction("Index", new { courseid = testing.courseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please try later";
                return View();
            }


        }
        public ActionResult Index(int? Id, int? page, int? courseid)
        {
            int teacherid = Convert.ToInt32(Session["Teacher"]);

            int schoolid;
            int classid;
            var getteacherid = dbContext.Teachers.Find(teacherid);

            schoolid = getteacherid.School_Id;
            classid = getteacherid.Class_Id;



            int pageSize = 5;
            int pageNumber = (page ?? 1);
            MessageReplyViewModel vm = new MessageReplyViewModel();
            var count = dbContext.Messages.Count();

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = dbContext.Messages.Where(x => x.Class_Id == classid && x.SchoolId == schoolid && x.courseId == courseid).OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
            ViewBag.MessagesInOnePage = vm.Messages;
            ViewBag.PageNumber = pageNumber;

            if (Id != null)
            {

                var replies = dbContext.Replies.Where(x => x.MessageId == Id.Value).OrderByDescending(x => x.ReplyDateTime).ToList();
                if (replies != null)
                {
                    foreach (var rep in replies)
                    {
                        MessageReplyViewModel.MessageReply reply = new MessageReplyViewModel.MessageReply();
                        reply.MessageId = rep.MessageId;
                        reply.Id = rep.Id;
                        reply.ReplyMessage = rep.ReplyMessage;
                        reply.ReplyDateTime = rep.ReplyDateTime;
                        reply.MessageDetails = dbContext.Messages.Where(x => x.Id == rep.MessageId).Select(s => s.MessageToPost).FirstOrDefault();
                        reply.ReplyFrom = rep.ReplyFrom;
                        vm.Replies.Add(reply);
                    }

                }
                else
                {
                }
                vm.Replies.Add(null);


                ViewBag.MessageId = Id.Value;
            }

            return View(vm);
        }

        [HttpPost]

        public ActionResult PostMessage(MessageReplyViewModel vm)
        {

            int msgid = 0;

            int teacherid = Convert.ToInt32(Session["Teacher"]);

            int schoolid;
            int classid;
            var getteacherid = dbContext.Teachers.Find(teacherid);
            schoolid = getteacherid.School_Id;
            classid = getteacherid.Class_Id;

            CourseDBHandle gc = new CourseDBHandle();
            int tempclassid;
            int originalclassid;
            tempclassid = getteacherid.Class_Id;
            var classid1 = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid1.Class_Id;
            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");

            Message messagetoPost = new Message();
            if (vm.Message.Subject != string.Empty && vm.Message.MessageToPost != string.Empty)
            {
                messagetoPost.DatePosted = DateTime.Now;
                messagetoPost.Subject = vm.Message.Subject;
                messagetoPost.courseId = vm.Message.courseId;

                messagetoPost.MessageToPost = vm.Message.MessageToPost;
                messagetoPost.From = Session["TeacherName"].ToString();
                messagetoPost.Class_Id = classid;
                messagetoPost.Role_ID = 3;
                messagetoPost.UserId = teacherid;
                messagetoPost.courseId = vm.Message.courseId;
                messagetoPost.SchoolId = schoolid;
                dbContext.Messages.Add(messagetoPost);
                dbContext.SaveChanges();
                msgid = messagetoPost.Id;
            }

            return RedirectToAction("Index", "Message", new { Id = msgid, courseid = vm.Message.courseId });
        }

        public ActionResult Create()
        {
            int teacherid = Convert.ToInt32(Session["Teacher"]);
            MessageReplyViewModel vm = new MessageReplyViewModel();
            CourseDBHandle gc = new CourseDBHandle();
            int tempclassid;
            int originalclassid;
            var getteacherid = dbContext.Teachers.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            var classid = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;
            List<tbl_CourseAssigntoTeacherValidation> list = gc.GetTeacherAssignedCourse(teacherid, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");
            return View(vm);
        }

        [HttpPost]
        public ActionResult ReplyMessage(MessageReplyViewModel vm, int messageId)
        {
            //var username = User.Identity.Name;
            //string fullName = "";
            //if (!string.IsNullOrEmpty(username))
            //{
            //    var user = dbContext.loginTables.SingleOrDefault(u => u.Name == username);
            //    fullName = string.Concat(new string[] { user.Name});
            //}
            if (vm.Reply.ReplyMessage != null)
            {
                Reply _reply = new Reply();
                _reply.ReplyDateTime = DateTime.Now;
                _reply.MessageId = messageId;
                _reply.ReplyFrom = Session["TeacherName"].ToString();
                _reply.ReplyMessage = vm.Reply.ReplyMessage;
                dbContext.Replies.Add(_reply);
                dbContext.SaveChanges();
            }
            //reply to the message owner          - using email template

            //var messageOwner = dbContext.Messages.Where(x => x.Id == messageId).Select(s => s.From).FirstOrDefault();
            //var users = from user in dbContext.loginTables
            //            orderby user.Name
            //            select new
            //            {
            //                FullName = user.Name,
            //                UserEmail = user.Email
            //            };

            //var uemail = users.Where(x => x.FullName == messageOwner).Select(s => s.UserEmail).FirstOrDefault();
            //SendGridMessage replyMessage = new SendGridMessage();
            //replyMessage.From = new MailAddress(username);
            //replyMessage.Subject = "Reply for your message :" + dbContext.Messages.Where(i=>i.Id==messageId).Select(s=>s.Subject).FirstOrDefault();
            //replyMessage.Text = vm.Reply.ReplyMessage;


            //replyMessage.AddTo(uemail);


            //var credentials = new NetworkCredential("YOUR SENDGRID USERNAME", "PASSWORD");
            //var transportweb = new Microsoft.Web(credentials);
            //transportweb.DeliverAsync(replyMessage);
            int courseid;
            Message _messageToDelete = dbContext.Messages.Find(messageId);
            courseid = Convert.ToInt32(_messageToDelete.courseId);
            return RedirectToAction("Index", "Message", new { Id = messageId, courseid = courseid });

        }

        [HttpPost]
        public ActionResult DeleteMessage(int messageId)
        {
            int teacherid = Convert.ToInt32(Session["Teacher"]);
            int userid;
            int roleid;
            int courseid;
            Message _messageToDelete = dbContext.Messages.Find(messageId);
            courseid = Convert.ToInt32(_messageToDelete.courseId);

            userid = Convert.ToInt32(_messageToDelete.UserId);
            roleid = Convert.ToInt32(_messageToDelete.Role_ID);


            //Message _messageToDelete = dbContext.Messages.Find(messageId);
            if (userid == teacherid && roleid == 3)
            {


                dbContext.Messages.Remove(_messageToDelete);
                dbContext.SaveChanges();

                var _repliesToDelete = dbContext.Replies.Where(i => i.MessageId == messageId).ToList();
                if (_repliesToDelete != null)
                {
                    foreach (var rep in _repliesToDelete)
                    {
                        dbContext.Replies.Remove(rep);
                        dbContext.SaveChanges();
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "This is Not Your Post";

            }





            return RedirectToAction("Index", "Message", new { courseid = courseid });
        }
    }
}