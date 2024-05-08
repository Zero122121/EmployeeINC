using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для Vacations.xaml
    /// </summary>
    public partial class Vacations : System.Windows.Window
    {
        public Vacations()
        {
            InitializeComponent();
            ShowTable(DataAcEntities1.GetContext().Отпуски);
        }

        public void ShowTable<T>(DbSet<T> query) where T : class
        {
            VacationGrid.ItemsSource = query.ToList();
        }

        private void export_click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < VacationGrid.Columns.Count; j++)
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = VacationGrid.Columns[j].Header;
            }
            for (int i = 0; i < VacationGrid.Columns.Count; i++)
            {
                for (int j = 0; j < VacationGrid.Items.Count; j++)
                {
                    TextBlock b = VacationGrid.Columns[i].GetCellContent(VacationGrid.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }
        }

        private void Search_Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                VacationGrid.ItemsSource = DataAcEntities1.GetContext().Отпуски.Where(item => item.Сотрудники.Фамилия == Search.Text || 
                item.Сотрудники.Фамилия.Contains(Search.Text)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void red_click(object sender, RoutedEventArgs e)
        {
            object abonent = VacationGrid.SelectedItem;
            Отпуски отпуски = abonent as Отпуски;
            Add menu = new Add(отпуски);
            menu.Show();
            Close();
            var CurrentUser = VacationGrid.SelectedItem as Отпуски;
            DataAcEntities1.GetContext().Отпуски.Remove(CurrentUser);
            DataAcEntities1.GetContext().SaveChanges();

            VacationGrid.ItemsSource = DataAcEntities1.GetContext().Сотрудники.ToList();
        }

        private void dobav_click(object sender, RoutedEventArgs e)
        {
            Add ma = new Add();
            ma.Show();
            Close();
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить данные о отпуске", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var CurrentUser = VacationGrid.SelectedItem as Отпуски;
                DataAcEntities1.GetContext().Отпуски.Remove(CurrentUser);
                DataAcEntities1.GetContext().SaveChanges();

                VacationGrid.ItemsSource = DataAcEntities1.GetContext().Отпуски.ToList();
                MessageBox.Show("Данные о отпуске удален");
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

        private void back_click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Close();
        }
    }
}
