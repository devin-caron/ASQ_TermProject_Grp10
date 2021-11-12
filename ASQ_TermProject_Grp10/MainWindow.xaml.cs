using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using AircraftTransmissionSystem;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Linq;

namespace ASQ_TermProject_Grp10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile bool liveData = true;
        private List<AircraftTelemetryEntry> listTest = new List<AircraftTelemetryEntry>();
        private Thread listener;

        // Incoming data from the client.  
        public static string data = null;

        public MainWindow()
        {
            InitializeComponent();

            dataSearch.IsReadOnly = true;
            searchBtn.IsEnabled = false;

            listener = new Thread(new ParameterizedThreadStart(RecieveTransmission));
            listener.Start();
         }

        private void RecieveTransmission(object o)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);


            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("[INFO] Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();

                    Console.WriteLine("[INFO] Connected.");
                    data = null;

                    // An incoming connection needs to be processed.  
                    while (true)
                    {
                        //C-FGAX|7_8_2018 19:36:34,-6E-06,-0.001649,-0.0008482153.744,605.7504,-0.028133,5.6E-05,|202
                        int bytesRec = handler.Receive(bytes);
                        data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                        else if (data.IndexOf("<EOT>") > -1)
                        {
                            break;
                        }

                        String[] splitPacket = data.Split('|');

                        AircraftTelemetryEntry entry = new AircraftTelemetryEntry(splitPacket[1], splitPacket[0]);

                        if (entry.calcChkSum() == int.Parse(splitPacket[2]))
                        {
                            //Sends acknowledgment data to ATTS   
                            byte[] msg = Encoding.ASCII.GetBytes("<ACK>");
                            handler.Send(msg);

                            listTest.Add(entry);
                        }
                    }

                    if(liveData) 
                    {
                        UpdateDataGrid();
                        
                        // update database function call
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private String[] splitData(String data)
        {
            String[] subStrings = data.Split('|');

            return new string[] { subStrings[0], subStrings[1], subStrings[2] };
        }

        private void UpdateDataGrid()
        {
            this.Dispatcher.Invoke(() =>
            {
                liveDataGrid.ItemsSource = listTest; 
            });
        }

        private void AsciiBtn_Click(object sender, RoutedEventArgs e)
        {
            using (TextWriter tw = new StreamWriter("Aircraft Telemetry.txt"))
            {
                foreach (var item in listTest)
                {
                    tw.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", item.Timestamp, item.Accelx.ToString(), item.Accely.ToString(), item.Accelz.ToString(),
                        item.Weight.ToString(), item.Altitude.ToString(), item.Pitch.ToString(), item.Bank.ToString()));
                }
            }
        }

        private void LiveDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (liveData)
            {
                // Stop Live Data
                liveData = false;
                dataSearch.IsReadOnly = false;
                searchBtn.IsEnabled = true;
                liveTxt.Text = "OFF";
            }
            else
            {
                // Start Live Data
                liveData = true;
                dataSearch.IsReadOnly = true;
                searchBtn.IsEnabled = false;
                liveTxt.Text = "ON";
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = dataSearch.Text;

            if (dataSearch.Text != "")
            {
                var filtered = listTest.Where(flightData => flightData.Timestamp.ToString().Contains(dataSearch.Text));
                string selItem = searchCB.Text;
                
                if (selItem == "timestamp")
                {
                    filtered = listTest.Where(flightData => flightData.Timestamp.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "accelx")
                {
                    filtered = listTest.Where(flightData => flightData.Accelx.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "accely")
                {
                    filtered = listTest.Where(flightData => flightData.Accely.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "accelz")
                {
                    filtered = listTest.Where(flightData => flightData.Accelz.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "weight")
                {
                    filtered = listTest.Where(flightData => flightData.Weight.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "altitude")
                {
                    filtered = listTest.Where(flightData => flightData.Altitude.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "pitch")
                {
                    filtered = listTest.Where(flightData => flightData.Pitch.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "bank")
                {
                    filtered = listTest.Where(flightData => flightData.Bank.ToString().Contains(dataSearch.Text));
                }

                liveDataGrid.ItemsSource = filtered;
            }
            else
            {
                liveDataGrid.ItemsSource = listTest;
            }
        }
    }
}
