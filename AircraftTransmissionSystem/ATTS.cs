using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AircraftTransmissionSystem
{
    class ATTS {
        private static List<FlightData> flights = new List<FlightData>();

        static void Main(string[] args) {
            readTelemetry();

            Client c = new Client();
            c.StartClient();
        }

        static void readTelemetry() {
            String[] filePaths;
            String flightDir = Path.GetFullPath(Path.Combine(@"..\..\..\AircraftTransmissionData\"));

            filePaths = Directory.GetFiles(flightDir); 
            int numOfFiles = filePaths.Length;

            //Console.WriteLine();

            for (int i = 0; i < numOfFiles; i++) {
                flights.Add(readFile(filePaths[i]));
                Console.WriteLine(flights[i].FlightName);
                Console.WriteLine(flights[i].TelemetryList.ElementAt(1).Timestamp.ToString());
            }       

            Console.ReadLine();
        }

    static FlightData readFile(String path) {
            String flightFilename = path.Substring(path.LastIndexOf('\\'));
            FlightData fd = new FlightData(flightFilename);
            
            try {
                using (StreamReader reader = new StreamReader(path)) {
                    String ln;

                    while ((ln = reader.ReadLine()) != null) {
                        if (ln == "") {
                            break;
                        }

                        AircraftTelemetryEntry newEntry = new AircraftTelemetryEntry(ln);
                        fd.addEntry(newEntry);
                    }
                }
            }
            catch(Exception e) {
                Console.WriteLine("ATTS::readFile() encountered: " + e.Message);
            }

            return fd;
        }
    
       
    }
}
