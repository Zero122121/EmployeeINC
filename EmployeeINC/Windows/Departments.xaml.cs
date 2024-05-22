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
    public partial class Departments : Window
    {
        public readonly List<(UIElement, UIElement, Отделы)> Отделы =
            new List<(UIElement, UIElement, Отделы)>();

        public Departments()
        {
            InitializeComponent();
            ShowTable();
        }

        private void ShowTable(string searchText = "")
        {
            for (int i = 1; i < Отделы.Count; i++)
            {
                (UIElement, UIElement, Отделы) tuple = Отделы[i];
                tuple.Item1 = null;
                tuple.Item2 = null;
                Отделы[i] = tuple;
            }

            var array = (Отделы[])new Отделы().ConvertToTables(DB.Database.ExecuteQuery($"SELECT * FROM Отделы"));

            array = array.Where(e => e.Name.Contains(searchText)).ToArray();

            foreach (Отделы отделы in array)
            {
                Border border = new Border();
                Grid grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                TextBlock textBlock1 = new TextBlock
                {
                    Text = отделы.Name,
                    FontSize = 18,
                    FontFamily = new FontFamily("Bahnschrift")
                };
                Grid.SetColumn(textBlock1, 0);
                grid.Children.Add(textBlock1);

                border.Name = "id_" + отделы.ID_Отдела;
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

                Отделы.Add((border, separator, отделы));

                menuItem.Click += (sender, e) =>
                {
                    DB.Database.Query($"DELETE FROM Отделы WHERE ID_Отдела = {отделы.ID_Отдела}");
                    Content.Children.Remove(border);
                    Content.Children.Remove(separator);
                };
                menuItemEdit.Click += (sender, e) =>
                {
                    var editEmployee = new AddDepartment(отделы);
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
            List<string> headers = new List<string>() { "Наименование" };
            for (int j = 0; j < 6; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = headers[j];
            }


            for (int j = 0; j < Отделы.Count; j++)
            {
                Excel.Range myRange = (Excel.Range)sheet1.Cells[j + 2, 1];
                myRange.Value2 = "Наименование";
            }
        }

        private void AddDepartment(object sender, RoutedEventArgs e)
        {
            AddDepartment window = new AddDepartment();
            window.Show();
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