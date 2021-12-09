using AircraftTransmissionSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using FDMScommonLib;

namespace ATTS.Tests {
    [TestClass]
    public class ATTSUnitTests {
        private static AircraftTransmissionSystem.ATTS ATTS; 

        [TestMethod]
        public void ATTS_CanReadAllFiles() {

            AircraftTransmissionSystem.ATTS.readTelemetry();
            Assert.IsTrue(AircraftTransmissionSystem.ATTS.flights.Count > 0);
        }

        [TestMethod]
        public void ATTS_CanReadOneFile() {
            var rnd = new Random();
            int rndFileIndex = rnd.Next(0,2);

            String[] filePaths;
            String flightDir = Path.GetFullPath(Path.Combine(@"..\..\..\AircraftTransmissionData\"));
            FlightData fd;

            filePaths = Directory.GetFiles(flightDir);

            fd = AircraftTransmissionSystem.ATTS.readFile(filePaths[rndFileIndex]);

            Assert.IsTrue(fd.TelemetryList.Count > 0);
        }

        [TestMethod]
        public void ATTS_packetizeCorrectlyCreatesPacket() {
            String expectedFirstPacketStr = "C-FGAX|7_8_2018 19:34:3,-0.319754,-0.716176,1.79715,2154.67,1643.844,0.022278,0.033622,|548"; //the correct packet for the very first packet
            String actualPacketStr = "";

            AircraftTransmissionSystem.ATTS.readTelemetry();

            actualPacketStr = AircraftTransmissionSystem.ATTS.packetize("C-FGAX", AircraftTransmissionSystem.ATTS.flights[0].TelemetryList[0]);
            Assert.AreEqual(expectedFirstPacketStr, actualPacketStr);
        }
    }
}
