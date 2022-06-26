using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace WpfCursFormsDB
{
    public partial class AddWPFSpeciality : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com;
        DataTable dT1;
        String strQ;
        SqlConnection sqlConn;

        public AddWPFSpeciality()
        {
            InitializeComponent();
            ShowTable();
        }

        private void ToStartWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }



        private void DataGrid1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowTable();
        }

        private void ShowTable()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();

            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter("SELECT * FROM Specialities", sqlConn);
                dT1 = new DataTable("Specialities");
                Data.Fill(dT1);
                DataGrid1.ItemsSource = dT1.DefaultView;
                sqlConn.Close();
            }
        }
    }
}
