using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Configuration;

namespace TempTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["connString"]);

        }
    }
}
