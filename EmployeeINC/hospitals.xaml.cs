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
    /// Логика взаимодействия для hospitals.xaml
    /// </summary>
    public partial class hospitals : Window
    {
        public readonly List<(UIElement, UIElement, Больничные)> Больничные =
            new List<(UIElement, UIElement, Больничные)>();
        public hospitals()
        {
            InitializeComponent();
            ShowTable();
        }

        private void ShowTable(string searchText = "")
        {
            for (int i = 1; i < Больничные.Count; i++)
            {
                (UIElement, UIElement, Больничные) tuple = Больничные[i];
                tuple.Item1 = null;
                tuple.Item2 = null;
                Больничные[i] = tuple;
            }

            var array = (Больничные[])new Больничные().ConvertToTables(
                DB.Database.ExecuteQuery($"SELECT DISTINCT * FROM Больничные"));

            array = array.Where(e => e.Сотрудник.Фамилия.Contains(searchText) ||e.Сотрудник.Фамилия.Contains(searchText) ||
                                     e.Сотрудник.Отчество.Contains(searchText) || e.Диагноз.Contains(searchText) ||
                                     e.Номер_больничного.ToString().Contains(searchText) || e.Дата_завершения.Contains(searchText))
                .ToArray();

            foreach (Больничные больничные in array)
            {
                Border border = new Border();
                Grid grid = new Grid();
                
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                
                var list = new List<string>()
                {
                    $"{больничные.Сотрудник.Фамилия} {больничные.Сотрудник.Имя[0]}.{больничные.Сотрудник.Отчество[0]}.",
                    $"{больничные.Дата_начала}",
                    $"{больничные.Дата_завершения}",
                    $"{больничные.Номер_больничного}",
                    $"{больничные.Диагноз}"
                };
                for (int i = 0; i < 5; i++)
                {

                    TextBlock textBlock1 = new TextBlock
                    {
                        Text = list[i],
                        FontSize = 18,
                        FontFamily = new FontFamily("Bahnschrift")
                    };
                    Grid.SetColumn(textBlock1, i);
                    grid.Children.Add(textBlock1);
                }

                border.Name = "id_" + больничные.ID_Больничные;
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

                Больничные.Add((border, separator, больничные));

                menuItem.Click += (sender, e) =>
                {
                    DB.Database.Query($"DELETE FROM Сотрудники WHERE ID_Сотрудника = {больничные.ID_Сотрудника}");
                    Content.Children.Remove(border);
                    Content.Children.Remove(separator);
                };
                menuItemEdit.Click += (sender, e) =>
                {
                    var window = new Addd(больничные);
                    window.Show();
                    Close();
                };
            }
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
        private void Search_Tbox_TextChanged(object sender, TextChangedEventArgs e) => ShowTable(Search.Text);

        private void export_click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add(Missing.Value);
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];   
            List<string> headers = new List<string>() { "ФИО", "Дата_начала", "Дата_завершения", "Номер_больничного", "Диагноз" };


            for (int j = 0; j < 5; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = headers[j];
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < Больничные.Count; j++)
                {
                    Excel.Range myRange = (Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = i switch
                    {
                        0 => ($"{Больничные[j].Item3.Сотрудник.Фамилия} {Больничные[j].Item3.Сотрудник.Имя} {Больничные[j].Item3.Сотрудник.Отчество}"),
                        1 => Больничные[j].Item3.Дата_начала,
                        2 => Больничные[j].Item3.Дата_завершения,
                        3 => Больничные[j].Item3.Номер_больничного,
                        4 => Больничные[j].Item3.Диагноз,
                        _ => myRange.Value2
                    };
                }
            }
        }

        private void dobav_click(object sender, RoutedEventArgs e)
        {
            Addd ma = new Addd();
            ma.Show();
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
