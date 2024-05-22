﻿using System;
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
using EmployeeINC.Database.Tables;

namespace EmployeeINC
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        private Отпуски _employee;
        private bool _isEdit;

        public Add(Отпуски отпуски = null)
        {
            InitializeComponent();
            bindcombo();
            _employee = отпуски;
            if (отпуски != null)
            {
                _isEdit = true;
                LoadData();
            }
        }

        private void LoadData()
        {
            ComboDol.Text = Convert.ToString(_employee.ID_Сотрудника);
            DateNach.SelectedDate = DateTime.Parse(_employee.Дата_начала);
            DateCon.SelectedDate = DateTime.Parse(_employee.Дата_завершения);
        }

        private void add_click(object sender, RoutedEventArgs e)
        {
            Отпуски сотрудники = new Отпуски
            {
                ID_Сотрудника = Convert.ToInt32(ComboDol.Text),
                Дата_начала = DateNach.SelectedDate.ToString(),
                Дата_завершения = DateCon.SelectedDate.ToString()
            };
            if (_isEdit)
            {
                DB.Database.Query($"UPDATE Отпуски SET Дата_начала = '{сотрудники.Дата_начала}', Дата_завершения = '{сотрудники.Дата_завершения}', ID_Сотрудника = {сотрудники.ID_Сотрудника} " +
                                  $"WHERE ID_Отпуска = {сотрудники.ID_Отпуска}");
            }
            else
            {
                DB.Database.Query(
                    $"INSERT INTO Отпуски (Дата_начала, Дата_завершения, ID_Сотрудника) VALUES ('{сотрудники.Дата_начала}', '{сотрудники.Дата_завершения}', {сотрудники.ID_Сотрудника})");
            }

            MessageBox.Show("Данные были добавлены в базу");
            Vacations em = new Vacations();
            em.Show();
            Close();
        }

        public Сотрудники[] Dolsh { get; set; }

        private void bindcombo()
        {
            var items = (Сотрудники[])new Сотрудники().ConvertToTables(
                DB.Database.ExecuteQuery($"SELECT * FROM Сотрудники;"));
            Dolsh = items;
            DataContext = Dolsh;
        }
    }
}