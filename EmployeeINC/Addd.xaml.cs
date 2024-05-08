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
    /// Логика взаимодействия для Addd.xaml
    /// </summary>
    /// 
    
    public partial class Addd : Window
    {
        private Больничные _employee;
        public Addd(Больничные больничные = null)
        {
            InitializeComponent();
            bindcombo();
            _employee = больничные;
            if (больничные != null)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            ComboDol.Text = Convert.ToString(_employee.ID_Сотрудника);
            DateNach.SelectedDate = _employee.Дата_начала;
            DateCon.SelectedDate = _employee.Дата_завершения;
            phone.Text = Convert.ToString( _employee.Номер_больничного);
            di.Text = _employee.Диагноз;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Больничные сотрудники = new Больничные();

            сотрудники.ID_Сотрудника = Convert.ToInt32(ComboDol.Text);
            сотрудники.Дата_начала = DateNach.SelectedDate;
            сотрудники.Дата_завершения = DateCon.SelectedDate;
            сотрудники.Номер_больничного = Convert.ToInt32(phone.Text);
            сотрудники.Диагноз = di.Text;

            DataAcEntities1.GetContext().Больничные.Add(сотрудники);
            DataAcEntities1.GetContext().SaveChanges();
            MessageBox.Show("Данные были добавлены в базу");
            hospitals em = new hospitals();
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

        

        private void RemoveText(object sender, EventArgs e)
        {
            if (phone.Text == "Введите номер больничного")
            {
                phone.Text = "";
                phone.Foreground = Brushes.White;
            }
        }
        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(phone.Text))
            {
                phone.Text = "Введите номер больничного";
                phone.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText2(object sender, EventArgs e)
        {
            if (di.Text == "Введите диагноз")
            {
                di.Text = "";
                di.Foreground = Brushes.White;
            }
        }
        private void AddText2(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(di.Text))
            {
                di.Text = "Введите диагноз";
                di.Foreground = Brushes.Gray;
            }
        }
    }
}
