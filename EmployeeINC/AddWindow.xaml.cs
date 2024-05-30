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

        private bool _isEdit = false;

        public AddWindow(Сотрудники сотрудники = null)
        {
            InitializeComponent();
            _isEdit = сотрудники != null;
            var отделы = (Отделы[])new Отделы().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Отделы"));
            ComboOtdel.ItemsSource = отделы.Select(e => e.Name);
            if (отделы.Length != 0) ComboOtdel.SelectedItem = отделы[0].Name;

            var должности =
                (Должности[])new Должности().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Должности"));

            ComboDol.ItemsSource = должности.Select(e => e.Наименование);
            if (должности.Length != 0) ComboDol.SelectedItem = должности[0].Наименование;

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
            phone.Text = _employee.Телефон;
            DateStart.Text = _employee.Дата_начала_работы;
            telegram.Text = _employee.tg_username;
            ComboOtdel.Text = _employee.Отдел == null ? "" : _employee.Отдел.Name;
            ComboDol.Text = _employee.Должность == null ? "" : _employee.Должность.Наименование;

            Живет.Text = _employee.место_рождения;
            ДатаРождение.Text = _employee.дата_рождения;
            Гражданство.Text = _employee.гражданство;
            Образование.Text = _employee.образование;
            Серия.Text = _employee.серия.ToString();
            Номер.Text = _employee.номер.ToString();
            Адрес.Text = _employee.адрес;
            СемейноеПоложение.Text = _employee.семейное_положение;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Сотрудники c = new Сотрудники
            {
                ID_Сотрудника = _employee?.ID_Сотрудника ?? -1,
                Фамилия = LastName.Text,
                Имя = Surname.Text,
                Отчество = Otchestvo.Text,
                Телефон = phone.Text,
                Дата_начала_работы = DateStart.SelectedDate.ToString(),
                tg_username = telegram.Text,
                место_рождения = Живет.Text,
                дата_рождения = ДатаРождение.Text,
                гражданство = Гражданство.Text,
                серия = int.Parse(Серия.Text),
                номер = int.Parse(Номер.Text),
                адрес = Адрес.Text,
                образование = Образование.Text,
                семейное_положение = СемейноеПоложение.Text,
            };
            var отдел = DB.Database.ExecuteQuery($"SELECT * FROM Отделы WHERE Name = '{ComboOtdel.SelectedItem}'")
                .FirstOrDefault();
            if (отдел != null && отдел.TryGetValue("ID_Отдела", out object отделObject))
            {
                c.ID_Отдел = int.Parse(отделObject.ToString());
            }

            var должность = DB.Database
                .ExecuteQuery($"SELECT * FROM Должности WHERE Наименованиеи = '{ComboDol.SelectedItem}'")
                .FirstOrDefault();
            if (должность != null && должность.TryGetValue("ID_Должности", out object должностьObject))
            {
                c.ID_Должность = int.Parse(должностьObject.ToString());
            }

            if (_isEdit)
            {
                DB.Database.Query(
                    $"UPDATE Сотрудники SET Фамилия = '{c.Фамилия}', Имя = '{c.Имя}', Отчество = '{c.Отчество}', " +
                    $"Телефон = '{c.Телефон}', Отдел = {c.ID_Отдел}, Должность =  {c.ID_Должность}, " +
                    $"Дата_начала_работы = '{c.Дата_начала_работы}', tg_username = '{c.tg_username}', " +
                    $"место_рождения = '{c.место_рождения}', " +
                    $"гражданство = '{c.гражданство}', " +
                    $"серия = '{c.серия}', " +
                    $"номер = '{c.номер}', " +
                    $"адрес = '{c.адрес}', " +
                    $"образование = '{c.образование}', " +
                    $"семейное_положение = '{c.семейное_положение}' " +
                    $"WHERE ID_Сотрудника = {c.ID_Сотрудника}");
            }
            else
            {
                DB.Database.Query(
                    $"INSERT INTO Сотрудники (Фамилия, Имя, Отчество, Телефон, Отдел, Должность, Дата_начала_работы, tg_username, " +
                    $"  место_рождения, гражданство, серия, номер, адрес, образование, семейное_положение) " +
                    $"VALUES ('{c.Фамилия}', '{c.Имя}', '{c.Отчество}', '{c.Телефон}', {c.ID_Отдел}, {c.ID_Должность}, " +
                    $"'{c.Дата_начала_работы}', '{c.tg_username}', '{c.место_рождения}','{c.гражданство}', '{c.серия}', '{c.номер}', " +
                    $"'{c.адрес}', '{c.образование}', '{c.семейное_положение}')");
            }

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

        private void RemoveText4(object sender, RoutedEventArgs e)
        {
            if (telegram.Text == "Введите telegram ник")
            {
                telegram.Text = "";
                telegram.Foreground = Brushes.White;
            }
        }

        private void AddText4(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(telegram.Text))
            {
                telegram.Text = "Введите telegram ник";
                telegram.Foreground = Brushes.Gray;
            }
        }


        private void RemoveText5(object sender, RoutedEventArgs e)
        {
            string text = ((TextBox)sender).Name switch
            {
                "Живет" => "Введите живет",
                "ДатаРождение" => "Введите дата рождение",
                "Гражданство" => "Введите гражданство",
                "Образование" => "Введите образование",
                "Серия" => "Введите серия",
                "Номер" => "Введите номер",
                "Адрес" => "Введите адрес",
                "СемейноеПоложение" => "Введите семейное положение",
            };
            if (((TextBox)sender).Text == text)
            {
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Foreground = Brushes.White;
            }
        }

        private void AddText5(object sender, RoutedEventArgs e)
        {
            string text = ((TextBox)sender).Name switch
            {
                "Живет" => "Введите живет",
                "ДатаРождение" => "Введите дата рождение",
                "Гражданство" => "Введите гражданство",
                "Образование" => "Введите образование",
                "Серия" => "Введите серия",
                "Номер" => "Введите номер",
                "Адрес" => "Введите адрес",
                "СемейноеПоложение" => "Введите семейное положение",
            };
            if (string.IsNullOrWhiteSpace(((TextBox)sender).Text))
            {
                ((TextBox)sender).Text = text;
                ((TextBox)sender).Foreground = Brushes.Gray;
            }
        }
    }
}