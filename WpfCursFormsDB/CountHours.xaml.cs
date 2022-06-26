using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace WpfCursFormsDB
{
    public partial class CountHours : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com;
        DataTable dT1;
        String strQ;
        SqlConnection sqlConn;

        public CountHours()
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
        private void DataGrid2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowTable();
        }

        private void ShowTable()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            strQ = "";
            strQ += "SELECT Departments.Name[Назва кафедри], SUM(Disciplines.HoursLec)[Год на лек], SUM(Disciplines.HoursLab)[Год на лаб], SUM(Disciplines.HoursPrac)[Год на прак], SUM(Disciplines.HoursCurs)[Год на курсову] ";
            strQ += "FROM Disciplines, Specialities, Departments, DisciplineSpeciality, DepartmentSpeciality ";
            strQ += "WHERE Departments.IDDepartment = DepartmentSpeciality.IDDepartment ";
            strQ += "AND Disciplines.IDDiscipline = DisciplineSpeciality.IDDiscipline ";
            strQ += "AND Specialities.IDSpeciality = DisciplineSpeciality.IDSpeciality ";
            strQ += "AND Specialities.IDSpeciality = DepartmentSpeciality.IDSpeciality ";
            strQ += "GROUP BY Departments.Name ";
            strQ += "ORDER BY Departments.Name;";
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter(strQ, sqlConn);
                dT1 = new DataTable();
                Data.Fill(dT1);
                DataGrid1.ItemsSource = dT1.DefaultView;
                sqlConn.Close();
            }
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            strQ = "";
            strQ += "SELECT 'KPI'[Інститут], SUM(Disciplines.HoursLec)[Год на лек], SUM(Disciplines.HoursLab)[Год на лаб], SUM(Disciplines.HoursPrac)[Год на прак], SUM(Disciplines.HoursCurs)[Год на курсову] ";
            strQ += "FROM Disciplines, Specialities, Departments, DisciplineSpeciality, DepartmentSpeciality ";
            strQ += "WHERE Departments.IDDepartment = DepartmentSpeciality.IDDepartment ";
            strQ += "AND Disciplines.IDDiscipline = DisciplineSpeciality.IDDiscipline ";
            strQ += "AND Specialities.IDSpeciality = DisciplineSpeciality.IDSpeciality ";
            strQ += "AND Specialities.IDSpeciality = DepartmentSpeciality.IDSpeciality; ";
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter(strQ, sqlConn);
                dT1 = new DataTable();
                Data.Fill(dT1);
                DataGrid2.ItemsSource = dT1.DefaultView;
                sqlConn.Close();
            }
        }
    }
}
