using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Database.Test
{
    [TestClass]
    public class UnitTest1
    {

        public string connString = "Data Source=.;Integrated Security=SSPI;Initial Catalog=fdms_db";

        [TestMethod]
        public void checkProgramsConnectionToDb()
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                //IF(EXISTS(SELECT COUNT(*) name FROM master.dbo.databases WHERE(name = 'fdms_db')))
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(name) FROM sys.databases WHERE name = 'fdms_db'", conn))
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
        }

        
        [TestMethod]
        public void chekcInsertingDataIntoDb()
        {

            using (SqlConnection conn = new SqlConnection(connString))
            {
                //IF(EXISTS(SELECT COUNT(*) name FROM master.dbo.databases WHERE(name = 'fdms_db')))
                using (SqlCommand cmd = new SqlCommand("INSERT INTO AttitudeParameters VALUES('T-TTTT', '2000-02-02 00:00:00.000', 1, 1, 1)", conn))
                {
                    conn.Open();

                    cmd.ExecuteNonQuery();
                    
                    conn.Close();
                }
            }
        }


        [TestMethod]
        public void checkReadingDataFromDb()
        {
            SqlDataAdapter da;
            DataTable dt;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                //IF(EXISTS(SELECT COUNT(*) name FROM master.dbo.databases WHERE(name = 'fdms_db')))
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM AttitudeParameters", conn))
                {
                    conn.Open();

                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);

                    conn.Close();
                }
            }
        }

        


    }
}
