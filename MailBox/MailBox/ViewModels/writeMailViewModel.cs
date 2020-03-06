using MailBox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBox.ViewModels
{
    class WriteMailViewModel:NotificationObject
    {
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

		public WriteMailViewModel(AccountInfo accountInfo)
		{
			AccountInfo = accountInfo;
		}
	}
}
