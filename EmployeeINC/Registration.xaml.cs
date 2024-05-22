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
using System.Xml.Linq;
using EmployeeINC.Database.Tables;

namespace EmployeeINC
{
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
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

        private void reg_click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(login.Text) && !string.IsNullOrEmpty(Parol.Text))
            {
                User account = new User
                {
                    Login = login.Text,
                    Password = Parol.Text
                };
                DB.Database.Query($"INSERT INTO User(Login, Password, role) " +
                                  $"VALUES ('{account.Login}', '{account.Password}', 'guest');");
                App.CurrentUser = (User)new User().ConvertToTable(DB.Database
                    .ExecuteQuery($"SELECT * FROM User " +
                                  $"WHERE Login = '{account.Login}' AND Password = '{account.Password}'")
                    .FirstOrDefault());
                MessageBox.Show("Регистрация прошла успешно!");
                TransitionWindowToMain();
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
            }
        }

        private void TransitionWindowToMain()
        {
            MainWindow men = new MainWindow();
            men.Show();
            Close();
        }
    }
}