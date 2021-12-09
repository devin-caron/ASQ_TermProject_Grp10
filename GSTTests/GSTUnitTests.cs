using ASQ_TermProject_Grp10;
using AircraftTransmissionSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace GSTTests
{
    [TestClass]
    public class GSTUnitTests
    {
        [TestMethod]
        public void GST_CanReceiveData()
        {
            String expectedFirstPacketStr = "C-FGAX|7_8_2018 19:34:3,-0.319754,-0.716176,1.79715,2154.67,1643.844,0.022278,0.033622,|548"; //the correct packet for the very first packet

            AircraftTransmissionSystem.ATTS.readTelemetry();
            String actualPacketStr = AircraftTransmissionSystem.ATTS.packetize("C-FGAX", AircraftTransmissionSystem.ATTS.flights[0].TelemetryList[0]);
            byte[] msg = Encoding.ASCII.GetBytes(actualPacketStr);

            String data = Encoding.ASCII.GetString(msg, 0, msg.Length);

            Assert.AreEqual(data, expectedFirstPacketStr);
        }

        [TestMethod]
        public void test()
        {

        }
    }
}
