using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;



namespace Dusan_Tmp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection conn;
        DbManager dbm;

        public MainWindow()
        {
            InitializeComponent();

            dbm = new DbManager();

        }


        private void Generate(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbm.ReadFromDb("[AttitudeParameters]");
            dg_Display.ItemsSource = dt.DefaultView;
        }

        private void GenerateGF(object sender, RoutedEventArgs e)
        {
            DataTable dt = dbm.ReadFromDb("[GForceParameters]");
            dg_Display.ItemsSource = dt.DefaultView;
        }



        private void Insert(object sender, RoutedEventArgs e)
        {

            //dbm.InsertAttitudeParameters("GABA", "5/2/1998 1:23:56 AM", 0.12, 0.13, 0.15);

        }

        private void InsertGF(object sender, RoutedEventArgs e)
        {
            //dbm.InsertGFParameters("GBBBA", "5/2/1998 1:23:56 AM", 0, 1, 2, 3200);
        }




    }

}
