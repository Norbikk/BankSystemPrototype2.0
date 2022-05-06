using System.Windows;
using System.Windows.Controls;

namespace BankSystemPrototype
{
    public partial class DataPage : Page
    {
        public DataPage()
        {
            InitializeComponent();
            UsersList.ItemsSource = UserDataBase.users;
        }
        /// <summary>
        /// Кнопка НАЗАД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainFrame.Content = new MainPagee();
        }

        public void OnClickUser(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedIndex == -1)
            {
                return;
            }
            UsersList.SelectedItem = UserDataBase.users[UsersList.SelectedIndex];
            SelectionItems();
        }
        private void SelectionItems()
        {
            var userInfo = UserDataBase.users[UsersList.SelectedIndex];
            NameBox.Text = userInfo.Name;
            SurnameBox.Text = userInfo.Surname;
            SecondnameBox.Text = userInfo.Secondname;
            PhoneBox.Text = userInfo.PhoneNumber;
            BirthdayBox.Text = userInfo.BirthDay.ToShortDateString();
            DepositeAccountBox.Text = AccountInfo("deposit");
            NonDepositeAccountBox.Text = AccountInfo("nonDeposit");


        }
        /// <summary>
        /// Возвращает строку с информацией
        /// </summary>
        /// <param name="accountType"></param>
        /// <returns></returns>
        private string AccountInfo(string accountType)
        {
            var user = UserDataBase.users[UsersList.SelectedIndex];
            for (int i = 0; i < user.Accounts.Count; i++)
            {
                if (user.Accounts[i] is DepositeAccount && accountType == "deposit")
                {
                    return user.Accounts[i].AccountNumber.ToString(); 
                }
                else if (user.Accounts[i] is NonDepositeAccount && accountType == "nonDeposit")
                {
                    return user.Accounts[i].AccountNumber.ToString();
                }
            }

            return "Нет";
           

        }
    }
}