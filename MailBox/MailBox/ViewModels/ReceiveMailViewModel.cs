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
        public ReceiveMailViewModel(AccountInfo account, bool flush)
        {
            MailItems = GetMailItems(account, flush);
        }
        // param flush: flush user mail directory
        private ObservableCollection<MailItem> GetMailItems(AccountInfo account, bool flush)
        {
            ObservableCollection<MailItem> items = new ObservableCollection<MailItem>();

            // TODO: Get info mails from account info 
            string root_dir = Environment.CurrentDirectory; // temporary using /bin/Debug
            string user_dir = Path.Combine(root_dir, account.Account);

            flush = false; // Temporary pass-------------------
            if (!Directory.Exists(user_dir) || flush)
            {
                if (Directory.Exists(user_dir))
                    Directory.Delete(user_dir, true);

                // start POP3 and receive mail
                MailUtil.LoginInfo loginInfo = new MailUtil.LoginInfo
                {
                    account = account.Account,
                    passwd = account.Password,
                    site = account.PopHost
                };
                try // TODO move to outside the function
                {
                    int num = MailUtil.get_num_mails(loginInfo);
                    if (num == -1) return items;
                    //info_pop3.account = "11";
                    Task[] tasks = new Task[num];
                    for (uint i = 1; i <= num; i++)
                    {
                        uint param = i;
                        var tokenSource = new CancellationTokenSource();
                        var token = tokenSource.Token;

                        tasks[i - 1] = WaitAsync(Task.Factory.StartNew(() =>
                        {
                            int r = MailUtil.pull_save_mail(loginInfo, param);
                            if (r != -1)
                                Console.WriteLine("Receive mail-{0} success", param);
                            else
                                Console.WriteLine("Receive mail-{0} fail", param);
                        }), TimeSpan.FromSeconds(4.0)); // run time up to 3 second

                        //tasks[i - 1] = Task.Factory.StartNew(() =>
                        //{
                        //    int r = MailUtil.pull_save_mail(loginInfo, param);
                        //    if (r != -1)
                        //        Console.WriteLine("Receive mail-{0} success", param);
                        //    else
                        //        Console.WriteLine("Receive mail-{0} fail", param);
                        //});

                    }
                    Task.WaitAll(tasks, TimeSpan.FromSeconds(4.0)); // wait for 4 seconds
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
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
