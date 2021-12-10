using System;
using System.Collections.Generic;
using System.Text;

namespace FDMScommonLib {
    public class AircraftTelemetryEntry {
        private DateTime timestamp;
        private String tailCode = "";
        private float accelx = 0.0f;
        private float accely = 0.0f;
        private float accelz = 0.0f;
        private float weight = 0.0f;
        private float altitude = 0.0f;
        private float pitch = 0.0f;
        private float bank = 0.0f;


        /*
          FUNCTION: AircraftTelemetryEntry() - CONSTRUCTOR
          DESCRIPTION: This constructor takes a string representing the aircraft's state
                       and uses it to populate the objects attributes.
          PARAMETERS: String telemetryLnStr: String representing the aircraft's state.
          RETURNS: void
        */
        public AircraftTelemetryEntry(String telemetryLnStr, String flightName) {

            if (telemetryLnStr == " ") {
                return;
            }

            tailCode = flightName;
            parseTelemetryLn(telemetryLnStr);
        }
        /*
          FUNCTION: calcChkSum()
          DESCRIPTION: This function calculates the check sum using the data stored in this object.
          PARAMETERS: void
          RETURNS: int : The resultant check sum, after being rounded.
        */
        //TODO: add test that this function calculates correct check sums
        public int calcChkSum() {
            float subCheckSum = 0.0f;

            subCheckSum = (altitude + pitch + bank) / 3;

            return (int)Math.Round(subCheckSum);
        }

        /*
          FUNCTION: parseTelemetryLn() 
          DESCRIPTION: This function parses the string that was sent into the constructor.
          PARAMETERS: String telemetryLnStr : A single line from the airplane's state file.
          RETURNS: void
        */
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

        /*
          FUNCTION: convertStringTimeToDateTime()
          DESCRIPTION: This function converts the file's version of timestamps into 
                       .NET's DateTime object.
          PARAMETERS: String strTimeStamp : The string time stamp.
          RETURNS: DateTime : The proper DateTime object, set with the timestamp from the data.
        */
        private DateTime convertStringTimeToDateTime(String strTimeStamp) {
            //7_8_2018 19:34:3

            char[] properStringFormat = strTimeStamp.ToCharArray();

            int firstUnderscore = strTimeStamp.IndexOf('_', 0);
            int secondUnderscore = strTimeStamp.IndexOf('_', firstUnderscore + 1);

            properStringFormat[firstUnderscore] = '/';
            properStringFormat[secondUnderscore] = '/';


            return DateTime.Parse(new string(properStringFormat));
        }

        /*
          FUNCTION: convertDateTimeToString()
          DESCRIPTION: This function does the reverse of the previous function.
          PARAMETERS: DateTime dt: The timestamp in DateTime format.
          RETURNS: String : A timestamp string the format of the original data text files.
        */
        private String convertDateTimeToString(DateTime dt) {
            return dt.ToString("M_d_yyyy HH:mm:s");
        }

        /*
          FUNCTION: ToString()
          DESCRIPTION: This is an override of the ToString function that makes the string representation 
                       of this object a line from the original aircraft data text file.
          PARAMETERS: void
          RETURNS: String : Representation of this object as line from the original aircraft data text file.
        */
        public override String ToString() {
            StringBuilder sb = new StringBuilder();

            sb.Append(convertDateTimeToString(timestamp) + @",");
            sb.Append(accelx + @"," + accely + @"," + accelz + @",");
            sb.Append(weight + @"," + altitude + @"," + pitch + @"," + bank + @",");

            return sb.ToString();
        }

        public DateTime Timestamp => timestamp;
        public String TailCode => tailCode;
        public float Accelx => accelx;
        public float Accely => accely;
        public float Accelz => accelz;
        public float Weight => weight;
        public float Altitude => altitude;
        public float Pitch => pitch;
        public float Bank => bank;
    }

    public class FlightData {
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
        public String cleanName(String dirtyName) {
            char[] dirtyNameArray = dirtyName.ToCharArray();
            char[] cleanNameArray = { 'A', 'A', 'A', 'A', 'A', 'A' };

            try {
                for (int i = 0; i < dirtyName.IndexOf('.') - 1; i++) {
                    cleanNameArray[i] = dirtyNameArray[dirtyName.LastIndexOf('\\') + i + 1];
                }
            }
            catch (IndexOutOfRangeException ie) {
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
