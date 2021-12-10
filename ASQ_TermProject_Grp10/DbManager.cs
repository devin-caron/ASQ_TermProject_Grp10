using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace ASQ_TermProject_Grp10 {
    class DbManager {

        private SqlConnection conn;

        public DbManager() {
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        }


        public void InserAttitudeParameters(string aircraftID, string timestamp, double altitude, double pitch, double bank) {
            SqlCommand cmd = null;

            try {
                conn.Open();

                cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO[dbo].[AttitudeParameters]([AircraftID],[Timestamp],[Altitude],[Pitch],[Bank]) " +
                    "VALUES('" + aircraftID + "','" + timestamp + "'," + altitude + "," + pitch + "," + bank + ")";
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            finally {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }




        public void InsertGForceParameters(string aircraftID, string timestamp, double x, double y, double z, double weight) {
            SqlCommand cmd = null;



            try {
                conn.Open();

                cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO[dbo].[GForceParameters]([AircraftID],[Timestamp],[AccelX],[AccelY],[AccelZ],[Weight]) " +
                    "VALUES('" + aircraftID + "','" + timestamp + "'," + x + "," + y + "," + z + ", " + weight + ")";
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            finally {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        public DataTable ReadFromDb(string tblName) {

            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;


            try {
                conn.Open();

                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM" + tblName;
                cmd.Connection = conn;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable(tblName);

                da.Fill(dt);


            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            finally {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return dt;

        }
    }
}
