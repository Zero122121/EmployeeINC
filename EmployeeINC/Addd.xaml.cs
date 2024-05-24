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
    /// Логика взаимодействия для Addd.xaml
    /// </summary>
    /// 
    
    public partial class Addd : Window
    {
        private Сотрудники[] _сотрудники = {};
        
        private Больничные _employee;
        private bool _isEdit;
        
        public Addd(Больничные больничные = null)
        {
            InitializeComponent();
            bindcombo();
            _сотрудники = (Сотрудники[])new Сотрудники().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Сотрудники;"));
            ComboDol.ItemsSource = _сотрудники.Select(e=> $"{e.Фамилия} {e.Имя[0]}. {e.Отчество[0]}.");
            _employee = больничные;
            if (больничные != null)
            {
                _isEdit = true;
                LoadData();
            }
        }

        private void LoadData()
        {
            ComboDol.SelectedItem = $"{_employee.Сотрудник.Фамилия} {_employee.Сотрудник.Имя[0]}. {_employee.Сотрудник.Отчество[0]}.";
            DateNach.SelectedDate = DateTime.Parse(_employee.Дата_начала);
            DateCon.SelectedDate = DateTime.Parse(_employee.Дата_завершения);
            phone.Text = Convert.ToString( _employee.Номер_больничного);
            di.Text = _employee.Диагноз;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Больничные c = new Больничные
            {
                ID_Сотрудника = _сотрудники.FirstOrDefault(сотрудник=> $"{сотрудник.Фамилия} {сотрудник.Имя[0]}. {сотрудник.Отчество[0]}." == ComboDol.SelectedItem.ToString()).ID_Сотрудника,
                Дата_начала = DateNach.SelectedDate.ToString(),
                Дата_завершения = DateCon.SelectedDate.ToString(),
                Номер_больничного = Convert.ToInt32(phone.Text),
                Диагноз = di.Text
            };
            if (_isEdit)
            {
                DB.Database.Query($"UPDATE Больничные SET ID_Сотрудника = {c.ID_Сотрудника}, Дата_завершения = '{c.Дата_завершения}', " +
                                  $"Номер_больничного = {c.Номер_больничного}, Диагноз = '{c.Диагноз}', Дата_начала = '{c.Дата_начала}' " +
                                  $"WHERE ID_Больничные = {_employee.ID_Больничные}");
            }
            else
            {
                DB.Database.Query(
                    $"INSERT INTO Больничные (ID_Сотрудника, Дата_завершения, Номер_больничного, Диагноз, Дата_начала)" +
                    $"VALUES ({c.ID_Сотрудника}, '{c.Дата_завершения}', {c.Номер_больничного}, '{c.Диагноз}', '{c.Дата_начала}')");
            }

            MessageBox.Show("Данные были сохранены в базу");
            hospitals em = new hospitals();
            em.Show();
            Close();
        }

        public Сотрудники[] Dolsh { get; set; }
        private void bindcombo()
        {
            var items = (Сотрудники[])new Сотрудники().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Сотрудники;"));
            Dolsh = items;
            DataContext = Dolsh;
        }
        private void RemoveText(object sender, EventArgs e)
        {
            if (phone.Text == "Введите номер больничного")
            {
                phone.Text = "";
                phone.Foreground = Brushes.White;
            }
        }
        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(phone.Text))
            {
                phone.Text = "Введите номер больничного";
                phone.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText2(object sender, EventArgs e)
        {
            if (di.Text == "Введите диагноз")
            {
                di.Text = "";
                di.Foreground = Brushes.White;
            }
        }
        private void AddText2(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(di.Text))
            {
                di.Text = "Введите диагноз";
                di.Foreground = Brushes.Gray;
            }
        }
    }
}
