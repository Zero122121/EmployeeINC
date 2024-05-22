using System;
using System.Windows;
using System.Windows.Media;
using EmployeeINC.Database.Tables;

namespace EmployeeINC.Windows
{
    public partial class AddDepartment : Window
    {
        private Отделы _employee = null;
        private bool _isEdit;
        
        public AddDepartment(Отделы отделы = null)
        {
            InitializeComponent();
            if (отделы != null)
            {
                _isEdit = true;
                _employee = отделы;
                LoadData();
            }
        }
        
        private void LoadData()
        {
            Наименование.Text = _employee.Name;
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (Наименование.Text == "Введите название")
            {
                Наименование.Text = "";
                Наименование.Foreground = Brushes.White;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Наименование.Text))
            {
                Наименование.Text = "Введите название";
                Наименование.Foreground = Brushes.Gray;
            }
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            if(!_isEdit) _employee = new Отделы();
            _employee.Name = Наименование.Text;

            DB.Database.Query(_isEdit
                ? $"UPDATE Отделы SET Name = '{_employee.Name}' WHERE ID_Отдела = {_employee.ID_Отдела}"
                : $"INSERT INTO Отделы (Name) VALUES ('{_employee.Name}')");

            MessageBox.Show("Должность была добавлен в базу");
            Departments em = new Departments();
            em.Show();
            Close();
        }
    }
}