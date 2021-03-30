using digital_School.Models;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace digital_School.Areas.Principle.Controllers
{
    public class RoomAllocationController : Controller
    {
        private Digital_SchoolEntities2 db = new Digital_SchoolEntities2();

        public ActionResult Index()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int schoolid = Convert.ToInt32(Session["school"]);
            ViewBag.departments = db.Tbl_Class.Where(s => s.SchoolId == schoolid).ToList();
            return View();
        }


        public ActionResult Create()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            int schoolid = Convert.ToInt32(Session["school"]);

            ViewBag.courses = db.Courses.Where(s => s.Status == true && s.User_Id == schoolid).ToList();
            ViewBag.departments = db.Tbl_Class.Where(s => s.SchoolId == schoolid).ToList();
            ViewBag.Rooms = db.Sections.Where(s => s.SchoolId == schoolid).ToList();
            ViewBag.Days = db.Days.ToList();
            return View();
        }

        public JsonResult GetCoursesByDeptId(int deptId)
        {
            var courses = db.Courses.Where(t => t.Class_Id == deptId).Select(x => new { x.courseId, x.Code }).ToList();
            return Json(courses, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveRoomSchedule(TimeTable roomAllocation)
        {
            var scheduleList = db.TimeTables.Where(t => t.SectionId == roomAllocation.SectionId && t.DayId == roomAllocation.DayId && t.AllocationStatus == "Allocated").ToList();
            if (scheduleList.Count == 0)
            {
                int schoolid = Convert.ToInt32(Session["school"]);
                roomAllocation.SchoolId = schoolid;
                roomAllocation.AllocationStatus = "Allocated";
                db.TimeTables.Add(roomAllocation);
                db.SaveChanges();
                return Json(true);
            }
            else
            {
                bool state = false;
                foreach (var allocation in scheduleList)
                {
                    if ((roomAllocation.StartTime >= allocation.StartTime && roomAllocation.StartTime < allocation.EndTime)
                         || (roomAllocation.EndTime > allocation.StartTime && roomAllocation.EndTime <= allocation.EndTime) && roomAllocation.AllocationStatus == "Allocated")
                    {
                        state = true;
                    }
                }
                if (state == false)
                {
                    int schoolid = Convert.ToInt32(Session["school"]);
                    roomAllocation.SchoolId = schoolid;
                    roomAllocation.AllocationStatus = "Allocated";
                    db.TimeTables.Add(roomAllocation);
                    db.SaveChanges();
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }

        }

        public JsonResult GetClassScheduleInfo(int deptId)
        {
            var courses = db.Courses.Where(t => t.Class_Id == deptId).ToList();
            List<ClassSchedule> classSchedules = new List<ClassSchedule>();
            foreach (var course in courses)
            {
                string schedule = "";
                int counter = 0;
                var courseSchedules = db.TimeTables.Where(t => t.ClassId == course.Class_Id && t.CourseId == course.courseId && t.AllocationStatus == "Allocated").ToList();
                foreach (var courseSchedule in courseSchedules)
                {
                    if (counter != 0)
                    {
                        schedule += "; ";
                    }
                    schedule += "Section : " + courseSchedule.Section.SectionName + ", " + courseSchedule.Day.Name.Substring(0, 3) + ", ";
                    int h, m;
                    string p = "AM";
                    int st = courseSchedule.StartTime;
                    h = st / 60;
                    m = st - (h * 60);
                    if (st >= 720)
                    {
                        h -= 12;
                        if (h == 0) h = 12;
                        p = "PM";
                    }
                    schedule += h + ":" + m.ToString("00") + " " + p + " - ";
                    int et = courseSchedule.EndTime;
                    h = et / 60;
                    m = et - (h * 60);
                    p = "AM";
                    if (et >= 720)
                    {
                        h -= 12;
                        if (h == 0) h = 12;
                        p = "PM";
                    }
                    schedule += h + ":" + m.ToString("00") + " " + p;
                    counter++;
                }
                if (schedule == "") schedule = "Not Scheduled Yet.";
                ClassSchedule classSchedule = new ClassSchedule(course.Code, course.courseName, schedule);
                classSchedules.Add(classSchedule);
            }
            return Json(classSchedules, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UnallocateRoom()
        {
            if (Session["school"] == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            return View();
        }

        public JsonResult UnallocateAllRooms(bool name)
        {
            int schoolid = Convert.ToInt32(Session["school"]);
            var roomInfo = db.TimeTables.Where(r => r.AllocationStatus == "Allocated" && r.SchoolId == schoolid).ToList();
            if (roomInfo.Count == 0)
            {
                return Json(false);
            }
            else
            {
                foreach (var room in roomInfo)
                {
                    room.AllocationStatus = null;
                    db.Entry(room).State = EntityState.Modified;
                    db.SaveChanges();

                }
                return Json(true);
            }


        }
    }
}
