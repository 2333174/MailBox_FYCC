using MailBox.Models;
using MailBox.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

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
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(AccountInfo.Account.Split('@')[0], AccountInfo.Account));
			message.To.Add(new MailboxAddress(ReceiveMail.Split('@')[0], ReceiveMail));
			message.Subject = Subject;

			message.Body = new TextPart("plain")
			{
				Text = MailContent
				
							};
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
	}
}
