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
using EmployeeINC.Database.Tables;

namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для AddWindoww.xaml
    /// </summary>
    public partial class AddWindoww : Window
    {
        private Должности _employee = null;

        public AddWindoww(Должности сотрудники = null)
        {
            InitializeComponent();
            if (сотрудники != null)
            {
                LoadData();
                _employee = сотрудники;
            }
        }

        private void LoadData()
        {
            LastName.Text = _employee.Наименование;
            Surname.Text = Convert.ToString(_employee.Ставка);
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (LastName.Text == "Введите название")
            {
                LastName.Text = "";
                LastName.Foreground = Brushes.White;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LastName.Text))
            {
                LastName.Text = "Введите название";
                LastName.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText1(object sender, EventArgs e)
        {
            if (Surname.Text == "Введите ставку")
            {
                Surname.Text = "";
                Surname.Foreground = Brushes.White;
            }
        }

        private void AddText1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Surname.Text))
            {
                Surname.Text = "Введите ставку";
                Surname.Foreground = Brushes.Gray;
            }
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Должности д = new Должности
            {
                Наименование = LastName.Text,
                Ставка = Convert.ToInt32(Surname.Text)
            };

            DB.Database.Query($"INSERT INTO Должности (Наименованиеи, Ставка) VALUES ('{д.Наименование}', {д.Ставка})");

            MessageBox.Show("Должность была добавлен в базу");
            dolshnosti em = new dolshnosti();
            em.Show();
            Close();
        }
    }
}