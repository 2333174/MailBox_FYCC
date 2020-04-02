using MailBee.Mime;
using MailBox.Commands;
using MailBox.Models;
using MailBox.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailBox.ViewModels
{
    class ReceiveMailViewModel : NotificationObject
    {
        public ReceiveMailViewModel(ObservableCollection<MailItem> items)
        {
            MailItems = items;
        }
        public ReceiveMailViewModel(AccountInfo account)
        {
            this.account = account;
            MailItems = GetMailItems(account);
            SaveAttachCommand = new DelegateCommand();
            SaveAttachCommand.ExecuteAction = new Action<object>(SaveAttach);
            DeleteMailCommand = new DelegateCommand();
            DeleteMailCommand.ExecuteAction = new Action<object>(DeleteMail);
        }
        public ReceiveMailViewModel() { }
        private AccountInfo account;
        private ObservableCollection<MailItem> mailItems;
        private Attachment selectedAttachment;
        private MailItem currentMail;
        private int selectedIndex;
        public DelegateCommand SaveAttachCommand { get; set; }
        public DelegateCommand DeleteMailCommand { get; set; }

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
        public Attachment SelectedAttachment
        {
            get
            {
                return selectedAttachment;
            }
            set
            {
                selectedAttachment = value;
                RaisePropertyChanged("SelectedAttachment");
            }
        }
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                RaisePropertyChanged("SelectedIndex");
            }
        }
        public MailItem CurrentMail
        {
            get
            {
                return currentMail;
            }
            set
            {
                currentMail = value;
                RaisePropertyChanged("CurrentMail");
            }
        }

        private void SaveAttach(object param)
        {
            Console.WriteLine("attachment select");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "选择附件保存的文件位置";
            if (SelectedAttachment != null)
            {
                saveFileDialog.FileName = SelectedAttachment.Filename; // init
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SelectedAttachment.SaveAsync(saveFileDialog.FileName, true);
                    Console.WriteLine("Attachment save succeed");
                }
                else
                {
                    Console.WriteLine("No element selected");
                }

            }
        }
        private void DeleteMail(object param)
        {
            if (CurrentMail == null)
                return;
            string filepath = CurrentMail.FilePath;
            MailUtil.LoginInfo info = new MailUtil.LoginInfo
            {
                account = account.Account,
                passwd = account.Password,
                site = account.PopHost
            };
            try
            {
                Regex regex = new Regex(@"\w+@\w+.com-(\d+).mail.tmp");
                string indexstr = regex.Match(filepath).Groups[1].Value;
                uint index = UInt32.Parse(indexstr);
                Console.WriteLine("Delete mail whose index = " + index);
                MailUtil.del_mail(info, index);
                // delete corresponding mail tmp file
                if (File.Exists(filepath))
                    File.Delete(filepath);

                // Flush binding item list
                MailItems = GetMailItems(account);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
