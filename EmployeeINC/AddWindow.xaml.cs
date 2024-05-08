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
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private Сотрудники _employee;
        public AddWindow(Сотрудники сотрудники = null)
        {
            InitializeComponent();
            bindcombo();
            _employee = сотрудники;
            if (сотрудники != null)
            {
                LoadData();
            }

            
        }

        private void LoadData()
        {
            LastName.Text = _employee.Фамилия;
            Surname.Text = _employee.Имя;
            Otchestvo.Text = _employee.Отчество;
            phone.Text = Convert.ToString(_employee.Телефон);
            ComboOtdel.Text = _employee.Отдел;
            ComboDol.Text = _employee.Должность;
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Сотрудники сотрудники = new Сотрудники();
            сотрудники.Фамилия = LastName.Text;
            сотрудники.Имя = Surname.Text;
            сотрудники.Отчество = Otchestvo.Text;
            сотрудники.Телефон = Convert.ToInt32(phone.Text);
            сотрудники.Отдел = ComboOtdel.Text;
            сотрудники.Должность = ComboDol.Text;

            DataAcEntities1.GetContext().Сотрудники.Add(сотрудники);
            DataAcEntities1.GetContext().SaveChanges();
            MessageBox.Show("Сотрудник был добавлен в базу");
            Employee em = new Employee();
            em.Show();
            this.Close();
            
        }


        public List<Должности> Dolsh {  get; set; }
        private void bindcombo()
        {
            DataAcEntities1 dc = new DataAcEntities1();

            var items = dc.dol.ToList();
            Dolsh = items;
            DataContext = Dolsh;
        }

        private void RemoveText(object sender, EventArgs e)
        {
            if (LastName.Text == "Введите фамилию")
            {
                LastName.Text = "";
                LastName.Foreground = Brushes.White;
            }
        }
        private void AddText(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LastName.Text))
            {
                LastName.Text = "Введите фамилию";
                LastName.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText1(object sender, EventArgs e)
        {
            if (Surname.Text == "Введите имя")
            {
                Surname.Text = "";
                Surname.Foreground = Brushes.White;
            }
        }
        private void AddText1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Surname.Text))
            {
                Surname.Text = "Введите имя";
                Surname.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText2(object sender, EventArgs e)
        {
            if (Otchestvo.Text == "Введите отчество")
            {
                Otchestvo.Text = "";
                Otchestvo.Foreground = Brushes.White;
            }
        }
        private void AddText2(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Otchestvo.Text))
            {
                Otchestvo.Text = "Введите отчество";
                Otchestvo.Foreground = Brushes.Gray;
            }
        }

        private void RemoveText3(object sender, EventArgs e)
        {
            if (phone.Text == "Введите номер телефона")
            {
                phone.Text = "";
                phone.Foreground = Brushes.White;
            }
        }
        private void AddText3(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(phone.Text))
            {
                phone.Text = "Введите номер телефона";
                phone.Foreground = Brushes.Gray;
            }
        }

        
    }
}
