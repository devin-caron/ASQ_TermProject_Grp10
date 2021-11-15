/*
 *FILE         : MainWindow.xaml.cs
 *PROJECT      : SENG3020 - Milestone 2
 *PROGRAMMERS  : Devin Caron, Cole Spehar, Isaiah Andrews, Dusan Sasic
 *FIRST VERSON : 2021-11-09
 *DESCRIPTION  : This program models a Ground Station Terminal that receives live data
 *               updates from an aircraft. It parses this data then displays it live.
 *               The program also allows you to pause the live data and search it.
 *				
 */

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
using Microsoft.Win32;

namespace ASQ_TermProject_Grp10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GroundStationTerminalUI : Window
    {
        private volatile bool liveData = true;
        private List<AircraftTelemetryEntry> liveList = new List<AircraftTelemetryEntry>();
        private List<AircraftTelemetryEntry> pausedList = new List<AircraftTelemetryEntry>();
        private Thread listener;
        public static string data = null;

        public GroundStationTerminalUI()
        {
            InitializeComponent();

            // Set defaults
            dataSearch.IsReadOnly = true;
            searchBtn.IsEnabled = false;
            asciiBtn.IsEnabled = false;

            // Start server thread to listen for client
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
                        // C-FGAX|7_8_2018 19:36:34,-6E-06,-0.001649,-0.0008482153.744,605.7504,-0.028133,5.6E-05,|202
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

                        // Split the packet
                        String[] splitPacket = data.Split('|');

                        AircraftTelemetryEntry entry = new AircraftTelemetryEntry(splitPacket[1], splitPacket[0]);

                        // Verify CheckSum
                        if (entry.calcChkSum() == int.Parse(splitPacket[2]))
                        {
                            //Sends acknowledgment data to ATTS   
                            byte[] msg = Encoding.ASCII.GetBytes("<ACK>");
                            handler.Send(msg);

                            // Add entry to the list
                            liveList.Add(entry);
                        }

                        // If live data is on update the datagrid
                        if (liveData)
                        {
                            UpdateDataGrid(liveList);

                            // Update database function call
                            DbManager dbm = new DbManager();
                            dbm.InserAttitudeParameters(entry.TailCode, entry.Timestamp.ToString(), entry.Altitude, entry.Pitch, entry.Bank);
                            dbm.InsertGForceParameters(entry.TailCode, entry.Timestamp.ToString(), entry.Accelx, entry.Accely, entry.Accelz, entry.Weight);

                        }
                    }


                    // close connections
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void UpdateDataGrid(List<AircraftTelemetryEntry> data)
        {
            // Display updated list to datagrid
            this.Dispatcher.Invoke(() =>
            {
                liveDataGrid.ItemsSource = data;
                liveDataGrid.Items.Refresh();
            });
        }

        private void AsciiBtn_Click(object sender, RoutedEventArgs e)
        {

            // Setup save file dialog
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "AircraftData.txt";
            save.Filter = "Text File | *.txt";

            // Verify save was pressed
            if (save.ShowDialog() == true)
            {
                // Start streamwriter and transfer data to txt file
                StreamWriter writer = new StreamWriter(save.OpenFile());

                foreach (var item in pausedList)
                {
                    writer.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", item.Timestamp, item.Accelx.ToString(), item.Accely.ToString(), item.Accelz.ToString(),
                        item.Weight.ToString(), item.Altitude.ToString(), item.Pitch.ToString(), item.Bank.ToString()));
                }

                // close writer
                writer.Dispose();
                writer.Close();
            }
        }

        private void LiveDataBtn_Click(object sender, RoutedEventArgs e)
        {
            if (liveData)
            {
                // Stop Live Data
                pausedList = new List<AircraftTelemetryEntry>(liveList);
                liveData = false;
                asciiBtn.IsEnabled = true;
                dataSearch.IsReadOnly = false;
                searchBtn.IsEnabled = true;
                liveTxt.Text = "OFF";
            }
            else
            {
                // Start Live Data
                UpdateDataGrid(liveList);
                liveData = true;
                asciiBtn.IsEnabled = false;
                dataSearch.IsReadOnly = true;
                searchBtn.IsEnabled = false;
                liveTxt.Text = "ON";
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchValue = dataSearch.Text;

            // Search by column selected
            if (dataSearch.Text != "")
            {
                var filtered = pausedList.Where(flightData => flightData.Timestamp.ToString().Contains(dataSearch.Text));
                string selItem = searchCB.Text;

                if (selItem == "timestamp")
                {
                    filtered = pausedList.Where(flightData => flightData.Timestamp.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "tailCode")
                {
                    filtered = pausedList.Where(FlightData => FlightData.TailCode.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "accelx")
                {
                    filtered = pausedList.Where(flightData => flightData.Accelx.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "accely")
                {
                    filtered = pausedList.Where(flightData => flightData.Accely.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "accelz")
                {
                    filtered = pausedList.Where(flightData => flightData.Accelz.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "weight")
                {
                    filtered = pausedList.Where(flightData => flightData.Weight.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "altitude")
                {
                    filtered = pausedList.Where(flightData => flightData.Altitude.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "pitch")
                {
                    filtered = pausedList.Where(flightData => flightData.Pitch.ToString().Contains(dataSearch.Text));
                }
                else if (selItem == "bank")
                {
                    filtered = pausedList.Where(flightData => flightData.Bank.ToString().Contains(dataSearch.Text));
                }

                liveDataGrid.ItemsSource = filtered;
            }
            else
            {
                // display full list
                UpdateDataGrid(pausedList);
            }
        }
    }
}
