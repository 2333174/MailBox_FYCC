using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBox.Models
{
	/**
	 * 存储账户信息
	 */
    class AccountInfo
    {

		private string account;

		public string Account
		{
			get { return account; }
			set { account = value; }
		}

		private string password;

		public string Password
		{
			get { return password; }
			set { password = value; }
		}

		private string popHost;

		public string PopHost
		{
			get { return popHost; }
			set { popHost = value; }
		}

		private string smtpHost;

		public string SmtpHost
		{
			get { return smtpHost; }
			set { smtpHost = value; }
		}

		public AccountInfo()
		{
		}

		public AccountInfo(string account, string password, string popHost, string smtpHost)
		{
			Account = account;
			Password = password;
			PopHost = popHost;
			SmtpHost = smtpHost;
		}
	}
}
