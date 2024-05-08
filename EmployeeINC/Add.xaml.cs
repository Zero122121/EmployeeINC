using System;
using System.Collections.Generic;
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

namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        private Отпуски _employee;
        public Add(Отпуски отпуски = null)
        {
            InitializeComponent();
            bindcombo();
            _employee = отпуски;
            if (отпуски != null)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            ComboDol.Text = Convert.ToString(_employee.ID_Cотрудника);
            DateNach.SelectedDate = _employee.Дата_начала;
            DateCon.SelectedDate = _employee.Дата_завершения;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Отпуски сотрудники = new Отпуски();

            сотрудники.ID_Cотрудника =Convert.ToInt32(ComboDol.Text);
            сотрудники.Дата_начала = DateNach.SelectedDate;
            сотрудники.Дата_завершения = DateCon.SelectedDate;

            DataAcEntities1.GetContext().Отпуски.Add(сотрудники);
            DataAcEntities1.GetContext().SaveChanges();
            MessageBox.Show("Данные были добавлены в базу");
            Vacations em = new Vacations();
            em.Show();
            this.Close();
        }

        public List<Сотрудники> Dolsh { get; set; }
        private void bindcombo()
        {
            DataAcEntities1 dc = new DataAcEntities1();

            var items = dc.sot.ToList();
            Dolsh = items;
            DataContext = Dolsh;
        }

        
    }
}
