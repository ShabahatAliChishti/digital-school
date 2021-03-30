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
    public class SectionDBHandle
    {
        public List<tbl_SectionValidation> GetSection(int schoolid)
        {
            List<tbl_SectionValidation> SectionInformation = new List<tbl_SectionValidation>();

            SqlCommand sc = new SqlCommand("[Digital_School].get_SchoolSection", Connections.GetConnection());
            sc.CommandType = CommandType.StoredProcedure;
            sc.Parameters.AddWithValue("@SchoolId", schoolid);

            SqlDataReader sdr = sc.ExecuteReader();


            while (sdr.Read())
            {
               tbl_SectionValidation gc = new tbl_SectionValidation();
                //gc.CourseId = Convert.ToInt32(sdr["courseId"]);
                gc.SectionName = sdr["SectionName"].ToString();
                gc.SectionID = Convert.ToInt32(sdr["SectionID"]);

                SectionInformation.Add(gc);
            }
            sdr.Close();
            return SectionInformation;
        }

    }
}