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
    public class SuperAdminDBHandle
    {

        public SuperAdminProfileDataModel GetProfileData(int CurrentUserid)
        {

            SuperAdminProfileDataModel sa = new SuperAdminProfileDataModel();
            SqlCommand sc = new SqlCommand("[Digital_School].get_SuperAdminProfileData", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@ad_Id", CurrentUserid);

            SqlDataReader reader = sc.ExecuteReader();
            //ek hi row return hoga islye 
            reader.Read();//return one row only
            sa.ad_email = reader["ad_email"].ToString();
            sa.ad_name = reader["ad_name"].ToString();
            sa.ad_imageurl = reader["ad_imageurl"].ToString();
            sa.password = reader["ad_password"].ToString();

            reader.Close();
            return sa;
        }

        public void UpdateProfileData(SuperAdminProfileDataModel sa, string userimage, int currentuserid)
        {
            sa.ad_imageurl = userimage;
            SqlCommand sc = new SqlCommand("[Digital_School].update_SuperAdminProfileData", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@ad_Id", currentuserid);
            sc.Parameters.AddWithValue("@ad_email", sa.ad_email);
            sc.Parameters.AddWithValue("@ad_name", sa.ad_name);

            sc.Parameters.AddWithValue("@ad_imageurl", sa.ad_imageurl);

            sc.ExecuteNonQuery();
        }
    }
}