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
    public class SchoolDBHandle
    {

        public tbl_schoolValidation GetProfileData(int CurrentUserid)
        {

            tbl_schoolValidation school = new tbl_schoolValidation();
            SqlCommand sc = new SqlCommand("[Digital_School].[get_SchoolProfileData]", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@School_Id", CurrentUserid);

            SqlDataReader reader = sc.ExecuteReader();
            //ek hi row return hoga islye 
            reader.Read();//return one row only
            school.School_Name = reader["School_Name"].ToString();
            school.School_Contactno = reader["School_Contactno"].ToString();
            school.School_Image = reader["School_Image"].ToString();
            school.Password = reader["Password"].ToString();
            school.School_Email = reader["School_Email"].ToString();
            school.School_Address = reader["School_Address"].ToString();

            reader.Close();
            return school;
        }
    }
}