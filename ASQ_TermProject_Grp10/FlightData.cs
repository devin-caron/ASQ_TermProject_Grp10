using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftTransmissionSystem
{
    class FlightData
    {
        private List<AircraftTelemetryEntry> telemetryList = new List<AircraftTelemetryEntry>();
        private String flightName = "";
        public FlightData(String name) {
            flightName = cleanName(name);
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

        //TODO: Make sure this funciton can handle varying flight names
        private String cleanName(String dirtyName) { 
            char[] dirtyNameArray = dirtyName.ToCharArray();
            char[] cleanNameArray = {'A','A','A','A','A','A'};

            try {
                for (int i = 0; i < dirtyName.IndexOf('.') - 1; i++) {
                    cleanNameArray[i] = dirtyNameArray[dirtyName.LastIndexOf('\\') + i + 1];
                }
            }
            catch(IndexOutOfRangeException ie) {
               Console.WriteLine("cleanName() hit an index out of range exception!"); 
            }

            return (new String(cleanNameArray));
        }

        public List<AircraftTelemetryEntry> TelemetryList {
            get => telemetryList;
            set => telemetryList = value;
        }
        public string FlightName => flightName;
    }
}
