using System.Windows;
using SystemBased.Screens;

namespace SystemBased
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Home(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(null);
        }

        private void Positions(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Positions());
        }

        private void Employees(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Employees());
        }

        private void Page4_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(new Delite());
        }
    }
}
