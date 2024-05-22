using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using EmployeeINC.Database.Tables;

namespace EmployeeINC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DB.Database.Connect();
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
            User user = (User)new User().ConvertToTables(DB.Database.ExecuteQuery(
                    $"SELECT * FROM User WHERE Login = '{login.Text}'")).FirstOrDefault();
            if (user != null)
            {
                MessageBox.Show("Такой пользователь уже есть!");
                return;
            }

            DB.Database.ExecuteQuery($"INSERT INTO User (Login, Password, role) VALUES ('{login.Text}', '{Parol.Text}', 'guest')");
            TransitionWindowToMain();
        }

        private void loginn_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)new User().ConvertToTables(DB.Database.ExecuteQuery(
                    $"SELECT * FROM User WHERE Login = '{login.Text}' AND Password = '{Parol.Text}'"))
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