using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;

namespace WpfCursFormsDB
{
    public partial class AddWPFDiscipline : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlCommand Com;
        DataTable dT1;
        String strQ;
        SqlConnection sqlConn;

        public AddWPFDiscipline()
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

        /*private static string ToUTF8(string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }*/

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();

            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                String LastID, name, semester, idSpeciality, hoursLec, hoursLab, hoursPrac, hoursCurs, typeOfReporting;
                Data = new SqlDataAdapter("SELECT * FROM Disciplines", sqlConn);
                dT1 = new DataTable();
                Data.Fill(dT1);
                if (dT1.Rows.Count > 0)
                    LastID = (1 + Convert.ToInt32(dT1.Rows[dT1.Rows.Count - 1][0])).ToString();
                else
                    LastID = "1";

                name = Name.Text;
                semester = Semester.Text;
                idSpeciality = IDSpeciality.Text;
                hoursLab = HoursLab.Text;
                hoursLec = HoursLec.Text;
                hoursPrac = HoursPrac.Text;
                hoursCurs = HoursCurs.Text;
                typeOfReporting = TypeOfReporting.Text;

                strQ = "";
                strQ += "INSERT INTO Disciplines ";
                strQ += "VALUES ('" + LastID + "','" + name + "','" + semester + "','" + hoursLec +"','" + hoursLab + "','" + hoursPrac + "','" + hoursCurs + "','" + typeOfReporting + "');";
                Com = new SqlCommand(strQ, sqlConn);
                try { MessageBox.Show(Com.ExecuteNonQuery().ToString()); }
                catch { }

                String idDiscipline = LastID;
                Data = new SqlDataAdapter("SELECT * FROM Disciplines", sqlConn);
                dT1 = new DataTable();
                Data.Fill(dT1);
                if (dT1.Rows.Count > 0)
                    LastID = (1 + Convert.ToInt32(dT1.Rows[dT1.Rows.Count - 1][0])).ToString();
                else
                    LastID = "1";

                strQ = "";
                strQ += "INSERT INTO DisciplineSpeciality ";
                strQ += "VALUES ('" + LastID + "','" + idDiscipline + "','" + idSpeciality + "');";
                Com = new SqlCommand(strQ, sqlConn);

                try { MessageBox.Show(Com.ExecuteNonQuery().ToString()); }
                catch { }

                sqlConn.Close();
                ShowTable();
            }
        }

        

        /*private void IDField_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }*/

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
                Data = new SqlDataAdapter("SELECT * FROM Disciplines", sqlConn);
                dT1 = new DataTable("Disciplines");
                Data.Fill(dT1);
                DataGrid1.ItemsSource = dT1.DefaultView;
                sqlConn.Close();
            }
        }

        private void DataGrid1_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            String ID;

            DataGridColumn EditedCol = e.Column;
            MessageBox.Show(EditedCol.DisplayIndex.ToString());
            DataGridRow EditedRow = e.Row;
            Int32 Row = ((DataGrid)sender).ItemContainerGenerator.IndexFromContainer(EditedRow);
            Int32 Col = EditedCol.DisplayIndex;
            TextBox t = e.EditingElement as TextBox;

            String editedCellValue = t.Text.ToString();

            DataRowView row = (DataRowView)DataGrid1.SelectedItem;
            ID = row["IDDiscipline"].ToString();
            if (Col == 3)
                strQ = "UPDATE Disciplines SET HoursLec = '" + editedCellValue + "' WHERE IDDiscipline = " + ID + ";";
            else if (Col == 4)
                strQ = "UPDATE Disciplines SET HoursLab = '" + editedCellValue + "' WHERE IDDiscipline = " + ID + ";";
            else if (Col == 5)
                strQ = "UPDATE Disciplines SET HoursPrac = '" + editedCellValue + "' WHERE IDDiscipline = " + ID + ";";
            else if (Col == 6)
                strQ = "UPDATE Disciplines SET HoursCurs = '" + editedCellValue + "' WHERE IDDiscipline = " + ID + ";";
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
