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
    public class TeacherDBHandle
    {
        public tbl_TeacherValidation GetProfileData(int CurrentUserid)
        {

            tbl_TeacherValidation teacher = new tbl_TeacherValidation();
            SqlCommand sc = new SqlCommand("[Digital_School].get_TeacherProfileData", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@Id", CurrentUserid);

            SqlDataReader reader = sc.ExecuteReader();
            //ek hi row return hoga islye 
            reader.Read();//return one row only
            teacher.Name = reader["Name"].ToString();
            teacher.Contact = reader["Contact"].ToString();
            teacher.Email = reader["Email"].ToString();
            teacher.Password = reader["Password"].ToString();
            teacher.Image = reader["Image"].ToString();

            reader.Close();
            return teacher;
        }


        public List<tbl_TeacherValidation> GetSchool(int School_Id)
        {
            List<tbl_TeacherValidation> TeacherSchoolInformation = new List<tbl_TeacherValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_TeacherSchool", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@School_Id", School_Id);

            SqlDataReader sdr = sc.ExecuteReader();


            while (sdr.Read())
            {
                tbl_TeacherValidation gc = new tbl_TeacherValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.Name = sdr["Name"].ToString();
                gc.Id = Convert.ToInt32(sdr["Id"]);


                TeacherSchoolInformation.Add(gc);
            }
            sdr.Close();
            return TeacherSchoolInformation;
        }

    }
}