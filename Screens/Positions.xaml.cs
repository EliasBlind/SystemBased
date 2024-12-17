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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemBased
{
    /// <summary>
    /// Логика взаимодействия для Positions.xaml
    /// </summary>
    public partial class Positions : Page
    {
        private readonly DataBase _dataBase = new DataBaseBuild().SetTableName(Position.TableName).Build();
        public Positions()
        {
            InitializeComponent();
        }

        private void positionsLoad(object sender, RoutedEventArgs e)
        {
            loadPositionsTable();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!validatePositionData())
            {
                MessageBox.Show("Пожалуйста, заполните обязательные поля (Название должности и часы работы в неделю)");
                return;
            }

            // Очищаем данные для следующего ввода
            clearPositionData();

            // Создаем список для записи данных в базу
            List<object> row = new List<object>()
            {
                PositionNameBox.Text,          // Название должности
                DescriptionBox.Text,           // Описание
                SalaryBox.Text, // Зарплата
                PrizeBox.Text,     // Премия
                WorkingHoursBox.Text, // Часы работы в неделю
            };

            // Записываем данные в базу
            _dataBase.Write(row);
            setPositionData();
            // Загружаем обновленную таблицу
            loadPositionsTable();
        }

        private void loadPositionsTable()
        {
            Position position = new Position();
            PositionsDataGrid.ItemsSource = position.AllPositions();
        }

        private bool validatePositionData()
        {
            bool result = true;
            if (string.IsNullOrWhiteSpace(PositionNameBox.Text) ||
                PositionNameBox.Text == PositionStrings.PositionName ||
                string.IsNullOrWhiteSpace(WorkingHoursBox.Text) ||
                WorkingHoursBox.Text == PositionStrings.WorkingHoursPerWeek)
            {
                result = false;
            }
            return result;
        }

        private void setPositionData()
        {
            PositionNameBox.Text = PositionStrings.PositionName;
            DescriptionBox.Text = PositionStrings.Description;
            SalaryBox.Text = PositionStrings.Salary;
            PrizeBox.Text = PositionStrings.Prize;
            WorkingHoursBox.Text = PositionStrings.WorkingHoursPerWeek;
        }

        private void clearPositionData()
        {
            // Проверка и установка для DescriptionBox
            DescriptionBox.Text = DescriptionBox.Text == PositionStrings.Description ? default : DescriptionBox.Text;

            // Проверка и установка для SalaryBox
            SalaryBox.Text = SalaryBox.Text == PositionStrings.Salary ? default : SalaryBox.Text;

            // Проверка и установка для PrizeBox
            PrizeBox.Text = PrizeBox.Text == PositionStrings.Prize ? default : PrizeBox.Text;
        }


    }
}
