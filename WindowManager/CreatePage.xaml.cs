using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;


namespace BankSystemPrototype
{
    public partial class CreatePage : Page
    {
        private DepositeAccount depAccount = new DepositeAccount();
        private NonDepositeAccount nonDepAccount = new NonDepositeAccount();
        private WorkWithJSON workWithJSON = new WorkWithJSON();

        private Journal journal = new Journal();

        public CreatePage()
        {
            InitializeComponent();
            
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
        /// <summary>
        /// Кнопка создания аккаунта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                var user = new User();

                user.Name = NameBox.Text;
                user.Surname = SurnameBox.Text;
                user.Secondname = SecondnameBox.Text;
                user.PhoneNumber = PhoneBox.Text;
                ChekBoxChecking(user, DepositeCheckBox, depAccount);
                ChekBoxChecking(user, NonDepositeCheckBox, nonDepAccount);
                user.BirthDay = BirthDate.SelectedDate.Value;
                user.Id = Guid.NewGuid();



                UserDataBase.users.Add(user);
                user.Post += Journal.PostMessage;
                user.CreateUserMessage();

                workWithJSON.DatabaseToJson(UserDataBase.users, "db.json");

                Refresh();
            }
            catch
            {
                MessageBox.Show($"Введите корректные данные!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        /// <summary>
        /// Общий метод проверки чекбокса
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cb"></param>
        /// <param name="account"></param>
        private void ChekBoxChecking(User user,CheckBox cb, Account account)
        {
            if ((bool)cb.IsChecked)
            {
                user.Accounts.Add(account.CreateAccount());
            }

        }
        /// <summary>
        /// Обновление страницы
        /// </summary>
        private void Refresh()
        {
            MainFrame.Content = new CreatePage();
        }

       
     
    }
}