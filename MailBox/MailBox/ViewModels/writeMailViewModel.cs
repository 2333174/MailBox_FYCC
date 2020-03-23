using MailBox.Models;
using MailBox.Commands;
using System;
using MailBox.Services;
using MailBox.Views;
using MaterialDesignThemes.Wpf;
using MimeKit;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace MailBox.ViewModels
{
    class WriteMailViewModel:NotificationObject
    {
		private ObservableCollection<MailFile> files;

		public ObservableCollection<MailFile> Files
		{
			get { return files; }
			set
			{
				files = value;
				this.RaisePropertyChanged("Paths");
			}
		}
		 //上传文件路径
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

            StringBuilder argv = new StringBuilder();
            argv.Append(MailContent + " " + AccountInfo.Account + " " + ReceiveMail + " " + Subject);
			// 文件路径的数组,路径分隔符是/，相对或绝对都可以
			//string[] paths = { "xxx/1.txt", "xxx/2.jpg" };
			foreach (var file in files)
			{
				argv.Append(" \"" + file.MailFilePath.Replace("\\", "/")+"\"");
			}


			Process p = new Process();
            p.StartInfo.FileName = "MimeWrapped.exe";//需要执行的文件路径
            p.StartInfo.UseShellExecute = false; //必需
            p.StartInfo.RedirectStandardOutput = true;//输出参数设定
            p.StartInfo.RedirectStandardInput = true;//传入参数设定
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.Arguments = argv.ToString();
            
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
				subject = "",
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
			Files.Clear();
		}

		public DelegateCommand UploadFileCommand { get; set; }

		private void UploadFile(object paramter)
		{
			Console.WriteLine("选择文件");
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Title = "选择需要上传的文件";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string txtFile = openFileDialog.FileName;
				string name = txtFile.Split('\\')[txtFile.Split('\\').Length - 1];
				Console.WriteLine(name);
				Files.Add(new MailFile(name,txtFile));
				Console.WriteLine(txtFile);
			}
		}

		public WriteMailViewModel(AccountInfo accountInfo)
		{
			AccountInfo = accountInfo;
			Files = new ObservableCollection<MailFile>();
			SendCommand = new DelegateCommand();
			SendCommand.ExecuteAction = new Action<object>(SendMail);
			ClearCommand = new DelegateCommand();
			ClearCommand.ExecuteAction = new Action<object>(ClearInfo);
			UploadFileCommand = new DelegateCommand();
			UploadFileCommand.ExecuteAction = new Action<object>(UploadFile) ;
		}

		private async void ShowDialog(string message)
		{
			DialogClosingEventHandler dialogClosingEventHandler = null;
			MessageController messageController = new MessageController(message);
			await DialogHost.Show(messageController, "MessageDialog", dialogClosingEventHandler);
		}
	}
}
