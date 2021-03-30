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
    public class ClassDBHandle
    {
        public List<tbl_ClassValidation> GetClass(int schoolid)
        {
            List<tbl_ClassValidation> ClassInformation = new List<tbl_ClassValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_SchoolClass", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@SchoolId", schoolid);

            SqlDataReader sdr = sc.ExecuteReader();


            while (sdr.Read())
            {
                tbl_ClassValidation gc = new tbl_ClassValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.Name = sdr["Name"].ToString();
                gc.Class_Id = Convert.ToInt32(sdr["Class_Id"]);

                ClassInformation.Add(gc);
            }
            sdr.Close();
            return ClassInformation;
        }
    }
}