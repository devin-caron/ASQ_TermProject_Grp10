using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AircraftTransmissionSystem;
using System.Threading;

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

            Server s = new Server();

            runThread = true;


            s.RecieveTransmission(ref listTest, ref newListInsert);



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
