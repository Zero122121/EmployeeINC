using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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
    /// Логика взаимодействия для dolshnosti.xaml
    /// </summary>
    public partial class dolshnosti : System.Windows.Window
    {

        public ICommand SortCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;

        public dolshnosti()
        {
            InitializeComponent();
            ShowTable(DataAcEntities1.GetContext().Должности);
        }

        private void ShowTable<T>(DbSet<T> query) where T : class
        {
            EmployeeGrid.ItemsSource = query.ToList();
        }

        private void Search_Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                EmployeeGrid.ItemsSource = DataAcEntities1.GetContext().Должности.Where(item => item.Наименование == Search.Text || item.Наименование.Contains(Search.Text)).ToList();
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
            AddWindoww m = new AddWindoww();
            m.Show();
            Close();
        }

        private void delete_click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить должность", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var CurrentUser = EmployeeGrid.SelectedItem as Должности;
                DataAcEntities1.GetContext().Должности.Remove(CurrentUser);
                DataAcEntities1.GetContext().SaveChanges();

                EmployeeGrid.ItemsSource = DataAcEntities1.GetContext().Должности.ToList();
                MessageBox.Show("Должность удалена");
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
