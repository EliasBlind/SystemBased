using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace SystemBased.Screens
{
    /// <summary>
    /// Логика взаимодействия для Delite.xaml
    /// </summary>
    public partial class Delite : Page
    {
        private DataBase _dataBase = null;
        private int _key;
        public Delite()
        {
            InitializeComponent();
        }

        private void MyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MyComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedValue = selectedItem.Content.ToString();
                if (selectedValue == "Сотрудники")
                    _dataBase = new DataBaseBuild().SetTableName(Employee.TableName).Build();
                else
                    _dataBase = new DataBaseBuild().SetTableName(Position.TableName).Build();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int key;

            if (!int.TryParse(KeyBox.Text, out key))
            {
                MessageBox.Show("Данные не найдены");
                return;
            }

            // Ensure _dataBase is set
            if (_dataBase == null)
            {
                MessageBox.Show("Выбирите таблицу");
                return;
            }
            List<object> data = _dataBase.GetRow(key);
            // Ensure that PrimaryKey has at least one element
            if (data == null || 
                !data.Contains(key))
            {
                MessageBox.Show("Данные не найдены");
                return;
            }
            DataBox.Text = string.Empty;
            foreach (var item in data)
            {
                // Check if item is null before converting to string
                if (item != null)
                {
                    DataBox.Text += item.ToString().Replace("  ", " ") + '\t';
                }
            }

            _key = key;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(_dataBase == null || 
                _key == 0)
            {
                MessageBox.Show("Выбирите таблицу и ключ");
                return;
            }
            _dataBase.DeleteRow(_key);
            if(DataBox.Text != "Данные для удаления")
                DataBox.Text = "Error 404";
        }
    }
}
