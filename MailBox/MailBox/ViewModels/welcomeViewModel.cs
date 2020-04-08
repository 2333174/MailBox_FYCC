using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailBox.Commands;
using MailBox.Models;
using MailBox.Views;
using MaterialDesignThemes.Wpf;
using MailBox.Services;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MailBox.ViewModels
{
    class WelcomeViewModel : NotificationObject
	{
        private ContentControl contentControl;
        public WelcomeViewModel(ContentControl content)
        {
            this.openLoginCommand = new DelegateCommand();
            this.openLoginCommand.ExecuteAction = new Action<object>(this.openLogin);
            this.enterMailBoxCommand = new DelegateCommand();
            this.enterMailBoxCommand.ExecuteAction = new Action<object>(this.enterMailBox);
            //读取账户
            AccountInfos=XMLOperation.loadAccouts();
            isAble = false;
            accountSelectedIndex = -1;
            contentControl = content;
        }

        /**
         * 账号信息结合
         */
        private ObservableCollection<AccountInfo> accountInfos =new ObservableCollection<AccountInfo>();

        public ObservableCollection<AccountInfo> AccountInfos
        {
            get { return accountInfos; }
            set
            {
                accountInfos = value;
                this.RaisePropertyChanged("AccountInfos");
            }
        }

        /**
         * 被选择的账号索引
         */
        private int accountSelectedIndex        ;

        public int AccountSelectedIndex
        {
            get { return accountSelectedIndex; }
            set
            {
                Console.WriteLine(value);
                accountSelectedIndex = value;
                if (accountSelectedIndex != -1)
                {
                    IsAble = true;
                }
                else
                {
                    IsAble = false;
                }
                this.RaisePropertyChanged("AccountSelected");
            }
        }

        /**
         * 进入收件箱按钮是否可用
         */
        private bool isAble;

        public bool IsAble
        {
            get { return isAble; }
            set
            {
                isAble = value;
                this.RaisePropertyChanged("IsAble");
            }
        }

        /**
         * 添加账号按钮事件，打开添加账号对话框
         */
        public DelegateCommand openLoginCommand { get; set; }

        private void openLogin(object parameter)
        {
            ShowAddDialog();
        }

        /**
         * 进入收件箱按钮事件
         */
        public DelegateCommand enterMailBoxCommand { get; set; }

        private void enterMailBox(object parameter)
        {
            Console.WriteLine("进入邮箱");
            contentControl.Content = new Frame
            {
                Content = new HomePage(AccountInfos,AccountSelectedIndex,contentControl)
            };
        }


        /**
         * 向xml中添加账户信息
         */
        private void AddAccount(string account, string password, string popHost, string smtpHost)
        {
            AccountInfo accountInfo = new AccountInfo(account, password, popHost, smtpHost);
            XMLOperation.AddAccountNode(accountInfo);
            AccountInfos = XMLOperation.loadAccouts();
        }

        private async void ShowAddDialog()
        {
            DialogClosingEventHandler dialogClosingEventHandler = null;
            LoginController loginController = new LoginController
            {
            };
            var result = await DialogHost.Show(loginController, "loginDialog", dialogClosingEventHandler); 
            if (Equals(true, result)){
                for (int i = 0; i < AccountInfos.Count; i++)
                {
                    if (AccountInfos[i].Account == loginController.NameTextBox.Text)
                    {
                        MessageController messageController = new MessageController("此账号已添加，请勿重复添加");
                        await DialogHost.Show(messageController, "validDialog", dialogClosingEventHandler);
                        return;
                    }
                }
                Console.WriteLine("添加账户");
                MailUtil.LoginInfo info_smtp
                = new MailUtil.LoginInfo()
                {
                    account = loginController.NameTextBox.Text,
                    passwd = loginController.FloatingPasswordBox.Password,
                    site = loginController.SmtpHostTextBox.Text
                };
                MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
                {
                    account = loginController.NameTextBox.Text,
                    passwd = loginController.FloatingPasswordBox.Password,
                    site = loginController.PopHostTextBox.Text,
                };
                Boolean isRight = MailUtil.validate_account_smtp(info_smtp) && (!MailUtil.validate_account_pop3(info_pop3));
                //如果验证成功，则添加账号
                if (isRight)
                {
                    Console.WriteLine("验证成功");
                    AddAccount(loginController.NameTextBox.Text,
                    loginController.FloatingPasswordBox.Password,
                    loginController.PopHostTextBox.Text,
                    loginController.SmtpHostTextBox.Text);
                }
                else
                {
                    MessageController messageController = new MessageController("账号信息错误");
                    await DialogHost.Show(messageController, "validDialog", dialogClosingEventHandler);
                    Console.WriteLine(isRight);
                }  
            }
        }
    }
}
