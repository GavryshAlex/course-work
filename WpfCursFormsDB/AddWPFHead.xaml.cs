using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace WpfCursFormsDB
{
    public partial class AddWPFHead : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com;
        DataTable dT1;
        String strQ;
        SqlConnection sqlConn;

        public AddWPFHead()
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
                Data = new SqlDataAdapter("SELECT * FROM Head", sqlConn);
                dT1 = new DataTable("Head");
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
            ID = row["IDHead"].ToString();
            if (Col == 1)
                strQ = "UPDATE Head SET Surname = '" + editedCellValue + "' WHERE IDHead = " + ID + ";";
            else if (Col == 2)
                strQ = "UPDATE Head SET Name = '" + editedCellValue + "' WHERE IDHead = " + ID + ";";

            else if (Col == 3)
                strQ = "UPDATE Head SET Middlename = '" + editedCellValue + "' WHERE IDHead = " + ID + ";";

            else if (Col == 4)
                strQ = "UPDATE Head SET Degree = '" + editedCellValue + "' WHERE IDHead = " + ID + ";";

            else if (Col == 5)
                strQ = "UPDATE Head SET Rank = '" + editedCellValue + "' WHERE IDHead = " + ID + ";";
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
