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

namespace MailBox.ViewModels
{
    class HomeViewModel:NotificationObject
    {
		private string title;

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
			Title = "写信";
			Visibility = System.Windows.Visibility.Hidden;
			Content = new Frame
			{
				Content = new WriteMailController()
			};
			AccountSelectedIndex = -1;
		}

		public DelegateCommand ReceiveMailCommand { get; set; }

		private void ReceiveMail(object parameter)
		{
			Title = "收件箱";
			Visibility = System.Windows.Visibility.Visible;
			Content = new Frame
			{
				Content = new ReceiveMailController()
			};
		}

		public HomeViewModel(ObservableCollection<AccountInfo> accountInfos, int selectIndex)
		{
			AccountInfos = accountInfos;
			AccountSelectedIndex = selectIndex;
			title = "收件箱";
			visibility = System.Windows.Visibility.Visible;
			Content = new Frame
			{
				Content = new ReceiveMailController()
			};
			NewMailCommand = new DelegateCommand();
			NewMailCommand.ExecuteAction = new Action<object>(NewMail);
			ReceiveMailCommand = new DelegateCommand();
			ReceiveMailCommand.ExecuteAction = new Action<object>(ReceiveMail);
		}
	}
}
