using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftTransmissionSystem
{
    class AircraftTelemetryEntry {
        private DateTime Timestamp;
        private static float accelx = 0.0f;
        private static float accely = 0.0f;
        private static float accelz = 0.0f;
        private static float weight = 0.0f;
        private static float altitude = 0.0f;
        private static float pitch = 0.0f;
        private static float bank = 0.0f;

        public AircraftTelemetryEntry(String telemetryLnStr) {
            parseTelemetryLn(telemetryLnStr);
            calcChkSum();
        }

        //TODO: add test that this function calculates correct check sums
        private static int calcChkSum() {
            float subCheckSum = 0.0f;

            subCheckSum = (altitude + pitch + bank) / 3;

            return (int)Math.Round(subCheckSum);
        }

        //TODO: add test that determines that this function parses correctly
        private static void parseTelemetryLn(String telemetryLnStr) {
            //7_8_2018 19:34:3,-0.319754, -0.716176, 1.797150, 2154.670410, 1643.844116, 0.022278, 0.033622,


        } 

    }
}
