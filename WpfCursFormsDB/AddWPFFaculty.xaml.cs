using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace WpfCursFormsDB
{
    public partial class AddWPFFaculty : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com;
        DataTable dT1;
        String strQ;
        SqlConnection sqlConn;

        public AddWPFFaculty()
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
                Data = new SqlDataAdapter("SELECT * FROM Faculties", sqlConn);
                dT1 = new DataTable("Faculties");
                Data.Fill(dT1);
                DataGrid1.ItemsSource = dT1.DefaultView;
                sqlConn.Close();
            }
        }

        private void DataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            String ID;

            DataGridColumn EditedCol = e.Column;
            DataGridRow EditedRow = e.Row;
            Int32 Row = ((DataGrid)sender).ItemContainerGenerator.IndexFromContainer(EditedRow);
            Int32 Col = EditedCol.DisplayIndex;
            TextBox t = e.EditingElement as TextBox;

            String editedCellValue = t.Text.ToString();

            DataRowView row = (DataRowView)DataGrid1.SelectedItem;
            ID = row["IDFaculty"].ToString();
            if (Col == 1)
                strQ = "UPDATE Faculties SET Name = '" + editedCellValue + "' WHERE IDFaculty = " + ID + ";";
            try
            {
                sqlConn = new SqlConnection(Connection);
                sqlConn.Open();
                Com = new SqlCommand(strQ, sqlConn);
                MessageBox.Show(Com.ExecuteNonQuery().ToString());
                sqlConn.Close();
            }
            catch { }
        }
    }
}
