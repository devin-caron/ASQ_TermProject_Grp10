using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ASQ_TermProject_Grp10;


namespace Dusan_Tmp
{
    class DbManager
    {

        private SqlConnection conn;

        public DbManager()
        {
            conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
        }


        public DataTable ReadFromDb(string tblName)
        {

            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;


            try
            {
                conn.Open();

                cmd = new SqlCommand();
                cmd.CommandText = "SELECT * FROM" + tblName;
                cmd.Connection = conn;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable(tblName);

                da.Fill(dt);


            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }

            return dt;

        }
    }
}
