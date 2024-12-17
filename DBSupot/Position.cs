using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemBased
{
    public static class PositionStrings
    {
        public const string PositionName = "*Должность";
        public const string Description = "*Описание";
        public const string Salary = "*Зарплата";
        public const string Prize = "*Премия";
        public const string WorkingHoursPerWeek = "*Часов работы";
    }

    internal class Position
    {
        public const string TableName = "Positions";

        public int Id { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public double Prize { get; set; }
        public double WorkingHoursPerWeek { get; set; }

        private DataBase _dataBase = new DataBaseBuild().SetTableName(Position.TableName).Build();

        public List<Position> AllPositions()
        {
            List<Position> positions = new List<Position>();
            List<int> id = _dataBase.GetСolumn<int>(PositionColumns.Id.ToString());
            List<string> positionNames = _dataBase.GetСolumn<string>(PositionColumns.Position_Name.ToString());
            List<string> descriptions = _dataBase.GetСolumn<string>(PositionColumns.Description.ToString());
            List<double> salaries = _dataBase.GetСolumn<double>(PositionColumns.Salary.ToString());
            List<double> prizes = _dataBase.GetСolumn<double>(PositionColumns.Prize.ToString());
            List<double> workingHours = _dataBase.GetСolumn<double>(PositionColumns.Working_Hours_Per_Week.ToString());

            for (int i = 0; i < _dataBase.Rows; i++)
            {
                Position position = new Position
                {
                    Id = id[i],
                    PositionName = positionNames[i],
                    Description = descriptions[i],
                    Salary = salaries[i],
                    Prize = prizes[i],
                    WorkingHoursPerWeek = workingHours[i],
                };
                positions.Add(position);
            }
            return positions;
        }
    }

    public enum PositionColumns
    {
        Id,
        Position_Name,
        Description,
        Salary,
        Prize,
        Working_Hours_Per_Week,
    }
}
