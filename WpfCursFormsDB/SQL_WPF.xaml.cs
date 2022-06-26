using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace WpfCursFormsDB
{
    public partial class SQL_WPF : Window
    {
        private static String Connection = @"Data Source=LAPTOP-29IABVA8;Initial Catalog=CourseWork_DB;Integrated Security=True";
        SqlDataAdapter Data;
        SqlConnection sqlConn;

        DataTable dT, dTDepartments, dTFaculties, dTSpecialities, dTDisciplines;
        String strQ;

        public SQL_WPF()
        {
            InitializeComponent();
            GetItemsDepartments();
            GetItemsFaculties();
            GetItemsSpecialities();
            GetItemsDisciplines();
            query1(DisciplinesMoreSem);
            query8(view_3);
        }

        private void ToStartWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }

        private void query1(DataGrid Table)
        {
            /*** Назви дисциплін, які читаються більше одного семестру? ***/
            strQ = "";
            strQ += "SELECT Disciplines.Name ";
            strQ += "FROM Disciplines ";
            strQ += "GROUP BY Disciplines.Name ";
            strQ += "HAVING COUNT(*) > '1' ";
            strQ += "ORDER BY Disciplines.Name;";
            
            Query(Table, strQ);
        }

        private void query2(DataGrid Table, String strIndex)
        {
            /*** Загальна кількість годин, що відводяться на лабораторні роботи в одному із семестрів, проведення яких забезпечує певна кафедра? ***/
            strQ = "";
            
            strQ += "SELECT SUM(Disciplines.HoursLab)[Загальнка кількість годин на лабораторні роботи] ";
            strQ += "FROM Disciplines, DisciplineSpeciality, Specialities, DepartmentSpeciality, Departments ";
            strQ += "WHERE Disciplines.IDDiscipline = DisciplineSpeciality.IDDiscipline ";
            strQ += "AND DisciplineSpeciality.IDSpeciality = Specialities.IDSpeciality ";
            strQ += "AND Specialities.IDSpeciality = DepartmentSpeciality.IDSpeciality ";
            strQ += "AND DepartmentSpeciality.IDDepartment = Departments.IDDepartment ";
            strQ += "AND Departments.IDDepartment = '" + strIndex + "' ";
            strQ += "AND Disciplines.Semester = '" + Semester.Text + "';";
            Query(Table, strQ);
        }

        private void query3(DataGrid Table, String strIndex)
        {
            /*** Назва дисциплін, за якими проводяться лабораторні роботи на факультеті? ***/
            strQ = "";

            strQ += "SELECT Disciplines.Name[Назва дисципліни], Disciplines.Semester[Семестр], Disciplines.HoursLab[Год на лаб] ";
            strQ += "FROM Disciplines, DisciplineSpeciality, Specialities, DepartmentSpeciality, Departments, Faculties ";
            strQ += "WHERE Disciplines.IDDiscipline = DisciplineSpeciality.IDDiscipline ";
            strQ += "AND DisciplineSpeciality.IDSpeciality = Specialities.IDSpeciality ";
            strQ += "AND Specialities.IDSpeciality = DepartmentSpeciality.IDSpeciality ";
            strQ += "AND DepartmentSpeciality.IDDepartment = Departments.IDDepartment ";
            strQ += "AND Departments.IDFaculty = Faculties.IDFaculty ";
            strQ += "AND Faculties.IDFaculty = '" + strIndex + "' ";
            strQ += "AND Disciplines.HoursLab > 0 ";
            strQ += "GROUP BY Disciplines.Name, Disciplines.Semester, Disciplines.HoursLab;";
            
            Query(Table, strQ);
        }

        private void query4(DataGrid Table, String strIndex)
        {
            /*** Різниця в годинах, відведених з кожної дисципліни на лабораторні та практичні заняття в одному із семестрів на заданому факультеті? ***/
            strQ = "";

            strQ += "SELECT Disciplines.Name[Назва дисципліни], Disciplines.Semester[Семестр], Disciplines.HoursLab[Год на лаб], Disciplines.HoursPrac[Год на прак], Disciplines.HoursLab - Disciplines.HoursPrac AS[Різниця] ";
            strQ += "FROM Disciplines, DisciplineSpeciality, Specialities, DepartmentSpeciality, Departments, Faculties ";
            strQ += "WHERE Disciplines.IDDiscipline = DisciplineSpeciality.IDDiscipline ";
            strQ += "AND DisciplineSpeciality.IDSpeciality = Specialities.IDSpeciality ";
            strQ += "AND Specialities.IDSpeciality = DepartmentSpeciality.IDSpeciality ";
            strQ += "AND DepartmentSpeciality.IDDepartment = Departments.IDDepartment ";
            strQ += "AND Departments.IDFaculty = Faculties.IDFaculty ";
            strQ += "AND Faculties.IDFaculty = '" + strIndex + "' ";
            strQ += "GROUP BY Disciplines.Name, Disciplines.Semester, Disciplines.HoursLab, Disciplines.HoursPrac ";
            strQ += "ORDER BY Disciplines.Name;";

            Query(Table, strQ);
        }

        private void query5(DataGrid Table, String strIndex)
        {
            /*** Визначення дисциплін, за якими виконують курсові роботи студенти зазначеної спеціальності ***/
            strQ = "";

            strQ += "SELECT Disciplines.Name[Назва дисципліни], Disciplines.Semester[Семестр], Disciplines.HoursCurs[Годин на курсові роботи] ";
            strQ += "FROM Disciplines, DisciplineSpeciality, Specialities ";
            strQ += "WHERE Disciplines.IDDiscipline = DisciplineSpeciality.IDDiscipline ";
            strQ += "AND DisciplineSpeciality.IDSpeciality = Specialities.IDSpeciality ";
            strQ += "AND Specialities.IDSpeciality = '" + strIndex + "' ";
            strQ += "AND Disciplines.HoursCurs > '0' ";
            strQ += "ORDER BY Disciplines.Name;";

            Query(Table, strQ);
        }

        private void query6(DataGrid Table, String strIndex)
        {
            /*** Для яких спеціальностей читається зазначена дисципліна? ***/
            strQ = "";
            strQ += "SELECT Specialities.IDSpeciality[ID], Specialities.Name[Назва спеціальності] ";
            strQ += "FROM Disciplines, DisciplineSpeciality, Specialities ";
            strQ += "WHERE Specialities.IDSpeciality = DisciplineSpeciality.IDSpeciality ";
            strQ += "AND DisciplineSpeciality.IDDiscipline = Disciplines.IDDiscipline ";
            strQ += "AND Disciplines.IDDiscipline = '" + strIndex + "' ";
            strQ += "ORDER BY Specialities.IDSpeciality;";

            Query(Table, strQ);
        }


        private void query7(DataGrid Table, String strIndex)
        {
            /*** Яка кількість дисциплін входить до навчального плану підготовки студентів за вказаною спеціальністю, і скільки років здійснюється підготовка? ***/
            strQ = "";

            strQ += "SELECT COUNT(Disciplines.IDDiscipline) AS[Кількість дисциплін], Specialities.Duration / 2 AS [Тривалість] ";
            strQ += "FROM Disciplines, DisciplineSpeciality, Specialities ";
            strQ += "WHERE Specialities.IDSpeciality = DisciplineSpeciality.IDSpeciality ";
            strQ += "AND DisciplineSpeciality.IDDiscipline = Disciplines.IDDiscipline ";
            strQ += "AND Specialities.IDSpeciality = '" + strIndex + "' ";
            strQ += "GROUP BY Specialities.Duration;";

            Query(Table, strQ);
        }


        private void query8(DataGrid Table)
        {
            strQ = "";

            strQ += "SELECT Disciplines.Name AS[Назва дисципліни], Disciplines.Semester AS[Семестр] ";
            strQ += "FROM Disciplines, DisciplineSpeciality, Specialities, DepartmentSpeciality, Departments ";
            strQ += "WHERE Specialities.IDSpeciality = DisciplineSpeciality.IDSpeciality ";
            strQ += "AND DisciplineSpeciality.IDDiscipline = Disciplines.IDDiscipline ";
            strQ += "AND Specialities.IDSpeciality = DepartmentSpeciality.IDSpeciality ";
            strQ += "AND DepartmentSpeciality.IDDepartment = Departments.IDDepartment ";
            strQ += "GROUP BY Disciplines.Name, Disciplines.Semester ";
            strQ += "HAVING COUNT(Departments.IDDepartment) > '1';";

            Query(Table, strQ);
        }

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
        private void GetItemsFaculties()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            String[] Items = { "" };
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter("SELECT * FROM Faculties", sqlConn);
                dTFaculties = new DataTable("Faculties");
                Data.Fill(dTFaculties);

                Items = new String[dTFaculties.Rows.Count];
                for (int i = 0; i < dTFaculties.Rows.Count; i++)
                {
                    Items[i] = dTFaculties.Rows[i][0].ToString() + ". " + dTFaculties.Rows[i][1].ToString();
                }
            }
            sqlConn.Close();
            FacultiesCBox.ItemsSource = Items;
            FacultiesCBox.SelectedIndex = 0;
        }

        private void GetItemsSpecialities()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            String[] Items = { "" };
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter("SELECT * FROM Specialities", sqlConn);
                dTSpecialities = new DataTable("Specialities");
                Data.Fill(dTSpecialities);

                Items = new String[dTSpecialities.Rows.Count];
                for (int i = 0; i < dTSpecialities.Rows.Count; i++)
                {
                    Items[i] = dTSpecialities.Rows[i][0].ToString() + ". " + dTSpecialities.Rows[i][1].ToString();
                }
            }
            sqlConn.Close();
            SpecialitiesCBox.ItemsSource = Items;
            SpecialitiesCBox.SelectedIndex = 0;
            SpecialitiesCBox2.ItemsSource = Items;
            SpecialitiesCBox2.SelectedIndex = 0;
        }

        private void GetItemsDisciplines()
        {
            sqlConn = new SqlConnection(Connection);
            sqlConn.Open();
            String[] Items = { "" };
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                Data = new SqlDataAdapter("SELECT * FROM Disciplines", sqlConn);
                dTDisciplines = new DataTable("Disciplines");
                Data.Fill(dTDisciplines);

                Items = new String[dTDisciplines.Rows.Count];
                for (int i = 0; i < dTDisciplines.Rows.Count; i++)
                {
                    Items[i] = dTDisciplines.Rows[i][0].ToString() + ". " + dTDisciplines.Rows[i][1].ToString();
                }
            }
            sqlConn.Close();
            DisciplinesCBox.ItemsSource = Items;
            DisciplinesCBox.SelectedIndex = 0;
        }



        private void Apply2_Click(object sender, RoutedEventArgs e)
        {
            Int32 index = DepartmentsCBox.SelectedIndex;
            query2(HoursLab, dTDepartments.Rows[index][0].ToString());
        }


		private void Apply3_Click(object sender, RoutedEventArgs e)
        {
            Int32 index = FacultiesCBox.SelectedIndex;
            query3(DisciplinesLab, dTFaculties.Rows[index][0].ToString());
            query4(LabPracDiff, dTFaculties.Rows[index][0].ToString());
            
        }

        private void Apply5_Click(object sender, RoutedEventArgs e)
        {
            Int32 index = SpecialitiesCBox.SelectedIndex;
            query5(DisciplineCurs, dTSpecialities.Rows[index][0].ToString());
        }
        private void Apply6_Click(object sender, RoutedEventArgs e)
        {
            Int32 index = DisciplinesCBox.SelectedIndex;
            query6(DisciplineSpec, dTDisciplines.Rows[index][0].ToString());
        }

        private void Apply7_Click(object sender, RoutedEventArgs e)
        {
            Int32 index = SpecialitiesCBox2.SelectedIndex;
            query7(DiscipSpecTime, dTSpecialities.Rows[index][0].ToString());
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
