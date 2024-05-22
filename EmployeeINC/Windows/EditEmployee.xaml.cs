using System.Linq;
using System.Windows;
using EmployeeINC.Database.Tables;

namespace EmployeeINC.Windows
{
    public partial class EditEmployee : Window
    {
        private Сотрудники _сотрудник;

        public EditEmployee(Сотрудники сотрудник)
        {
            InitializeComponent();

            _сотрудник = сотрудник;
            Фамилия.Text = сотрудник.Фамилия;
            Имя.Text = сотрудник.Имя;
            Отчество.Text = сотрудник.Отчество;
            Телефон.Text = сотрудник.Телефон;

            var отделы = (Отделы[])new Отделы().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Отделы;"));
            Отдел.ItemsSource = отделы.Select(d => d.Name);
            Отдел.SelectedItem = сотрудник.Должность.Наименование;

            var должности =
                (Должности[])new Должности().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Должности;"));
            Должность.ItemsSource = должности.Select(d => d.Наименование);
            Должность.SelectedItem = сотрудник.Должность.Наименование;

            ДатаНачалаРаботы.Text = сотрудник.Дата_начала_работы;
        }

        private void SaveEdited(object sender, RoutedEventArgs e)
        {
            var отдел = (Отделы)new Отделы().ConvertToTables(
                    DB.Database.ExecuteQuery($"SELECT * FROM Отделы WHERE Name = '{Отдел.SelectedItem}'"))
                .FirstOrDefault();
            var должность = (Должности)new Должности().ConvertToTables(
                    DB.Database.ExecuteQuery($"SELECT * FROM Должности WHERE Name = '{Должность.SelectedItem}'"))
                .FirstOrDefault();

            DB.Database.Query($"UPDATE Сотрудники " +
                              $"SET Фамилия = '{Фамилия.Text}', " +
                              $"Имя = '{Имя.Text}', " +
                              $"Отчество = '{Отчество.Text}', " +
                              $"Телефон = '{Телефон.Text}', " +
                              $"Отдел = '{отдел.ID_Отдела}', " +
                              $"Должность = '{должность.ID_Должности}', " +
                              $"Дата_начала_работы = '{ДатаНачалаРаботы.Text}' " +
                              $"WHERE ID_Сотрудника = {_сотрудник.ID_Сотрудника}");
            
            var employee = new Employee();
            employee.Show();
            Close();
        }
    }
}