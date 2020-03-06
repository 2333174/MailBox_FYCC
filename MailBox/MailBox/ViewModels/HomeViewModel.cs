using MailBox.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MailBox.Commands;

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

		public DelegateCommand NewMailCommand { get; set; }

		private void NewMail(object parameter)
		{
			Title = "写信";
			Visibility = System.Windows.Visibility.Hidden;
			Content = new Frame
			{
				Content = new WriteMailController()
			};
		}

		public HomeViewModel()
		{
			title = "收件箱";
			visibility = System.Windows.Visibility.Visible;
			Content = new Frame
			{
				Content = new ReceiveMailController()
			};
			NewMailCommand = new DelegateCommand();
			NewMailCommand.ExecuteAction = new Action<object>(NewMail);
		}
	}
}
