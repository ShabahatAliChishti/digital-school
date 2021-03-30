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
    public class StudentDBHandle
    {
        public tbl_StudentValidation GetProfileData(int CurrentUserid)
        {

            tbl_StudentValidation student = new tbl_StudentValidation();
            SqlCommand sc = new SqlCommand("[Digital_School].get_StudentProfileData", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@Id", CurrentUserid);

            SqlDataReader reader = sc.ExecuteReader();
            //ek hi row return hoga islye 
            reader.Read();//return one row only
            student.Name = reader["Name"].ToString();
            student.ContactNo = reader["ContactNo"].ToString();
            student.Email = reader["Email"].ToString();
            student.Password = reader["Password"].ToString();
            student.Image = reader["ImagePath"].ToString();

            reader.Close();
            return student;
        }


        public List<tbl_StudentValidation> GetSchoolStudent(int schoolid)
        {
            List<tbl_StudentValidation> CourseInformation = new List<tbl_StudentValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_SchoolStudent", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@School_Id", schoolid);

            SqlDataReader sdr = sc.ExecuteReader();

            while (sdr.Read())
            {
                tbl_StudentValidation gc = new tbl_StudentValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.Id = Convert.ToInt32(sdr["Id"]);
                gc.RegNo = sdr["RegNo"].ToString();
                CourseInformation.Add(gc);
            }
            sdr.Close();
            return CourseInformation;
        }
    }
}