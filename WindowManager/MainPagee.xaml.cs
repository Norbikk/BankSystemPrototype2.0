using System.Windows;
using System.Windows.Controls;

namespace BankSystemPrototype
{
    public partial class MainPagee : Page
    {
        public MainPagee()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new DataPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new CreatePage();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ControlPage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}