using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EmployeeINC.Database.Tables;
using EmployeeINC.Windows;
using Excel = Microsoft.Office.Interop.Excel;


namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : Window
    {
        public readonly List<(UIElement, UIElement, Сотрудники)> Сотрудники =
            new List<(UIElement, UIElement, Сотрудники)>();

        public Employee()
        {
            InitializeComponent();
            ShowTable();
        }

        private void ShowTable(string searchText = "")
        {
            for (int i = 1; i < Сотрудники.Count; i++)
            {
                (UIElement, UIElement, Сотрудники) tuple = Сотрудники[i];
                tuple.Item1 = null;
                tuple.Item2 = null;
                Сотрудники[i] = tuple;
            }

            var array = (Сотрудники[])new Сотрудники().ConvertToTables(
                DB.Database.ExecuteQuery($"SELECT DISTINCT * FROM Сотрудники"));

            array = array.Where(e => e.Имя.Contains(searchText) || e.Фамилия.Contains(searchText) ||
                                     e.Отчество.Contains(searchText) || e.Дата_начала_работы.Contains(searchText) ||
                                     e.Телефон.Contains(searchText) || e.Дата_начала_работы.Contains(searchText) ||
                                     e.tg_username.Contains(searchText)).ToArray();

            foreach (Сотрудники сотрудник in array)
            {
                Border border = new Border();
                Grid grid = new Grid();

                var list = new List<string>()
                {
                    $"{сотрудник.Фамилия}",
                    $"{сотрудник.Имя}",
                    $"{сотрудник.Отчество}",
                    $"{сотрудник.Должность.Наименование}",
                    $"{сотрудник.Телефон}",
                    $"{сотрудник.Отдел.Name}",
                    $"{сотрудник.tg_username}"
                };
                for (int i = 0; i < 7; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    TextBlock textBlock1 = new TextBlock
                    {
                        Text = list[i],
                        FontSize = 18,
                        FontFamily = new FontFamily("Bahnschrift"),
                        TextTrimming = TextTrimming.CharacterEllipsis
                    };
                    Grid.SetColumn(textBlock1, i);
                    grid.Children.Add(textBlock1);
                }

                border.Name = "id_" + сотрудник.ID_Сотрудника;
                border.Child = grid;

                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItemProfile = new MenuItem() { Header = "Профиль" };
                contextMenu.Items.Add(menuItemProfile);
                MenuItem menuItemEdit = new MenuItem() { Header = "Редактировать" };
                contextMenu.Items.Add(menuItemEdit);
                MenuItem menuItem = new MenuItem() { Header = "Удалить" };
                contextMenu.Items.Add(menuItem);
                border.ContextMenu = contextMenu;

                Content.Children.Add(border);

                Separator separator = new Separator();
                Content.Children.Add(separator);

                Сотрудники.Add((border, separator, сотрудник));

                menuItem.Click += (sender, e) =>
                {
                    DB.Database.Query($"DELETE FROM document WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}");
                    DB.Database.Query($"DELETE FROM Отпуски WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}");
                    DB.Database.Query($"DELETE FROM Больничные WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}");
                    
                    DB.Database.Query($"DELETE FROM Сотрудники WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}");
                    Content.Children.Remove(border);
                    Content.Children.Remove(separator);
                };
                menuItemEdit.Click += (_, _) =>
                {
                    var editEmployee = new AddWindow(сотрудник);
                    editEmployee.Show();
                    Close();
                };
                menuItemProfile.Click += (_, _) =>
                {
                    var docs = (Document[])new Document().ConvertToTables(
                        DB.Database.ExecuteQuery($"SELECT * FROM document WHERE employee_id = {сотрудник.ID_Сотрудника}")
                        );
                    var editEmployee = new EmployeeProfile(сотрудник, docs);
                    editEmployee.Show();
                    Close();
                };
            }
        }

        private void Search_Tbox_TextChanged(object sender, TextChangedEventArgs e) => ShowTable(Search.Text);

        private void RemoveText(object sender, EventArgs e)
        {
            if (Search.Text == "Search...")
            {
                Search.Text = "";
                Search.Foreground = Brushes.White;
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Search.Text))
            {
                Search.Text = "Search...";
                Search.Foreground = Brushes.Gray;
            }
        }

        private void export_click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add(Missing.Value);
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            List<string> headers = new List<string>()
                { "Фамилия", "Имя", "Отчество", "Должность", "Телефон", "Отдел", "telegram nick" };
            for (int j = 0; j < 7; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = headers[j];
            }

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < Сотрудники.Count; j++)
                {
                    Excel.Range myRange = (Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = i switch
                    {
                        0 => Сотрудники[j].Item3.Фамилия,
                        1 => Сотрудники[j].Item3.Имя,
                        2 => Сотрудники[j].Item3.Отчество,
                        3 => Сотрудники[j].Item3.Должность.Наименование,
                        4 => Сотрудники[j].Item3.Телефон,
                        5 => Сотрудники[j].Item3.Отдел.Name,
                        6 => Сотрудники[j].Item3.tg_username,
                        _ => myRange.Value2
                    };
                }
            }
        }

        private void dobav_click(object sender, RoutedEventArgs e)
        {
            AddWindow m = new AddWindow();
            m.Show();
            Close();
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Close();
        }
    }
}