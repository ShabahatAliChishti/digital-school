using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace digital_School.ConnectionClass
{
    public class Connections
    {
      
            public static SqlConnection My_SQL_Connection;
            public static SqlConnection GetConnection()
            {
                if (My_SQL_Connection == null)
                {
                    My_SQL_Connection = new SqlConnection();
                    My_SQL_Connection.ConnectionString = ConfigurationManager.ConnectionStrings["Digital_School"].ToString();
                    My_SQL_Connection.Open();
                }
                return My_SQL_Connection;
            }

        
    }
}