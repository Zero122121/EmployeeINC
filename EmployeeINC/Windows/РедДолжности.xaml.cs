using System.Windows;
using EmployeeINC.Database.Tables;

namespace EmployeeINC.Windows
{
    public partial class РедДолжности : Window
    {
        private readonly Должности _должность;

        public РедДолжности(Должности должность)
        {
            _должность = должность;
            InitializeComponent();
            
            Наименование.Text = должность.Наименование;
            Ставка.Text = должность.Ставка.ToString();
        }

        private void SaveEdited(object sender, RoutedEventArgs e)
        {
            DB.Database.Query($"UPDATE Должности " +
                              $"SET Наименованиеи = '{Наименование.Text}', " +
                              $"Ставка = '{Ставка.Text}'"+
                              $"WHERE ID_Должности = {_должность.ID_Должности}");
            
            var dolshnosti = new dolshnosti();
            dolshnosti.Show();
            Close();
        }
    }
}