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
    }
}
