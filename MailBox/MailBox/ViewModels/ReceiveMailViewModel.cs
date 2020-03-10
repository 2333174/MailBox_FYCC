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
            string directory = Environment.CurrentDirectory.Replace("\\bin\\Debug", "\\Resources\\test_eml");
            MailItems = new ObservableCollection<MailItem>();
            string[] email_paths = { "Issue-163.eml", "github_1-ms.eml", "EML Test-to-163.eml", "Multipart-163.eml" };
            foreach (string path in email_paths)
            {
                string ab_path = Path.Combine(directory, path);
                if(File.Exists(ab_path))
                    mailItems.Add(new MailItem(ab_path, 0));
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
