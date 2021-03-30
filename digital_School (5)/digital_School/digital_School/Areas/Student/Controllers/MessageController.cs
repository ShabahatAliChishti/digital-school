using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Net;
using System.Net.Mail;

using PagedList;
using System.Data.Entity;

using digital_School.Models;
using digital_School.DBHandleClass;
using digital_School.ViewModel;

namespace digital_School.AreasStudent.Controllers
{
    public class MessageController : Controller
    {
        Digital_SchoolEntities2 dbContext = new Digital_SchoolEntities2();
        public ActionResult viewdiscussion()
        {
            if (Session["Student"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int studentid = Convert.ToInt32(Session["Student"]);
            int tempclassid;
            int originalclassid;
            string Regno;

            var getstudentid = dbContext.Students.Find(studentid);
            tempclassid = getstudentid.Class_Id;
            Regno = getstudentid.RegNo;
            var classid = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid.Class_Id;
            //AB enroll course wala dropdown ajayega 
            CourseDBHandle gc = new CourseDBHandle();

            List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");



            return View();
        }
        [HttpPost]
        public ActionResult viewdiscussion(tblEnrollStudentInCourseValidation testing)
        {
            try
            {
                int studentid = Convert.ToInt32(Session["Student"]);
                int tempclassid;
                int originalclassid;
                string Regno;
                var getstudentid = dbContext.Students.Find(studentid);
                tempclassid = getstudentid.Class_Id;
                Regno = getstudentid.RegNo;
                var classid = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
                originalclassid = classid.Class_Id;
                //AB enroll course wala dropdown ajayega 
                CourseDBHandle gc = new CourseDBHandle();

                List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
                ViewBag.course = new SelectList(list, "courseId", "courseName");
                //TempData["course"] = testing.courseId;

                //var data = db.SchoolAssignments.ToList();
                return RedirectToAction("Index", new { courseid = testing.courseId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please try later";
                return View();
            }


        }
        public ActionResult Index(int? Id, int? page,int? courseid)
        {
            int teacherid = Convert.ToInt32(Session["Student"]);

            int schoolid;
            int classid;
            //int courseid = Convert.ToInt32(TempData["course"]);
            var getteacherid = dbContext.Students.Find(teacherid);

            schoolid = getteacherid.School_Id;
            classid = getteacherid.Class_Id;
        


            int pageSize = 5;
            int pageNumber = (page ?? 1);
            MessageReplyViewModel vm = new MessageReplyViewModel();
            var count = dbContext.Messages.Count();

            decimal totalPages = count / (decimal)pageSize;
            ViewBag.TotalPages = Math.Ceiling(totalPages);
            vm.Messages = dbContext.Messages.Where(x => x.Class_Id == classid && x.SchoolId == schoolid && x.courseId==courseid).OrderBy(x => x.DatePosted).ToPagedList(pageNumber, pageSize);
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
                    vm.Replies.Add(null);
                }


                ViewBag.MessageId = Id.Value;
            }

            return View(vm);
        }

        [HttpPost]
       
        public ActionResult PostMessage(MessageReplyViewModel vm)
        {
            //var username = User.Identity.Name;
            //string fullName = "";
            int msgid = 0;

            int teacherid = Convert.ToInt32(Session["Student"]);
            int tempclassid;
            int originalclassid;
            string Regno;
            var getteacherid = dbContext.Students.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            Regno = getteacherid.RegNo;
            var classid1 = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid1.Class_Id;
            //AB enroll course wala dropdown ajayega 
            CourseDBHandle gc = new CourseDBHandle();

            List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
            ViewBag.course = new SelectList(list, "courseId", "courseName");
            int schoolid;
            int classid;
            schoolid = getteacherid.School_Id;
            classid =  getteacherid.Class_Id;

            //if (!string.IsNullOrEmpty(username))
            //{
            //    var user = dbContext.loginTables.SingleOrDefault(u => u.Name == username);
            //    fullName = string.Concat(new string[] { user.Name });
            //}
            Message messagetoPost = new Message();
            if (vm.Message.Subject != string.Empty && vm.Message.MessageToPost != string.Empty)
            {
                messagetoPost.DatePosted = DateTime.Now;
                messagetoPost.Subject = vm.Message.Subject;
                messagetoPost.MessageToPost = vm.Message.MessageToPost;
                messagetoPost.From = Session["StudentName"].ToString();
                messagetoPost.Class_Id = classid;
                messagetoPost.Role_ID = 4;
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
            MessageReplyViewModel vm = new MessageReplyViewModel();
            int teacherid = Convert.ToInt32(Session["Student"]);
            int tempclassid;
            int originalclassid;
            string Regno;
            var getteacherid = dbContext.Students.Find(teacherid);
            tempclassid = getteacherid.Class_Id;
            Regno = getteacherid.RegNo;
            var classid1 = dbContext.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
            originalclassid = classid1.Class_Id;
            //AB enroll course wala dropdown ajayega 
            CourseDBHandle gc = new CourseDBHandle();

            List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
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
                _reply.ReplyFrom = Session["StudentName"].ToString(); 
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
            int teacherid = Convert.ToInt32(Session["Student"]);
            //var message = dbContext.Messages.Where(i => i.Id == messageId && i.UserId == teacherid && i.Role_ID == 4);

            ////Message _messageToDelete = dbContext.Messages.Find(messageId);
            ////var message = new Message { Id= messageId ,UserId= teacherid ,Role_ID=4};
            //if (message != null)
            //{
            //dbContext.SaveChanges();
            int userid;
            int courseid;
            int roleid;
                Message _messageToDelete = dbContext.Messages.Find(messageId);
            courseid = Convert.ToInt32(_messageToDelete.courseId);
            userid = Convert.ToInt32(_messageToDelete.UserId);
            roleid = Convert.ToInt32(_messageToDelete.Role_ID);

            if(userid==teacherid && roleid==4)
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
                               TempData["ErrorMessage"] =  "This is Not Your Post";

            }

            // also delete the replies related to the message
           
          


            return RedirectToAction("Index", "Message", new { courseid = courseid });
        }
    }
}
