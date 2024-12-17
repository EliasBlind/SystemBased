using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBased
{
    internal class Employee
    {
        public const string TableName = "Employees";
        public int Id { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public string ContactInformation { get; set; }

        private DataBase _dataBase = new DataBaseBuild().SetTableName(Employee.TableName).Build();

        public List<Employee> AllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            List<int> id = _dataBase.GetСolumn<int>(EmployeeColumns.Id.ToString());
            List<string> gender = _dataBase.GetСolumn<string>(EmployeeColumns.Gender.ToString());
            List<string> name = _dataBase.GetСolumn<string>(EmployeeColumns.Name.ToString());
            List<string> lastName = _dataBase.GetСolumn<string>(EmployeeColumns.Last_Name.ToString());
            List<string> patronymic = _dataBase.GetСolumn<string>(EmployeeColumns.Patronymic.ToString());
            List<string> position = _dataBase.GetСolumn<string>(EmployeeColumns.Position.ToString());
            List<string> contactInformations = _dataBase.GetСolumn<string>(EmployeeColumns.Contact_Information.ToString());

            for (int i = 0; i < _dataBase.Rows; i++)
            {
                Employee employe = new Employee
                {
                    Id = id[i],
                    Gender = gender[i],
                    Name = name[i],
                    LastName = lastName[i],
                    Patronymic = patronymic[i],
                    Position = position[i],
                    ContactInformation = contactInformations[i],
                };
                employees.Add(employe);
            }
            return employees;
        }
    }

    public static class EmployeeStrings
    {
        public const string Name = "*Имя";
        public const string LastName = "*Фамилия";
        public const string Patronymic = "*Отчество";
        public const string Gender = "*Пол";
        public const string Position = "*Должность";
        public const string ContactInformation = "*Контактная информация";
    }

    public enum EmployeeColumns
    {
        Id,
        Gender,
        Name,
        Last_Name,
        Patronymic,
        Position,
        Contact_Information,
    }
}
