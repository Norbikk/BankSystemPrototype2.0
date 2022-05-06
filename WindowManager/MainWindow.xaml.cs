using System.Windows;


namespace BankSystemPrototype
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new MainPagee();
            UserDataBase.users = WorkWithJSON.DeserializePersonJson("db.json");
        }


    }
}
