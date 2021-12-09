using AircraftTransmissionSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using FDMScommonLib;

namespace GSTTests
{
    [TestClass]
    public class GSTUnitTests
    {
        [TestMethod]
        public void GST_CanReceiveData()
        {
            String expectedFirstPacketStr = "C-FGAX|7_8_2018 19:34:3,-0.319754,-0.716176,1.79715,2154.67,1643.844,0.022278,0.033622,|548"; //the correct packet for the very first packet

            ATTS.readTelemetry();
            String actualPacketStr = ATTS.packetize("C-FGAX", ATTS.flights[0].TelemetryList[0]);
            byte[] msg = Encoding.ASCII.GetBytes(actualPacketStr);

            String data = Encoding.ASCII.GetString(msg, 0, msg.Length);

            Assert.AreEqual(data, expectedFirstPacketStr);
        }

        [TestMethod]
        public void GST_CanStoreDataPacket()
        {
            String data = "C-FGAX|7_8_2018 19:34:3,-0.319754,-0.716176,1.79715,2154.67,1643.844,0.022278,0.033622,|548";
            String[] splitPacket = data.Split('|');

            List<AircraftTelemetryEntry> testList = new List<AircraftTelemetryEntry>();

            AircraftTelemetryEntry entry = new AircraftTelemetryEntry(splitPacket[1], splitPacket[0]);
            testList.Add(entry);

            Assert.AreEqual(testList[0], entry);
        }

        [TestMethod]
        public void GST_CanVerifyChecksum()
        {

        }

        [TestMethod]
        public void GST_PrintsCorrectDataToASCII()
        {

        }
    }
}
