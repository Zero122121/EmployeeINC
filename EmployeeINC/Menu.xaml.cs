using System;
using System.Collections.Generic;
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
using EmployeeINC.Windows;

namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
            ValidateSettings(App.CurrentUser.role);
        }

        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = new Employee();
            employee.Show();
            Close();
        }

        private void Vacations_Click(object sender, RoutedEventArgs e)
        {
            Vacations v = new Vacations();
            v.Show();
            Close();
        }

        private void Hospitals_Click(object sender, RoutedEventArgs e)
        {
            hospitals s = new hospitals();
            s.Show();
            Close();
        }

        private void dol_click(object sender, RoutedEventArgs e)
        {

        }

        private void do_Click(object sender, RoutedEventArgs e)
        {
            dolshnosti s = new dolshnosti();
            s.Show();
            Close();
        }

        private void departments_Click(object sender, RoutedEventArgs e)
        {
            Departments s = new Departments();
            s.Show();
            Close();
        }

        private void statistic_Click(object sender, RoutedEventArgs e)
        {
            EmployeeStats s = new EmployeeStats();
            s.Show();
            Close();
        }

        private void ValidateSettings(string role)
        {
            switch (role)
            {
                case "guest":
                    СотрудникиButton.Visibility = Visibility.Visible;
                    ОтпускиButton.Visibility = Visibility.Visible;
                    БольничныеButton.Visibility = Visibility.Visible;
                    ДолжностиButton.Visibility = Visibility.Visible;
                    ОтделыButton.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
