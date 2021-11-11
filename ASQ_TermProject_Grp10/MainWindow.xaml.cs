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

            Server s = new Server();


            Thread thread = new Thread(() => s.RecieveTransmission(ref listTest, ref newListInsert));
            thread.Start();

            //s.RecieveTransmission(ref listTest, ref newListInsert);



            // When connected start printing the data as received
            Thread t = new Thread(new ThreadStart(InsertDataItem));
            t.Start();



            // send data to database


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

        public void InsertDataItem()
        {
            Console.WriteLine("inside");
            if (runThread)
            {
                if(newListInsert)
                {
                    liveDataGrid.Items.Clear();

                    for (int i = 0; i < listTest.Count; i++)
                    {
                        liveDataGrid.Items.Add(listTest[i]);
                    }   
                }
            }


            //liveDataGrid.ItemsSource = 
        }
    }
}
