using MailBox.Models;
using MailBox.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailBox.ViewModels
{
    class ReceiveMailViewModel:NotificationObject
    {
        public ReceiveMailViewModel(ObservableCollection<MailItem> items)
        {
            MailItems = items;
        }
        public ReceiveMailViewModel(AccountInfo account)
        {
            MailItems = GetMailItems(account);
        }
        // param flush: flush user mail directory
        private ObservableCollection<MailItem> GetMailItems(AccountInfo account)
        {
            ObservableCollection<MailItem> items = new ObservableCollection<MailItem>();

            // TODO: Get info mails from account info 
            string root_dir = Environment.CurrentDirectory; // temporary using /bin/Debug
            string user_dir = Path.Combine(root_dir, account.Account);
            string[] mailFiles = Directory.GetFiles(user_dir);
            if (mailFiles.Length == 0)
            {
                // TODO handle
                Console.WriteLine("Mail box is empty");
            }
            else
            {               
                //string[] email_paths = { "Issue-163.eml", "github_1-ms.eml", "EML Test-to-163.eml", "Multipart-163.eml" };
                foreach (string m in mailFiles)
                {
                    if (!m.EndsWith(".tmp")) continue;
                    string ab_path = Path.Combine(user_dir, m);
                    if (File.Exists(ab_path))
                        //items.Add(new MailItem(ab_path, 0));
                        items.Add(new MailItem(ab_path));

                }
            }
            return items;
        }
        async Task WaitAsync(Task task, TimeSpan timeout)
        {
            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {
                var delayTask = Task.Delay(timeout, timeoutCancellationTokenSource.Token);
                if (await Task.WhenAny(task, delayTask) == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    await task;
                }
                else
                    throw new TimeoutException("The operation has timed out.");
                //Console.WriteLine("timeout happened.");
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
