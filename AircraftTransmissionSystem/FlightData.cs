/*
 * FILE         : FlightData.cs
 * PROJECT      : SENG3020 - Milestone 2
 * PROGRAMMERS  : Devin Caron, Cole Spehar, Isaiah Andrews, Dusan Sasic
 * FIRST VERSON : 2021-11-09
 * DESCRIPTION  : An object created from this class contains a string representing the tail code of the plane the data has come from,
 *                and a list of all of the telemetry records.
 */

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

        /*
          FUNCTION: FlightData() - CONSTRUCTOR
          DESCRIPTION: This constructor cleans the flight name, and constructs the object.
          PARAMETERS: String name : The tail code of the airplane where the date is coming from.
          RETURNS: void
        */
        public FlightData(String name) {
            flightName = cleanName(name);
        }

        /*
          FUNCTION: getEntry()
          DESCRIPTION: This function returns the AircraftTransmissionEntry at the given index.
          PARAMETERS: int index : The requested index.
          RETURNS: AircraftTelemetryEntry : The requested AircraftTelemetryEntry.
        */ 
        public AircraftTelemetryEntry getEntry(int index) {
            if (index > telemetryList.Count) {
                AircraftTelemetryEntry ret = null;
                return ret;
            }

            return telemetryList[index];
        }

        /*
          FUNCTION: addEntry()
          DESCRIPTION: This function 
          PARAMETERS: The function takes an AircraftTelemetryEntry object and adds it to the telemetryList.
          RETURNS: void
        */
        public void addEntry(AircraftTelemetryEntry entry) {
            telemetryList.Add(entry);
        }

        /*
          FUNCTION: cleanName()
          DESCRIPTION: This function 'cleans' the string containing the airplane tail code.
          PARAMETERS: String dirtyName : The 'dirty name' to be cleaned.
          RETURNS: String : A 'cleaned', read: with no superflous symbols, string.
        */
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
