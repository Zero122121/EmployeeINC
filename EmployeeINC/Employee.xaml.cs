using Microsoft.Office.Interop.Excel;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;


namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для Employee.xaml
    /// </summary>
    public partial class Employee : System.Windows.Window
    {
        public Employee()
        {
            InitializeComponent();
            ShowTable(DataAcEntities1.GetContext().Сотрудники);
        }

        public void ShowTable<T>(DbSet<T> query) where T : class
        {
            EmployeeGrid.ItemsSource = query.ToList();
        }

        private void Search_Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                EmployeeGrid.ItemsSource = DataAcEntities1.GetContext().Сотрудники.Where(item => item.Фамилия == Search.Text || item.Фамилия.Contains(Search.Text) 
                || item.Отчество == Search.Text || item.Отчество.Contains(Search.Text) || item.Имя == Search.Text || item.Имя.Contains(Search.Text) 
                ||  item.Отдел == Search.Text || item.Отдел.Contains(Search.Text)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void export_click(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            for (int j = 0; j < EmployeeGrid.Columns.Count; j++)
            {
                Range myRange = (Range)sheet1.Cells[1, j + 1];
                sheet1.Cells[1, j + 1].Font.Bold = true;
                sheet1.Columns[j + 1].ColumnWidth = 20;
                myRange.Value2 = EmployeeGrid.Columns[j].Header;
            }
            for (int i = 0; i < EmployeeGrid.Columns.Count; i++)
            {
                for (int j = 0; j < EmployeeGrid.Items.Count; j++)
                {
                    TextBlock b = EmployeeGrid.Columns[i].GetCellContent(EmployeeGrid.Items[j]) as TextBlock;
                    Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                    myRange.Value2 = b.Text;
                }
            }

        }

        private void dobav_click(object sender, RoutedEventArgs e)
        {
            AddWindow m = new AddWindow();
            m.Show();
            Close();
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить сотрудника", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var CurrentUser = EmployeeGrid.SelectedItem as Сотрудники;
                DataAcEntities1.GetContext().Сотрудники.Remove(CurrentUser);
                DataAcEntities1.GetContext().SaveChanges();

                EmployeeGrid.ItemsSource = DataAcEntities1.GetContext().Сотрудники.ToList();
                MessageBox.Show("Сотрудник удален");
            }
        }

        private void red_click(object sender, RoutedEventArgs e)
        {
            
            object abonent = EmployeeGrid.SelectedItem;
            Сотрудники abonent1 = abonent as Сотрудники;
            AddWindow menu = new AddWindow(abonent1);
            menu.Show();
            Close();
            var CurrentUser = EmployeeGrid.SelectedItem as Сотрудники;
            DataAcEntities1.GetContext().Сотрудники.Remove(CurrentUser);
            DataAcEntities1.GetContext().SaveChanges();

            EmployeeGrid.ItemsSource = DataAcEntities1.GetContext().Сотрудники.ToList();
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            Menu m = new Menu();
            m.Show();
            Close();
        }
    }
}
