using System.Windows;

namespace WpfCursFormsDB
{    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitButn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void AddFaculty_Click(object sender, RoutedEventArgs e)
        {
            AddWPFFaculty window = new AddWPFFaculty();
            Hide();
            window.Show();
        }
        private void AddDepartment_Click(object sender, RoutedEventArgs e)
        {
            AddWPFDepartment window = new AddWPFDepartment();
            Hide();
            window.Show();
        }
        private void AddHead_Click(object sender, RoutedEventArgs e)
        {
            AddWPFHead window = new AddWPFHead();
            Hide();
            window.Show();
        }
        private void AddSpeciality_Click(object sender, RoutedEventArgs e)
        {
            AddWPFSpeciality window = new AddWPFSpeciality();
            Hide();
            window.Show();
        }
        private void AddDiscipline_Click(object sender, RoutedEventArgs e)
        {
            AddWPFDiscipline window = new AddWPFDiscipline();
            Hide();
            window.Show();
        }


        

        private void SQLButnForm_Click(object sender, RoutedEventArgs e)
        {
            SQL_WPF window = new SQL_WPF();
            Hide();
            window.Show();
        }

        private void RequestDepartDisc_Click(object sender, RoutedEventArgs e)
        {
            RequestDepartDisc window = new RequestDepartDisc();
            Hide();
            window.Show();
        }

		private void CountHours_Click(object sender, RoutedEventArgs e)
		{
            CountHours window = new CountHours();
            Hide();
            window.Show();

        }
	}
}
