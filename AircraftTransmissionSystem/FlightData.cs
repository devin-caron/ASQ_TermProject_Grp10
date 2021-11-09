using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftTransmissionSystem
{
    class FlightData
    {
        private static List<AircraftTelemetryEntry> telemetryList;
        private static String flightName = "";
        public FlightData(String name) {
            flightName = name;
        }

        public AircraftTelemetryEntry getEntry(int index) {
            if (index > telemetryList.Count) {
                AircraftTelemetryEntry ret = null;
                return ret;
            }

            return telemetryList[index];
        }

        public void addEntry(AircraftTelemetryEntry entry) {
            telemetryList.Add(entry);
        }


        public static List<AircraftTelemetryEntry> TelemetryList => telemetryList;
        public static string FlightName => flightName;
    }
}
