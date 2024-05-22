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
    public partial class AddWindow : Window
    {
        private Сотрудники _employee;

        public AddWindow(Сотрудники сотрудники = null)
        {
            InitializeComponent();
            var отделы = 
                (Отделы[])new Отделы().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Отделы"));
            ComboOtdel.ItemsSource = отделы.Select(e => e.Name);
            if(отделы.Length != 0) ComboOtdel.SelectedItem = отделы[0].Name;
            
            var должности =
                (Должности[])new Должности().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Должности"));

            ComboDol.ItemsSource = должности.Select(e => e.Наименование);
            if(должности.Length != 0) ComboDol.SelectedItem = должности[0].Наименование;

            _employee = сотрудники;
            bindcombo();
            if (сотрудники != null)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            LastName.Text = _employee.Фамилия;
            Surname.Text = _employee.Имя;
            Otchestvo.Text = _employee.Отчество;
            phone.Text = Convert.ToString(_employee.Телефон);
            ComboOtdel.Text = _employee.Отдел == null ? "" : _employee.Отдел.Name;
            ComboDol.Text = _employee.Должность == null ? "" : _employee.Должность.Наименование;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Сотрудники c = new Сотрудники
            {
                Фамилия = LastName.Text,
                Имя = Surname.Text,
                Отчество = Otchestvo.Text,
                Телефон = phone.Text,
                Дата_начала_работы = DateStart.SelectedDate.ToString(),
            };
            var отдел = DB.Database.ExecuteQuery($"SELECT * FROM Отделы WHERE Name = '{ComboOtdel.SelectedItem}'")
                .FirstOrDefault();
            if (отдел != null && отдел.TryGetValue("ID_Отдела", out object отделObject))
            {
                c.ID_Отдел = int.Parse(отделObject.ToString());
                Console.WriteLine($"номер отдела: {отделObject}");
            }
            
            var должность = DB.Database.ExecuteQuery($"SELECT * FROM Должности WHERE Наименованиеи = '{ComboDol.SelectedItem}'").FirstOrDefault();
            if (должность != null && должность.TryGetValue("ID_Должности", out object должностьObject))
            {
                c.ID_Должность = int.Parse(должностьObject.ToString());
                Console.WriteLine($"номер должность: {должностьObject}");
            }

            DB.Database.Query(
                $"INSERT INTO Сотрудники (Фамилия, Имя, Отчество, Телефон, Отдел, Должность, Дата_начала_работы) " +
                $"VALUES ('{c.Фамилия}', '{c.Имя}', '{c.Отчество}', '{c.Телефон}', {c.ID_Отдел}, {c.ID_Должность}, '{c.Дата_начала_работы}')");

            MessageBox.Show("Сотрудник был добавлен в базу");
            Employee em = new Employee();
            em.Show();
            Close();
        }

        public Должности[] Dolsh { get; set; }

        private void bindcombo()
        {
            var items = (Должности[])new Должности().ConvertToTables(
                DB.Database.ExecuteQuery($"SELECT * FROM Должности;"));
            Dolsh = items;
            DataContext = Dolsh;
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (LastName.Text == "Введите фамилию")
            {
                LastName.Text = "";
                LastName.Foreground = Brushes.White;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LastName.Text))
            {
                LastName.Text = "Введите фамилию";
                LastName.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText1(object sender, EventArgs e)
        {
            if (Surname.Text == "Введите имя")
            {
                Surname.Text = "";
                Surname.Foreground = Brushes.White;
            }
        }

        private void AddText1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Surname.Text))
            {
                Surname.Text = "Введите имя";
                Surname.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText2(object sender, EventArgs e)
        {
            if (Otchestvo.Text == "Введите отчество")
            {
                Otchestvo.Text = "";
                Otchestvo.Foreground = Brushes.White;
            }
        }

        private void AddText2(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Otchestvo.Text))
            {
                Otchestvo.Text = "Введите отчество";
                Otchestvo.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText3(object sender, EventArgs e)
        {
            if (phone.Text == "Введите номер телефона")
            {
                phone.Text = "";
                phone.Foreground = Brushes.White;
            }
        }

        private void AddText3(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(phone.Text))
            {
                phone.Text = "Введите номер телефона";
                phone.Foreground = Brushes.Gray;
            }
        }
    }
}