using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace WpfCursFormsDB
{
    public partial class AddWPFDepartment : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com;
        DataTable dT1;
        String strQ;
        SqlConnection sqlConn;

        public AddWPFDepartment()
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
                Data = new SqlDataAdapter("SELECT Departments.IDDepartment, Departments.Name[Кафедра], Departments.PhoneNumber[Телефон], Faculties.Name[Факультет], Head.Surname + ' ' + Head.Name + ' ' + Head.Middlename AS [Завідувач] FROM Departments, Faculties, Head WHERE Departments.IDDepartment = Faculties.IDFaculty AND Departments.IDHead = Head.IDHead", sqlConn);
                dT1 = new DataTable("Departments");
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
            ID = row["IDDepartment"].ToString();
            if (Col == 1)
                strQ = "UPDATE Departments SET Name = '" + editedCellValue + "' WHERE IDDepartment = " + ID + ";";
            else if (Col == 2)
                strQ = "UPDATE Departments SET PhoneNumber = '" + editedCellValue + "' WHERE IDDepartment = " + ID + ";";
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
