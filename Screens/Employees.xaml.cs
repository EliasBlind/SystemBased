using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SystemBased
{
    /// <summary>
    /// Логика взаимодействия для Employees.xaml
    /// </summary>
    public partial class Employees : Page
    {
        private readonly DataBase _dataBase = new DataBaseBuild().SetTableName(Employee.TableName).Build();
        public Employees()
        {
            InitializeComponent();
        }

        private void employeesLoad(object sender, RoutedEventArgs e)
        {
            loadEmployeesTable();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!validateData())
            {
                MessageBox.Show("Пожалуйста заполните обязательные поля (Имя, Фамилия, Должность)");
                return;
            }
            clearData();
            List<object> row = new List<object>()
            {
                GenderBox.Text,          // Пол
                NameBox.Text,           // Имя
                LastNameBox.Text,       // Фамилия
                PatronymicBox.Text,     // Отчество
                PositionBox.Text,       // Должность
                ContactInformationBox.Text, // Контактная информация
            };
            _dataBase.Write(row);
            setData();
            loadEmployeesTable();
        }

        private void loadEmployeesTable()
        {
            Employee employees = new Employee();
            EmployeesDataGrid.ItemsSource = employees.AllEmployees();
        }

        private bool validateData()
        {
            bool result = true;
            if (string.IsNullOrWhiteSpace(NameBox.Text) ||
                 NameBox.Text == EmployeeStrings.Name ||
                 string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                 LastNameBox.Text == EmployeeStrings.LastName||
                 string.IsNullOrWhiteSpace(PositionBox.Text) ||
                 PositionBox.Text == EmployeeStrings.Position)
                result = false;
            return result;
        }

        private void setData()
        {
            NameBox.Text = EmployeeStrings.Name;
            LastNameBox.Text = EmployeeStrings.LastName;
            PatronymicBox.Text = EmployeeStrings.Patronymic;
            GenderBox.Text = EmployeeStrings.Gender;
            PositionBox.Text = EmployeeStrings.Position;
            ContactInformationBox.Text = EmployeeStrings.ContactInformation;
        }

        private void clearData()
        {
            // Проверка и установка для GenderBox
            GenderBox.Text = GenderBox.Text == EmployeeStrings.Gender ? default : GenderBox.Text;

            // Проверка и установка для PatronymicBox
            PatronymicBox.Text = PatronymicBox.Text == EmployeeStrings.Patronymic ? default : PatronymicBox.Text;

            // Проверка и установка для ContactInformationBox
            ContactInformationBox.Text = ContactInformationBox.Text == EmployeeStrings.ContactInformation ? default : ContactInformationBox.Text;
        }
    }
}
