using digital_School.ConnectionClass;
using digital_School.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace digital_School.DBHandleClass
{
    public class CourseDBHandle
    {

        public List<jobModel> GetJobs()
        {
            List<jobModel> JobData = new List<jobModel>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_AnotherDatabase", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {

                jobModel gc = new jobModel();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.jobId = Convert.ToInt32(sdr["jobId"]);
                gc.jobTitle = sdr["jobTitle"].ToString();
                gc.imageUrl = sdr["imageUrl"].ToString();
                gc.date = Convert.ToString(sdr["date"]);
                gc.shortDescription = sdr["shortDescription"].ToString();
                gc.longDescription = sdr["longDescription"].ToString();
                gc.views = Convert.ToInt32(sdr["views"]);
                //lv.Name = sdr["Name"].ToString(
                JobData.Add(gc);
            }
            sdr.Close();
            return JobData;
        }
        public List<tbl_OnlineTestValidation> GetCourse()
        {
            List<tbl_OnlineTestValidation> CourseInformation = new List<tbl_OnlineTestValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_OurCourse", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                tbl_OnlineTestValidation gc = new tbl_OnlineTestValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseName = sdr["courseName"].ToString();
                CourseInformation.Add(gc);
            }
            sdr.Close();
            return CourseInformation;
        }
        public List<tbl_coursevalidation> GetCourseClient()
        {
            List<tbl_coursevalidation> CourseData = new List<tbl_coursevalidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_OurCourseMain", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;

            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                tbl_coursevalidation gc = new tbl_coursevalidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseName = sdr["courseName"].ToString();
                gc.imageLink = sdr["imageLink"].ToString();
                CourseData.Add(gc);
            }
            sdr.Close();
            return CourseData;
        }
        public List<tbl_CourseAssigntoTeacherValidation> GetTeacherAssignedCourse(int TeacherId, int Class_Id)
        {
            List<tbl_CourseAssigntoTeacherValidation> CourseInformation = new List<tbl_CourseAssigntoTeacherValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_TeacherAssignedCourse", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@TeacherId", TeacherId);
            sc.Parameters.AddWithValue("@Class_Id", Class_Id);
            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                tbl_CourseAssigntoTeacherValidation gc = new tbl_CourseAssigntoTeacherValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseName = sdr["courseName"].ToString();
                CourseInformation.Add(gc);
            }
            sdr.Close();
            return CourseInformation;
        }

        public List<tblEnrollStudentInCourseValidation> GetSudentEnrollCourse(string RegNo, int Class_Id)
        {
            List<tblEnrollStudentInCourseValidation> CourseInformation = new List<tblEnrollStudentInCourseValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_StudentEnrollCourse", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@RegNo", RegNo);
            sc.Parameters.AddWithValue("@Class_Id", Class_Id);
            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                tblEnrollStudentInCourseValidation gc = new tblEnrollStudentInCourseValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseName = sdr["courseName"].ToString();
                CourseInformation.Add(gc);
            }
            sdr.Close();
            return CourseInformation;
        }

        public List<tbl_OnlineTestValidation> GetSchoolCourse(int schoolid, int roleid)
        {
            List<tbl_OnlineTestValidation> CourseInformation = new List<tbl_OnlineTestValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_SchoolCourse", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@User_Id", schoolid);
            sc.Parameters.AddWithValue("@Role_Id", roleid);


            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                tbl_OnlineTestValidation gc = new tbl_OnlineTestValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseName = sdr["courseName"].ToString();
                CourseInformation.Add(gc);
            }
            sdr.Close();
            return CourseInformation;
        }

        public List<tbl_CourseAssigntoTeacherValidation> GetTeacherStudentSimilarCourse(int Id, string RegNo)
        {
            List<tbl_CourseAssigntoTeacherValidation> CourseInformation = new List<tbl_CourseAssigntoTeacherValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_TeacherStudentSimilarCourse", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@Id", Id);
            sc.Parameters.AddWithValue("@RegNo", RegNo);
            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                tbl_CourseAssigntoTeacherValidation gc = new tbl_CourseAssigntoTeacherValidation();
                gc.courseId = Convert.ToInt32(sdr["courseId"]);
                gc.courseName = sdr["courseName"].ToString();
                CourseInformation.Add(gc);
            }
            sdr.Close();
            return CourseInformation;
        }
        public List<Tbl_UserEnrollInCourseValidation> GetStudentRegisterationNoSimilarCourse(int Id)
        {
            List<Tbl_UserEnrollInCourseValidation> EnrollmentInformation = new List<Tbl_UserEnrollInCourseValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_StudentRegisterationNoSimilarCourse", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@Id", Id);
            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                Tbl_UserEnrollInCourseValidation gc = new Tbl_UserEnrollInCourseValidation();
                gc.EnrollmentId = Convert.ToInt32(sdr["EnrollmentId"]);
                gc.RegistrationId = sdr["RegistrationId"].ToString();
                EnrollmentInformation.Add(gc);
            }
            sdr.Close();
            return EnrollmentInformation;
        }

    }
}