using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;



namespace BankSystemPrototype
{
    public partial class ControlPage : Page
    {
        private WorkWithJSON workWithJSON = new WorkWithJSON();

        public ControlPage()
        {
            InitializeComponent();
            UsersList.ItemsSource = UserDataBase.users;

        }
        /// <summary>
        /// Выбор юзера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnClickUser(object sender, RoutedEventArgs e)
        {
            if (UsersList.SelectedIndex == -1)
            {
                return;
            }
            ResetField();
            UsersList.SelectedItem = UserDataBase.users[UsersList.SelectedIndex];
            SelectionItems();


        }
        /// <summary>
        /// Выбранные айтемы
        /// </summary>
        private void SelectionItems()
        {
            var userInfo = UserDataBase.users[UsersList.SelectedIndex];
            NameBox.Text = userInfo.Name;
            SurnameBox.Text = userInfo.Surname;
            AccountBox.ItemsSource = userInfo.Accounts.Select(x => x.GetInfo());
        }
        /// <summary>
        /// Кнопка НАЗАД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new MainPagee();
        }


        /// <summary>
        /// Кнопка для пополнения денег
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            TryingErrors(() =>
            {
                var account = new AccountTransactions<Account>();
                var cash = Convert.ToInt64(AddMoneyBox.Text);
                var user = UserDataBase.users[UsersList.SelectedIndex];

                user.Post += Journal.PostMessage;
                user.PutMoneyMessage(cash);

                account.AddMoney(cash, UsersList.SelectedIndex, AccountBox.SelectedIndex, CompleteTransaction, AddMoneyBox.Text);

                user.Post -= Journal.PostMessage;

            }, AddMoneyBox.Text);

            }
        /// <summary>
        /// Кнопка для пересылки денег
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {

            TryingErrors(() =>
            {
                var account = new AccountTransactions<NonDepositeAccount>();
                var cash = Convert.ToInt64(HowMuchBox.Text);

                var user = UserDataBase.users[UsersList.SelectedIndex];
                var reciever = UserDataBase.GetUsersByListView(ToWhomListView);

                user.Post += Journal.PostMessage;
                user.MoneyTransaction(reciever, cash);

                account.TransferMoney(UsersList.SelectedIndex, ToWhomListView, AccountBox.SelectedIndex, cash, CompleteTransaction, HowMuchBox.Text);
            }, HowMuchBox.Text);

        }
        /// <summary>
        /// Кнопка для Перевода денег внутри аккаунта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferInDeposit(object sender, RoutedEventArgs e)
        {

            TryingErrors(() =>
            {
                var cash = Convert.ToInt64(AddMoneyBox1.Text);
                var user = UserDataBase.users[UsersList.SelectedIndex];

                user.Post += Journal.PostMessage;

                if (DepositRB.IsChecked == true)
                {
                    var transaction = new AccountTransactions<DepositeAccount>();
                    transaction.TransferMoneyInAccount(cash, UsersList.SelectedIndex, typeof(NonDepositeAccount), CompleteTransaction, AddMoneyBox1.Text);
                    user.TransferMoneyInDeposit(cash, "Недепозитный");

                }
                else if (NondepositRB.IsChecked == true)
                {
                    var transaction = new AccountTransactions<NonDepositeAccount>();
                    transaction.TransferMoneyInAccount(cash, UsersList.SelectedIndex, typeof(DepositeAccount), CompleteTransaction, AddMoneyBox1.Text);
                    user.TransferMoneyInDeposit(cash, "Депозитный");

                }
                else
                {
                    return;
                }
                user.Post -= Journal.PostMessage;
            }, AddMoneyBox1.Text);

            //  workWithJSON.DatabaseToJson(UserDataBase.users, "db.json");
        }
        /// <summary>
        /// Кнопка для закрытия аккаунта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAccount_Click(object sender, RoutedEventArgs e)
        {
            var account = UserDataBase.users[UsersList.SelectedIndex].Accounts[AccountBox.SelectedIndex];
            var user = UserDataBase.users[UsersList.SelectedIndex];
            if (account.Cash != 0)
            {
                MessageBox.Show($"Для закрытия счета, на нем не должно быть денег!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (account.isClosed)
            {
                MessageBox.Show($"Счет уже закрыт", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            account.Close();

            user.Post += Journal.PostMessage;
            user.CloseAccountMessage();


            AccountBox.ItemsSource = user.Accounts.Select(x => x.GetInfo());
            workWithJSON.DatabaseToJson(UserDataBase.users, "db.json");
            ResetField();
        }

        /// <summary>
        /// Вывод нужных окон при нажатии на "Выбор операции"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOperationBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var select = SelectOperationBox.SelectedItem;
            if (select == AddMoneyItem)
            {
                AddMoneyPanel.Visibility = Visibility.Visible;
                TransferGrid.Visibility = Visibility.Hidden;
                TransferInNonDeposPanel.Visibility = Visibility.Hidden;
            }
            else if (select == TransferMoneyItem)
            {
                AddMoneyPanel.Visibility = Visibility.Hidden;
                TransferGrid.Visibility = Visibility.Visible;
                TransferInNonDeposPanel.Visibility = Visibility.Hidden;
            }
            else if (select == TransferInAccounts)
            {
                TransferInNonDeposPanel.Visibility = Visibility.Visible;
                AddMoneyPanel.Visibility = Visibility.Hidden;
                TransferGrid.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Выбор элемента в Комбобоксе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccountBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = UserDataBase.users[UsersList.SelectedIndex];
            if (UsersList.SelectedIndex == -1 || AccountBox.SelectedIndex == -1)
            {
                return;
            }
            ResetAccountFields();
            SelectedAccountBox.Text = user.Accounts[AccountBox.SelectedIndex].AccountNumber.ToString();
            CashBox.Text = user.Accounts[AccountBox.SelectedIndex].Cash.ToString();


            var selectedItem = AccountBox.SelectedItem.ToString();
            if (selectedItem == "Депозитный аккаунт")
            {
                TransferInAccounts.IsEnabled = true;
                TransferMoneyItem.IsEnabled = false;
                SelectOperationBox.Visibility = Visibility.Visible;

            }
            if (selectedItem == "Недепозитный аккаунт")
            {
                ToWhomListView.ItemsSource = UserDataBase.users.Where(x => x.Id != UserDataBase.users[UsersList.SelectedIndex].Id);
                TransferInAccounts.IsEnabled = false;
                TransferMoneyItem.IsEnabled = true;
                SelectOperationBox.Visibility = Visibility.Visible;
            }
            if (selectedItem.Contains("[ЗАКРЫТ]"))
            {
                SelectOperationBox.Visibility = Visibility.Hidden;

            }

        }
        /// <summary>
        /// Скрытие окон операций
        /// </summary>
        private void ResetAccountFields()
        {
            AddMoneyPanel.Visibility = Visibility.Hidden;
            TransferGrid.Visibility = Visibility.Hidden;
            TransferInNonDeposPanel.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Скрытие всей правой части
        /// </summary>
        private void ResetField()
        {
            AccountBox.SelectedItem = null;
            SelectOperationBox.SelectedItem = null;
            SelectOperationBox.Visibility = Visibility.Hidden;
            AddMoneyPanel.Visibility = Visibility.Hidden;
            TransferGrid.Visibility = Visibility.Hidden;
            TransferInNonDeposPanel.Visibility = Visibility.Hidden;
            CashBox.Text = null;
            SelectedAccountBox.Text = null;
            HowMuchBox.Text = null;
            AddMoneyBox.Text = null;
        }

        private void CompleteTransaction(string where)
        {
            var time = DateTime.Now;
            CashBox.Text = UserDataBase.users[UsersList.SelectedIndex].Accounts[AccountBox.SelectedIndex].Cash.ToString();
            if (where == AddMoneyBox.Text)
            {
                MessageBox.Show($"Вы успешно пополнили счет на {where}\nВремя проведения операции {time}", "Получилось!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Деньги успешно переведены в кол-ве {where}\nВремя проведения операции {time}", $"Получилось!", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            workWithJSON.DatabaseToJson(UserDataBase.users, "db.json");

        }

        private void TryingErrors(Action action, string textBox)
        {
            try
            {

                if (!Int64.TryParse(textBox, out long result))
                {
                    throw new MoneyValueException(1);
                }
                else if (textBox.Length > 10)
                {
                    throw new MoneyValueException(2);
                }
                action?.Invoke();

            }
            catch (MoneyValueException d) when (d.Count == 1)
            {
                MessageBox.Show($"Введите числовые данные!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (MoneyValueException d) when (d.Count == 2)
            {
                MessageBox.Show($"Можно ввести до 10 символов!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}