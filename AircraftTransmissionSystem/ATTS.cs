using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AircraftTransmissionSystem
{
    class ATTS {
        private static List<FlightData> flights = new List<FlightData>();
        private static bool bDataRetrieved = false;

        static void Main(string[] args) {
           int menuOption = 0; 


           printMenu();

            while (true) { 
                printMenu();

                try {
                    menuOption = int.Parse(Console.ReadLine());
                }
                catch {
                    menuOption = 0;
                    continue;
                }
                

                switch (menuOption) {
                    case 1:
                        Console.Clear();

                        if(bDataRetrieved)
                        {
                            Console.WriteLine("[WARN] Data already retrieved! Ignoring.");
                            Thread.Sleep(1500);
                            break;
                        }

                        readTelemetry();
                        bDataRetrieved = true;
                        break;
                    case 2:
                        Console.Clear();
                        if (!bDataRetrieved) {
                            Console.WriteLine("[ERROR] There is no data to transmit! Have you downloaded the data yet?");
                            Thread.Sleep(2500);
                            continue;
                        }

                        StartTransmission();
                        break;
                    case 3:
                        return;
                    default:
                        continue;
                }
            }
        }

        static void readTelemetry() {
            String[] filePaths;
            String flightDir = Path.GetFullPath(Path.Combine(@"..\..\..\AircraftTransmissionData\"));

            filePaths = Directory.GetFiles(flightDir); 
            int numOfFiles = filePaths.Length;


            for (int i = 0; i < numOfFiles; i++) {
                flights.Add(readFile(filePaths[i]));
                Console.WriteLine("[INFO] Downloading data from Aircraft '{0}' flight start '{1}'", flights[i].FlightName, flights[i].TelemetryList.ElementAt(1).Timestamp.ToString());
                Thread.Sleep(400);
            }       
        }

        static FlightData readFile(String path) {
            String flightFilename = path.Substring(path.LastIndexOf('\\'));
            FlightData fd = new FlightData(flightFilename);
            
            try {
                using (StreamReader reader = new StreamReader(path)) {
                    String ln;

                    while ((ln = reader.ReadLine()) != null) {
                        if (ln == " ") {
                            //Console.WriteLine("[WARN] Read empty line. Ignoring data.");
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
    
        static void printMenu() {
            Console.Clear();
            Console.WriteLine(" === Aircraft Transmission System Terminal ===");
            Console.WriteLine("\t1) Download Telemetry");
            Console.WriteLine("\t2) Transmit All Flight Data");
            Console.WriteLine("\t3) Exit");
        }

        static void StartTransmission() {


            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily, 
                    SocketType.Stream, ProtocolType.Tcp);

                Console.WriteLine("[INFO] Attempting to connect to the Ground Station Terminal.");

                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                   
                    sender.Connect(remoteEP);

                    Console.WriteLine("[INFO] Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                    int totalBytesSent = 0;
                    

                    for(int i = 0; i < flights.Count; i++) {

                        Console.WriteLine("[INFO] Transmitting data from flight '{0}'", flights[i].FlightName);

                        for (int y = 0; y < flights[i].TelemetryList.Count; y++) {
                            sendString(ref totalBytesSent, sender, packetize(flights[i].FlightName, flights[i].TelemetryList.ElementAt(y)));
                            Thread.Sleep(1000);// This line attempts to simulate real-world transfer speeds.
                        }

                    }
                   
                    
                    sendString(ref totalBytesSent, sender, "<EOF>");


                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    Console.WriteLine("[INFO] Transmission completed, sent: {0} bytes.", totalBytesSent);
                    Thread.Sleep(2500);
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    Thread.Sleep(2500);
                }
                catch (SocketException se)
                {
                    Console.WriteLine("[ERROR] ATTS could not connect to the Ground Station Terminal. Is it online?");
                    Thread.Sleep(2500);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    Thread.Sleep(2500);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Thread.Sleep(2500);
            }
        }

        static void sendString(ref int bytesSent, Socket socket, String strMsg) {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
             

            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes(strMsg);

            // Send the data through the socket.  
            bytesSent += socket.Send(msg);

            // Receive the response from the remote device.  
            int bytesRec = socket.Receive(bytes);
        }
		
		static String packetize(String flightName, AircraftTelemetryEntry currentEntry) {
			StringBuilder sb = new StringBuilder();

            sb.Append(flightName + @"|");
            sb.Append(currentEntry.ToString() + @"|");
            sb.Append(currentEntry.calcChkSum());


            return sb.ToString();
		}
	}
}
