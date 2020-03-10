using MailBox.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBox.ViewModels
{
    class ReceiveMailViewModel:NotificationObject
    {
        public ReceiveMailViewModel(AccountInfo account)
        {
            // TODO: Get info mails from account info 
            string directory = "D:\\File\\Projects\\Socket-SMTP-POP3-IMAP\\eml\\";
            string i1 = "Issue-163.eml";
            string i2 = "github_1-ms.eml";
            string i3 = "EML Test-to-163.eml";
            MailItems = new ObservableCollection<MailItem>();
            string[] email_paths = { "Issue-163.eml", "github_1-ms.eml", "EML Test-to-163.eml", "Multipart-163.eml" };
            foreach (string path in email_paths)
            {
                mailItems.Add(new MailItem(Path.Combine(directory, path), 0));
            }
        }

        public ReceiveMailViewModel() { }
        private ObservableCollection<MailItem> mailItems;
        public ObservableCollection<MailItem> MailItems
        {
            get
            {
                return mailItems;
            }
            set
            {
                mailItems = value;
                RaisePropertyChanged("MailItems");
            }
        }
    }
}
