using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EmployeeINC.Database.Tables;
using Excel = Microsoft.Office.Interop.Excel;

namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для Vacations.xaml
    /// </summary>
    public partial class Vacations : Window
    {
        public readonly List<(UIElement, UIElement, Отпуски)> Отпуски =
            new List<(UIElement, UIElement, Отпуски)>();

        public Vacations()
        {
            InitializeComponent();
            ShowTable();
        }

        private void ShowTable(string searchText = "")
        {
            for (int i = 1; i < Отпуски.Count; i++)
            {
                (UIElement, UIElement, Отпуски) tuple = Отпуски[i];
                tuple.Item1 = null;
                tuple.Item2 = null;
                Отпуски[i] = tuple;
            }

            var array = (Отпуски[])new Отпуски().ConvertToTables(
                DB.Database.ExecuteQuery($"SELECT DISTINCT * FROM Отпуски"));

            array = array.Where(e => e.Дата_начала.Contains(searchText) || e.Дата_завершения.Contains(searchText) ||
                                     (e.Сотрудник.Фамилия + e.Сотрудник.Имя + e.Сотрудник.Отчество)
                                     .Contains(searchText)).ToArray();

            foreach (Отпуски отпуски in array)
            {
                Border border = new Border();
                Grid grid = new Grid();
                var list = new List<string>()
                {
                    $"{отпуски.Дата_начала}",
                    $"{отпуски.Дата_завершения}",
                    $"{отпуски.Сотрудник.Имя} {отпуски.Сотрудник.Фамилия[0]}. {отпуски.Сотрудник.Отчество[0]}."
                };
                for (int i = 0; i < 3; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    TextBlock textBlock1 = new TextBlock
                    {
                        Text = list[i],
                        FontSize = 18,
                        FontFamily = new FontFamily("Bahnschrift")
                    };
                    Grid.SetColumn(textBlock1, i);
                    grid.Children.Add(textBlock1);
                }

                border.Name = "id_" + отпуски.ID_Сотрудника;
                border.Child = grid;

                ContextMenu contextMenu = new ContextMenu();
                MenuItem menuItem = new MenuItem() { Header = "Удалить" };
                contextMenu.Items.Add(menuItem);
                MenuItem menuItemEdit = new MenuItem() { Header = "Редактировать" };
                contextMenu.Items.Add(menuItemEdit);
                border.ContextMenu = contextMenu;

                Content.Children.Add(border);

                Separator separator = new Separator();
                Content.Children.Add(separator);

                Отпуски.Add((border, separator, отпуски));

                menuItem.Click += (sender, e) =>
                {
                    DB.Database.Query($"DELETE FROM Отпуски WHERE ID_Сотрудника = {отпуски.ID_Сотрудника}");
                    Content.Children.Remove(border);
                    Content.Children.Remove(separator);
                };
                menuItemEdit.Click += (sender, e) =>
                {
                    Add ma = new Add(отпуски);
                    ma.Show();
                    Close();
                };
            }
        }

        private void export_click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add(Missing.Value);
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            List<string> headers = new List<string>() { "ФИО", "Начало", "Конец" };

            for (int j = 0; j < 3; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = headers[j];
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Отпуски.Count; j++)
                {
                    Excel.Range myRange = (Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = i switch
                    {
                        0 => $"{Отпуски[j].Item3.Сотрудник.Фамилия} {Отпуски[j].Item3.Сотрудник.Имя[0]}. {Отпуски[j].Item3.Сотрудник.Отчество[0]}.",
                        1 => Отпуски[j].Item3.Дата_начала,
                        2 => Отпуски[j].Item3.Дата_завершения,
                        _ => myRange.Value2
                    };
                }
            }
        }

        private void Search_Tbox_TextChanged(object sender, TextChangedEventArgs e) => ShowTable(Search.Text);


        private void dobav_click(object sender, RoutedEventArgs e)
        {
            Add ma = new Add();
            ma.Show();
            Close();
        }

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

        private void back_click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Close();
        }
    }
}