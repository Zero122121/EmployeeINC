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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeINC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


        }

       

        private void TransitionWindowToMain()
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            var user = DataAcEntities1.GetContext().User
                .Where(u => u.Password == Parol.Text && u.Login == login.Text)
                .Select(u => u)
                .FirstOrDefault();
            if (user != null)
            {
                TransitionWindowToMain();
            }
        }

        private void loginn_Click(object sender, RoutedEventArgs e)
        {
            var user = DataAcEntities1.GetContext().User
                .Where(u => u.Password == Parol.Text && u.Login == login.Text)
                .Select(u => u)
                .FirstOrDefault();
            if (user != null)
            {
                TransitionWindowToMain();
            }
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (login.Text == "Введите логин")
            {
                login.Text = "";
                login.Foreground = Brushes.White;
            }
        }
        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(login.Text))
            {
                login.Text = "Введите логин";
                login.Foreground = Brushes.Gray;
            }
        }

        private void RemovText(object sender, EventArgs e)
        {
            if (Parol.Text == "Введите пароль")
            {
                Parol.Text = "";
                Parol.Foreground = Brushes.White;
            }
        }
        private void AdText(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(Parol.Text))
            {
                Parol.Text = "Введите пароль";
                Parol.Foreground = Brushes.Gray;
            }
        }
    }
   
}

