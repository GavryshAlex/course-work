using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace WpfCursFormsDB
{
    public partial class RequestDepartDisc : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com;
        DataTable dT, dTDepartments;
        String strQ;
        SqlConnection sqlConn;

        public RequestDepartDisc()
        {
            InitializeComponent();
            GetItemsDepartments();
        }

        private void ToStartWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }



        /*private void DataGrid1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowTable_Click(sender, e);
        }*/

        /*private void ShowTable_Click(object sender, RoutedEventArgs e)
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();

            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter("SELECT * FROM Specialities", sqlConn);
                dT = new DataTable("Specialities");
                Data.Fill(dT);
                DataGrid1.ItemsSource = dT.DefaultView;
                sqlConn.Close();
            }
        }*/
        private void GetItemsDepartments()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            String[] Items = { "" };
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter("SELECT * FROM Departments", sqlConn);
                dTDepartments = new DataTable("Departments");
                Data.Fill(dTDepartments);

                Items = new String[dTDepartments.Rows.Count];
                for (int i = 0; i < dTDepartments.Rows.Count; i++)
                {
                    Items[i] = dTDepartments.Rows[i][0].ToString() + ". " + dTDepartments.Rows[i][1].ToString();
                }
            }
            sqlConn.Close();
            DepartmentsCBox.ItemsSource = Items;
            DepartmentsCBox.SelectedIndex = 0;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
		{
            Int32 index = DepartmentsCBox.SelectedIndex;
            query(DataGrid1, dTDepartments.Rows[index][0].ToString());
        }
        private void query(DataGrid Table, String strIndex)
        {
            /*** Різниця в годинах, відведених з кожної дисципліни на лабораторні та практичні заняття в одному із семестрів на заданому факультеті? ***/
            strQ = "";

            strQ += "SELECT Disciplines.Name[Назва дисципліни], Disciplines.Semester[Семестр], Specialities.IDSpeciality[ID спеціальності], Specialities.Name[Назва спеціальності], Disciplines.HoursLec[Год на лек], Disciplines.HoursLab[Год на лаб], Disciplines.HoursPrac[Год на прак], Disciplines.HoursCurs[Год на курсову] ";
            strQ += "FROM Disciplines, Specialities, Departments, DisciplineSpeciality, DepartmentSpeciality ";
            strQ += "WHERE Departments.IDDepartment = DepartmentSpeciality.IDDepartment ";
            strQ += "AND Disciplines.IDDiscipline = DisciplineSpeciality.IDDiscipline ";
            strQ += "AND Specialities.IDSpeciality = DisciplineSpeciality.IDSpeciality ";
            strQ += "AND Specialities.IDSpeciality = DepartmentSpeciality.IDSpeciality ";
            strQ += "AND Departments.IDDepartment = '" + strIndex + "' ";
            strQ += "ORDER BY Disciplines.Name, Disciplines.Semester;";

            Query(Table, strQ);
        }
        private void Query(DataGrid Table, string Str)
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            Data = new SqlDataAdapter(Str, sqlConn);
            dT = new DataTable(Str);
            Data.Fill(dT);
            Table.ItemsSource = dT.DefaultView;
            sqlConn.Close();
        }
    }
}
