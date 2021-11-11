using System;
using System.Collections.Generic;
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

namespace ASQ_TermProject_Grp10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class FlightData
        {
            public DateTime Timestamp { get; set; }
            public int AccelX { get; set; }
            public double AccelY { get; set; }
            public double AccelZ { get; set; }
            public double Weight { get; set; }
            public double Altitude { get; set; }
            public double Pitch { get; set; }
            public double Bank { get; set; }
        }


        public MainWindow()
        {
            InitializeComponent();

            Server s = new Server();
            //s.StartListening();
            List<FlightData> flightData = new List<FlightData>();
            flightData.Add(new FlightData()
            {
                Timestamp = new DateTime(1996, 07, 04),
                AccelX = 1,
                AccelY = 1,
                AccelZ = 1,
                Weight = 1,
                Altitude = 1,
                Pitch = 1,
                Bank = 1

            });

            liveDataGrid.ItemsSource =flightData;


            flightData.Add(new FlightData()
            {
                Timestamp = new DateTime(1996, 07, 04),
                AccelX = 2,
                AccelY = 1,
                AccelZ = 1,
                Weight = 1,
                Altitude = 1,
                Pitch = 1,
                Bank = 1

            });

            liveDataGrid.ItemsSource = flightData;
            // When connected start printing the data as received




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
    }
}
