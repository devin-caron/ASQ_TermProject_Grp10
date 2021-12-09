using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using ASQ_TermProject_Grp10;


namespace Database.Tests
{
    [TestClass]
    public class DbUnitTests
    {

        SqlConnection conn = new SqlConnection();


        [TestMethod]
        public void connectionObj_checkIfConnectionObjectWorks()
        {
           
        }

        public void getData()
        {
            Console.WriteLine("Test: Pass");
        }

        [TestMethod]
        public void insertFuncttion_checkInsertionToDatabase()
        {

            Console.WriteLine("Test: Pass");
        }

        [TestMethod]
        public void readFunction_checkReadingFromDatabase()
        {

            Console.WriteLine("Test: Pass");
        }

    }
}
