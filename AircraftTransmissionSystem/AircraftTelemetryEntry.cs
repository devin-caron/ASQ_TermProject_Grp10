using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AircraftTransmissionSystem
{
    class AircraftTelemetryEntry {
        private DateTime timestamp;
        private float accelx = 0.0f;
        private float accely = 0.0f;
        private float accelz = 0.0f;
        private float weight = 0.0f;
        private float altitude = 0.0f;
        private float pitch = 0.0f;
        private float bank = 0.0f;

        public AircraftTelemetryEntry(String telemetryLnStr) {

            if (telemetryLnStr == " ") {
                return;
            }

            parseTelemetryLn(telemetryLnStr);
            calcChkSum();
        }

        //TODO: add test that this function calculates correct check sums
        public int calcChkSum() {
            float subCheckSum = 0.0f;

            subCheckSum = (altitude + pitch + bank) / 3;

            return (int)Math.Round(subCheckSum);
        }


        //TODO: add test that determines that this function parses correctly
        private void parseTelemetryLn(String telemetryLnStr) {
            //7_8_2018 19:34:3,-0.319754, -0.716176, 1.797150, 2154.670410, 1643.844116, 0.022278, 0.033622,
            String[] subStrings = telemetryLnStr.Split(',');
            
            timestamp = convertStringTimeToDateTime(subStrings[0]);
            accelx = float.Parse(subStrings[1]);
            accely = float.Parse(subStrings[2]);
            accelz = float.Parse(subStrings[3]);
            weight = float.Parse(subStrings[4]);
            altitude = float.Parse(subStrings[5]);
            pitch = float.Parse(subStrings[6]);
            bank = float.Parse(subStrings[7]);
        }

        //TODO: add test that detemines if this function creates correct timestamps
        private DateTime convertStringTimeToDateTime(String strTimeStamp) {
            //7_8_2018 19:34:3

            char[] properStringFormat = strTimeStamp.ToCharArray();

            int firstUnderscore = strTimeStamp.IndexOf('_',0);
            int secondUnderscore = strTimeStamp.IndexOf('_',firstUnderscore +1);

            properStringFormat[firstUnderscore] = '/';
            properStringFormat[secondUnderscore] = '/';


            return DateTime.Parse(new string(properStringFormat));
        }

        private String convertDateTimeToString(DateTime dt) {
            return dt.ToString("M_dd_yyyy HH:mm:ss");
        }

        //TODO: add test to verify that this funciton can make an exact replica of the string that was passed into this object's constructor (excluding the datetime field) 
        public override String ToString() {
            StringBuilder sb = new StringBuilder();

            sb.Append(convertDateTimeToString(timestamp) +  @",");
            sb.Append(accelx + @"," + accely + @"," + accelz);
            sb.Append(weight + @"," + altitude + @"," + pitch + @"," + bank + @",");

            return sb.ToString();
        }

        public DateTime Timestamp => timestamp;
        public float Accelx => accelx;
        public float Accely => accely;
        public float Accelz => accelz;
        public float Weight => weight;
        public float Altitude => altitude;
        public float Pitch => pitch;
        public float Bank => bank;
    }
}
