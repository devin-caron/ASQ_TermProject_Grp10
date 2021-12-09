using Microsoft.VisualStudio.TestTools.UnitTesting;
using AircraftTransmissionSystem;
using System;
using System.Linq;
using FDMScommonLib;

namespace ATTS.Tests {
    [TestClass]
    public class TelemetryEntryTests {
        public static String entryStr = "7_8_2018 19:34:4,0.257793, -0.141772, 2.658766, 2154.66, 1630.865, 0.031823, 0.034561,";
        public static AircraftTelemetryEntry entry = new AircraftTelemetryEntry(entryStr, "");

        [TestMethod]
        public void Constructor_FieldsShouldPopulateCorrectly() {
            Assert.AreNotEqual(null, entry.Timestamp);
            Assert.AreNotEqual(0.0f, entry.Accelx);
            Assert.AreNotEqual(0.0f, entry.Accely);
            Assert.AreNotEqual(0.0f, entry.Accelz);
            Assert.AreNotEqual(0.0f, entry.Weight);
            Assert.AreNotEqual(0.0f, entry.Altitude);
            Assert.AreNotEqual(0.0f, entry.Pitch);
            Assert.AreNotEqual(0.0f, entry.Bank);
        }

        [TestMethod]
        public void calcChkSum_checkSumCalculatesCorrectly() {
            int expectedCheckSum = 544; //based on values from entryStr

            Assert.AreEqual(expectedCheckSum, entry.calcChkSum());
        }

        [TestMethod]
        public void convertStringTimeToDateTime_CreatesCorrectDateTimeObj() {
            DateTime expectedDateTime = createDateTime("7/8/2018 19:34:4"); //correctly formatted and based on values from entryStr
            
            //DateTime.Compare returns 0 if (DT1 == DT2)
            Assert.AreEqual(0, DateTime.Compare(expectedDateTime, entry.Timestamp));
        }

        [TestMethod]
        public void ToStringOvrRde_ToStringProvidesReplicaOfInput() {
            String expectedString = RemoveWhitespace(entryStr);
            String entryString = RemoveWhitespace(entry.ToString());


            Assert.AreEqual(expectedString, entryString);
        }

        public static DateTime createDateTime(String str) {
            return DateTime.Parse(str);
        }
        public static bool ApproximatelyEqualEpsilon(float a, float b, float epsilon)
        {
            const float floatNormal = (1 << 23) * float.Epsilon;
            float absA = Math.Abs(a);
            float absB = Math.Abs(b);
            float diff = Math.Abs(a - b);

            if (a == b)
            {
                // Shortcut, handles infinities
                return true;
            }

            if (a == 0.0f || b == 0.0f || diff < floatNormal)
            {    
                // a or b is zero, or both are extremely close to it.
                // relative error is less meaningful here
                return diff < (epsilon * floatNormal);
            }

            // use relative error
            return diff / Math.Min((absA + absB), float.MaxValue) < epsilon;
        }
        public static string RemoveWhitespace(string input)
         {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
         }
    }
}
