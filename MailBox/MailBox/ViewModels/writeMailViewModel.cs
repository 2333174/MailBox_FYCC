using MailBox.Models;
using MailBox.Commands;
using System;
using MailBox.Services;
using MailBox.Views;
using MaterialDesignThemes.Wpf;
using MimeKit;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;

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

            Process p = new Process();
            p.StartInfo.FileName = "MimeWarpped.exe";//需要执行的文件路径
            p.StartInfo.UseShellExecute = false; //必需
            p.StartInfo.RedirectStandardOutput = true;//输出参数设定
            p.StartInfo.RedirectStandardInput = true;//传入参数设定
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = MailContent+" " + AccountInfo.Account+" "+ ReceiveMail+" "+ Subject;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

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
				body = output,
			};
			Int32 result = MailUtil.login_send_mail_extern(info_smtp, mail_info);
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
