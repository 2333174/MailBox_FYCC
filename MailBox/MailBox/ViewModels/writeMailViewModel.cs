using MailBox.Models;
using MailBox.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailBox.Services;
using MailBox.Views;
using MaterialDesignThemes.Wpf;

namespace MailBox.ViewModels
{
    class WriteMailViewModel:NotificationObject
    {
		private string subject;

		public string Subject
		{
			get { return subject; }
			set
			{
				subject = value;
				this.RaisePropertyChanged("Subject");
			}
		}

		private string receiveMail;

		public string ReceiveMail
		{
			get { return receiveMail; }
			set
			{
				receiveMail = value;
				this.RaisePropertyChanged("ReceiveMail");
			}
		}

		private string mailContent;

		public string MailContent
		{
			get { return mailContent; }
			set
			{
				mailContent = value;
				this.RaisePropertyChanged("MailContent");
			}
		}


		private AccountInfo accountInfo;

		public AccountInfo AccountInfo
		{
			get { return accountInfo; }
			set
			{
				accountInfo = value;
				this.RaisePropertyChanged("AccountInfo");
			}
		}

		public DelegateCommand SendCommand { get; set; }

		private void SendMail(object paramter)
		{
			MailUtil.LoginInfo info_smtp = new MailUtil.LoginInfo()
			{
				account = AccountInfo.Account,
				passwd = AccountInfo.Password,
				site = AccountInfo.SmtpHost
			};

			MailUtil.MailInfo mail_info = new MailUtil.MailInfo()
			{
				from = AccountInfo.Account,
				to = ReceiveMail,
				cc = AccountInfo.Account,
				subject = Subject,
				body = MailContent,
			};
			Int32 result = MailUtil.login_send_mail(info_smtp, mail_info);
			if (result == 200)
			{
				ShowDialog("发送成功");
				ClearInfo(null);
			}
			else
			{
				ShowDialog("发送失败");
			}
			Console.WriteLine(result);
		}


		public DelegateCommand ClearCommand { get; set; }

		private void ClearInfo(object paramter)
		{
			Console.WriteLine("重置");
			Subject = "";
			MailContent = "";
			ReceiveMail = "";
		}

		public WriteMailViewModel(AccountInfo accountInfo)
		{
			AccountInfo = accountInfo;
			SendCommand = new DelegateCommand();
			SendCommand.ExecuteAction = new Action<object>(SendMail);
			ClearCommand = new DelegateCommand();
			ClearCommand.ExecuteAction = new Action<object>(ClearInfo);
		}

		private async void ShowDialog(string message)
		{
			DialogClosingEventHandler dialogClosingEventHandler = null;
			MessageController messageController = new MessageController(message);
			await DialogHost.Show(messageController, "MessageDialog", dialogClosingEventHandler);
		}
	}
}
