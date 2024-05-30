using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DB.Utils;
using EmployeeINC.Database.Tables;
using Microsoft.Win32;

namespace EmployeeINC.Windows
{
    public partial class EmployeeProfile : Window
    {
        private readonly Сотрудники _сотрудник;
        private Document[] _document;

        private bool _selectedPicture = false;

        public EmployeeProfile(Сотрудники сотрудник, Document[] document)
        {
            _сотрудник = сотрудник;
            _document = document;
            InitializeComponent();
            UpdateData();
            UpdateDocsList();
        }

        private void SaveProfile(object sender, RoutedEventArgs e)
        {
            if (_selectedPicture)
            {
                var photoHex = BitConverter.ToString(_сотрудник.picture).Replace("-", "");
                DB.Database.Query(
                    $"UPDATE Сотрудники SET picture = x'{photoHex}' WHERE ID_Сотрудника = {_сотрудник.ID_Сотрудника}");
            }

            Employee employee = new Employee();
            employee.Show();
            Close();
        }

        private void UpdateData()
        {
            LastName.Text = "Фамилия: " + _сотрудник.Фамилия;
            Surname.Text = "Имя: " +  _сотрудник.Имя;
            Otchestvo.Text = "Отчество: " +  _сотрудник.Отчество;
            phone.Text = "Телефон: " +  _сотрудник.Телефон;
            DateStart.Text = "Дата начала: " +  _сотрудник.Дата_начала_работы;
            telegram.Text = "Telegram: " +  _сотрудник.tg_username;
            ComboOtdel.Text = "Отдел: " + (_сотрудник.Отдел == null ? "" : _сотрудник.Отдел.Name);
            ComboDol.Text = "Должность: " + (_сотрудник.Должность == null ? "" : _сотрудник.Должность.Наименование);
            ProfileIcon.Source = _сотрудник.pictureBitmap;
            Живет.Text = "Место жительства: " + _сотрудник.место_рождения;
            ДатаРождение.Text = "Дата рождение: " + _сотрудник.дата_рождения;
            Гражданство.Text = "Гражданство: " + _сотрудник.гражданство;
            Образование.Text = "Образование: " + _сотрудник.образование;
            Серия.Text = "Серия: " + _сотрудник.серия;
            Номер.Text = "Номер: " + _сотрудник.номер;
            Адрес.Text = "Адрес: " + _сотрудник.адрес;
            СемейноеПоложение.Text = "Семейное положение: " + _сотрудник.семейное_положение;
        }

        private void UpdateDocsList()
        {
            DocsContent.Children.Clear();

            var docs = (Document[])new Document().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM document WHERE employee_id = {_сотрудник.ID_Сотрудника};"));
            foreach (Document document in docs)
            {
                Button button = new()
                {
                    Style = (Style)Application.Current.Resources["RoundButton"],
                    Margin = new Thickness(20, 5, 0, 0),
                    Name = $"id_{document.id_document}"
                };
                
                button.Click += DownloadDocument;
                TextBlock textBlock = new()
                {
                    Text = document.name,
                    FontSize = 18
                };
                
                ContextMenu contextMenu = new ContextMenu();
                MenuItem removeDoc = new MenuItem() { Header = "Удалить документ" };
                contextMenu.Items.Add(removeDoc);
                button.ContextMenu = contextMenu;
                removeDoc.Click += (sender, args) =>
                {
                    DB.Database.Query($"DELETE FROM document WHERE id_document = {document.id_document};");
                    UpdateDocsList();
                };

                button.Content = textBlock;
                DocsContent.Children.Add(button);
            }
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
            _selectedPicture = true;
        }

        private void DownloadDocument(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).Name.Replace("id_", ""));
            byte[] fileContent = ((Document)new Document().ConvertToTable(DB.Database.ExecuteQuery(
                $"SELECT * FROM document WHERE id_document = {id}").FirstOrDefault())).document;

            SaveFileDialog saveFileDialog = new() { Filter = "Word Documents (*.docx)|*.docx" };

            if (saveFileDialog.ShowDialog() != true) return;
            string saveFilePath = saveFileDialog.FileName;
            File.WriteAllBytes(saveFilePath, fileContent);
        }
        
        private void AddDocument(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new() { Filter = "Word Documents (*.docx)|*.docx" };

            if (openFileDialog.ShowDialog() != true) return;
            string filePath = openFileDialog.FileName;
            string name = Path.GetFileName(filePath);

            // Чтение содержимого файла
            byte[] fileContent = File.ReadAllBytes(filePath);

            // Сохранение файла в базе данных
            var fileContentHex = BitConverter.ToString(fileContent).Replace("-", "");
            DB.Database.ExecuteQuery($"INSERT INTO document (name, document, employee_id) VALUES ('{name}', x'{fileContentHex}', {_сотрудник.ID_Сотрудника})");
            
            UpdateDocsList();
        }
    }
}