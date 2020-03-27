using MailBox.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MailBox.Commands;
using System.Collections.ObjectModel;
using MailBox.Models;
using MaterialDesignThemes.Wpf;
using System.Windows;

namespace MailBox.ViewModels
{
    class HomeViewModel:NotificationObject
    {
		private string title;
		public Window window;
		public string Title
		{
			get { return title; }
			set
			{
				title = value;
				this.RaisePropertyChanged("Title");
			}
		}

		private object visibility;

		public object Visibility
		{
			get { return visibility; }
			set
			{
				visibility = value;
				this.RaisePropertyChanged("Visibility");
			}
		}

		private object content;

		public object Content
		{
			get { return content; }
			set
			{
				content = value;
				this.RaisePropertyChanged("Content");
			}
		}

		/**
         * 账号信息结合
         */
		private ObservableCollection<AccountInfo> accountInfos = new ObservableCollection<AccountInfo>();

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
		private int accountSelectedIndex;

		public int AccountSelectedIndex
		{
			get { return accountSelectedIndex; }
			set
			{
				Console.WriteLine(value);
				accountSelectedIndex = value;
				this.RaisePropertyChanged("AccountSelected");
			}
		}

		public DelegateCommand NewMailCommand { get; set; }

		private void NewMail(object parameter)
		{
			if (Title == "写信") return;
			Title = "写信";
			Visibility = System.Windows.Visibility.Hidden;
			Content = new Frame
			{
				Content = new WriteMailController(AccountInfos[AccountSelectedIndex])
			};
		}

		public DelegateCommand ReceiveMailCommand { get; set; }

		// switch mail user function
		private void ReceiveMail(object parameter)
		{
			Title = "收件箱";
			Visibility = System.Windows.Visibility.Visible;
			Content = new Frame
			{
				Content = new ReceiveMailController(AccountInfos[AccountSelectedIndex], false) // don't flush
			};
		}

		public DelegateCommand FreshCommand { get; set; }
		private void FreshMail(object parameter)
		{
			//Visibility = System.Windows.Visibility.Visible;
			//Content = new Frame
			//{
			//	Content = new ReceiveMailController(AccountInfos[AccountSelectedIndex], true)
			//};
			ShowFreshDialog(parameter);
		}
		private async void ShowFreshDialog(object parameter)
		{
			DialogOpenedEventHandler openedEventHandler = null;
			DialogClosingEventHandler closingEventHandler = null;
			Console.WriteLine("Parameter:", parameter);
			await DialogHost.Show(new FreshProgessController(), openedEventHandler, closingEventHandler);
		}
		public HomeViewModel(ObservableCollection<AccountInfo> accountInfos, int selectIndex)
		{
			AccountInfos = accountInfos;
			AccountSelectedIndex = selectIndex;
			title = "收件箱";
			visibility = System.Windows.Visibility.Visible;
			Content = new Frame
			{
				Content = new ReceiveMailController(AccountInfos[AccountSelectedIndex], true)
			};
			NewMailCommand = new DelegateCommand();
			NewMailCommand.ExecuteAction = new Action<object>(NewMail);
			ReceiveMailCommand = new DelegateCommand();
			ReceiveMailCommand.ExecuteAction = new Action<object>(ReceiveMail);
			FreshCommand = new DelegateCommand();
			FreshCommand.ExecuteAction = new Action<object>(FreshMail);
		}
	}
}
