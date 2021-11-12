using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using AircraftTransmissionSystem;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace ASQ_TermProject_Grp10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private volatile bool runThread = false;
        private volatile bool newListInsert = false;

        private List<AircraftTelemetryEntry> listTest = new List<AircraftTelemetryEntry>();

        public MainWindow()
        {
            InitializeComponent();

            runThread = true;

            //Server s = new Server();


            //Thread thread = new Thread(() => RecieveTransmission(ref listTest, ref newListInsert));
            //thread.Start();

            //s.RecieveTransmission(ref listTest, ref newListInsert);

            // When connected start printing the data as received
            //Thread t = new Thread(new ThreadStart(InsertDataItem));
            //t.Start();

            // send data to database

            RecieveTransmission(ref listTest, ref newListInsert);
        }

        private void liveDataCheck_Checked(object sender, RoutedEventArgs e)
        {
            if (liveDataCheck.IsChecked == true)
            {
                // Restart Live Data

            }
            else
            {
                // Stop Live Data

            }

        }

        // Incoming data from the client.  
        public static string data = null;

        private void RecieveTransmission(ref List<AircraftTelemetryEntry> list, ref bool newListInsert)
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

                        AircraftTelemetryEntry entry = new AircraftTelemetryEntry(splitPacket[1]);


                        if (entry.calcChkSum() == int.Parse(splitPacket[2]))
                        {
                            //Sends acknowledgment data to ATTS   
                            byte[] msg = Encoding.ASCII.GetBytes("<ACK>");
                            handler.Send(msg);

                            //list.Add(entry);

                            // print data here
                            Console.WriteLine("GOT A MESSAGE!");

                            InsertDataItem();


                            //newListInsert = true;
                        }
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



        private void InsertDataItem()
        {
            Console.WriteLine("inside");

            testTxt.Text = "Working";
            //while (runThread)
            //{
            //if(newListInsert)
            //{
            //liveDataGrid.Items.Clear();

            //for (int i = 0; i < listTest.Count; i++)
            //{
            //liveDataGrid.Items.Add(listTest[i]);
            //}   
            // }
            //}
        }
    }
}
