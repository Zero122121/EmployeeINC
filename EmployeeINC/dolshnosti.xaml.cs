using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Логика взаимодействия для dolshnosti.xaml
    /// </summary>
    public partial class dolshnosti : Window
    {
        public ICommand SortCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly List<(UIElement, UIElement, Должности)> _должности =
            new List<(UIElement, UIElement, Должности)>();

        public dolshnosti()
        {
            InitializeComponent();
            ShowTable();
        }

        private void ShowTable(string searchText = "")
        {
            for (int i = 1; i < _должности.Count; i++)
            {
                (UIElement, UIElement, Должности) tuple = _должности[i];
                tuple.Item1 = null;
                tuple.Item2 = null;
                _должности[i] = tuple;
            }

            var array = (Должности[])new Должности().ConvertToTables(
                DB.Database.ExecuteQuery($"SELECT DISTINCT * FROM Должности"));

            array = array.Where(e => e.Наименование.Contains(searchText)).ToArray();

            foreach (Должности должности in array)
            {
                Border border = new Border();
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                TextBlock textBlock1 = new TextBlock
                {
                    Text = должности.Наименование,
                    FontSize = 18,
                    FontFamily = new FontFamily("Bahnschrift")
                };
                TextBlock textBlock2 = new TextBlock
                {
                    Text = должности.Ставка.ToString(),
                    FontSize = 18,
                    FontFamily = new FontFamily("Bahnschrift")
                };
                var сотрудниковВДолжности = new Сотрудники().ConvertToTables(DB.Database.ExecuteQuery(
                    $"SELECT DISTINCT * FROM Сотрудники WHERE Должность = {должности.ID_Должности}")).Length;
                TextBlock textBlock3 = new TextBlock
                {
                    Text = сотрудниковВДолжности.ToString(),
                    FontSize = 18,
                    FontFamily = new FontFamily("Bahnschrift")
                };

                Grid.SetColumn(textBlock1, 0);
                Grid.SetColumn(textBlock2, 1);
                Grid.SetColumn(textBlock3, 2);

                grid.Children.Add(textBlock1);
                grid.Children.Add(textBlock2);
                grid.Children.Add(textBlock3);

                border.Name = "id_" + должности.ID_Должности;
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

                _должности.Add((border, separator, должности));
                
                menuItem.Click += (sender, e) =>
                {
                    var сотрудниковВДолжности = new Сотрудники().ConvertToTables(DB.Database.ExecuteQuery(
                        $"SELECT DISTINCT * FROM Сотрудники WHERE Должность = {должности.ID_Должности}")).Length;
                    if (сотрудниковВДолжности > 0)
                    {
                        MessageBox.Show($"Количество сотрудников с этой должностью {сотрудниковВДолжности}. " +
                                        $"Измените их должность и попробуйте снова!");
                        return;
                    }
                    DB.Database.Query($"DELETE FROM Должности WHERE ID_Должности = {должности.ID_Должности}");
                    Content.Children.Remove(border);
                    Content.Children.Remove(separator);
                };
                menuItemEdit.Click += (sender, e) =>
                {
                    var редДолжности = new AddWindoww(должности);
                    редДолжности.Show();
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
            List<string> headers = new List<string>() { "Наименование", "Ставка" };
            for (int j = 0; j < 2; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = headers[j];
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < _должности.Count; j++)
                {
                    Excel.Range myRange = (Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = i == 0 ? _должности[j].Item3.Наименование : _должности[j].Item3.Ставка.ToString();
                }
            }
        }

        private void dobav_click(object sender, RoutedEventArgs e)
        {
            AddWindoww m = new AddWindoww();
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