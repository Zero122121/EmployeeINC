using System;
using System.Windows;
using System.Windows.Media.Imaging;
using DB.Utils;
using EmployeeINC.Database.Tables;
using Microsoft.Win32;

namespace EmployeeINC.Windows
{
    public partial class EmployeeProfile : Window
    {
        private readonly Сотрудники _сотрудник;

        public EmployeeProfile(Сотрудники сотрудник)
        {
            _сотрудник = сотрудник;
            InitializeComponent();
            UpdateData();
        }

        private void SaveProfile(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(_сотрудник.picture);
            var photoHex = BitConverter.ToString(_сотрудник.picture).Replace("-", "");
            DB.Database.Query($"UPDATE Сотрудники SET picture = x'{photoHex}' WHERE ID_Сотрудника = {_сотрудник.ID_Сотрудника}");
            Employee employee = new Employee();
            employee.Show();
            Close();
        }

        private void UpdateData()
        {
            LastName.Text = _сотрудник.Фамилия;
            Surname.Text = _сотрудник.Имя;
            Otchestvo.Text = _сотрудник.Отчество;
            phone.Text = _сотрудник.Телефон;
            DateStart.Text = _сотрудник.Дата_начала_работы;
            ComboOtdel.Text = _сотрудник.Отдел == null ? "" : _сотрудник.Отдел.Name;
            ComboDol.Text = _сотрудник.Должность == null ? "" : _сотрудник.Должность.Наименование;
            ProfileIcon.Source = _сотрудник.pictureBitmap;
        }

        private void SelectPicture(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() != true) return;
            string filePath = openFileDialog.FileName;

            BitmapImage bitmap = new(new Uri(filePath));
            ProfileIcon.Source = bitmap;

            byte[] imageBytes = bitmap.ConvertBitmapImageToByteArray();
            _сотрудник.picture = imageBytes;
            _сотрудник.pictureBitmap = bitmap;
        }
    }
}