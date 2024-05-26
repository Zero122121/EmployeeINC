using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EmployeeINC.Database.Tables;
using Excel = Microsoft.Office.Interop.Excel;

namespace EmployeeINC.Windows
{
    public partial class EmployeeStats : Window
    {
        public readonly List<(UIElement, UIElement, Сотрудники сотрудник, Отпуски[] отпуски, Больничные[] больничные)> Сотрудники =
            new List<(UIElement, UIElement, Сотрудники, Отпуски[], Больничные[])>();

        public EmployeeStats()
        {
            InitializeComponent();
            ShowTable();
        }

        private void ShowTable(string searchText = "")
        {
            for (int i = 1; i < Сотрудники.Count; i++)
            {
                (UIElement, UIElement, Сотрудники, Отпуски[], Больничные[]) tuple = Сотрудники[i];
                tuple.Item1 = null;
                tuple.Item2 = null;
                Сотрудники[i] = tuple;
            }

            var array = (Сотрудники[])new Сотрудники().ConvertToTables(
                DB.Database.ExecuteQuery($"SELECT * FROM Сотрудники"));

            array = array.Where(e => e.Имя.Contains(searchText) || e.Фамилия.Contains(searchText) ||
                                     e.Отчество.Contains(searchText) || e.Дата_начала_работы.Contains(searchText) ||
                                     e.Телефон.Contains(searchText) || e.Дата_начала_работы.Contains(searchText) ||
                                     e.tg_username.Contains(searchText)).ToArray();

            foreach (Сотрудники сотрудник in array)
            {
                var отпуски = (Отпуски[])new Отпуски().ConvertToTables(DB.Database.ExecuteQuery(
                    $"SELECT * FROM Отпуски WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}"));
                
                var больничные = (Больничные[])new Больничные().ConvertToTables(DB.Database.ExecuteQuery(
                    $"SELECT * FROM Больничные WHERE ID_Сотрудника = {сотрудник.ID_Сотрудника}"));
                
                Border border = new Border();
                Grid grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60, GridUnitType.Star) });
                
                
                var list = new List<string>()
                {
                    $"{сотрудник.Отдел.Name}",
                    $"{сотрудник.Фамилия} {сотрудник.Имя[0]}. {сотрудник.Отчество[0]}.",
                    $"{сотрудник.Должность.Наименование}",
                    $"{отпуски.Length}",
                    $"{больничные.Length}",
                    $"{отпуски.Length + больничные.Length}"
                };
                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine(i);

                    Border borderBlock = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1, 0, 1, 2)
                    };
                    TextBlock textBlock1 = new TextBlock
                    {
                        Text = list[i],
                        FontSize = 18,
                        FontFamily = new FontFamily("Bahnschrift")
                    };
                    Grid.SetColumn(borderBlock, i);
                    grid.Children.Add(borderBlock);
                    borderBlock.Child = textBlock1;
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

                Сотрудники.Add((border, separator, сотрудник, отпуски, больничные));

                menuItem.Click += (sender, e) =>
                {
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
                    var editEmployee = new EmployeeProfile(сотрудник);
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
            Excel.Application excel = new() { Visible = true };
            Excel.Workbook workbook = excel.Workbooks.Add(Missing.Value);
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            var headers = new List<string> { "Отдел", "ФИО", "Должность", "В отпуске", "На больничном", "Всего" };
            for (int j = 0; j < 6; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = headers[j];
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < Сотрудники.Count; j++)
                {
                    Excel.Range myRange = (Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = i switch
                    {
                        0 => Сотрудники[j].Item3.Отдел.Name,
                        1 => $"{Сотрудники[j].Item3.Фамилия} {Сотрудники[j].Item3.Имя[0]}. {Сотрудники[j].Item3.Отчество[0]}.",
                        2 => Сотрудники[j].Item3.Должность.Наименование,
                        3 => Сотрудники[j].отпуски.Length,
                        4 => Сотрудники[j].больничные.Length,
                        5 => Сотрудники[j].больничные.Length + Сотрудники[j].отпуски.Length,
                        _ => myRange.Value2
                    };
                }
            }
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Close();
        }
    }
}