using MailBee.Mime;
using MailBox.Commands;
using MailBox.Models;
using MailBox.Services;
using MailBox.Views;
using MaterialDesignThemes.Wpf;
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
        public ReceiveMailViewModel(AccountInfo account)
        {
            this.account = account;
            mailItems = GetMailItems(account); // backup all mails
            DisplayMailItems = mailItems;
            SaveAttachCommand = new DelegateCommand();
            SaveAttachCommand.ExecuteAction = new Action<object>(SaveAttach);
            DeleteMailCommand = new DelegateCommand();
            DeleteMailCommand.ExecuteAction = new Action<object>(ShowDeleteDialog);
        }
        public ReceiveMailViewModel() { }
        private AccountInfo account;
        private ObservableCollection<MailItem> mailItems;
        private ObservableCollection<MailItem> displayMailItems;

        private Attachment selectedAttachment;
        private MailItem currentMail;
        private int selectedIndex;
        private bool isSnackActive;
        private string tipMessage;
        public DelegateCommand SaveAttachCommand { get; set; }
        public DelegateCommand DeleteMailCommand { get; set; }

        private ObservableCollection<MailItem> GetMailItems(AccountInfo account)
        {
            ObservableCollection<MailItem> items = new ObservableCollection<MailItem>();

            // TODO: Get info mails from account info 
            string root_dir = Environment.CurrentDirectory; // temporary using /bin/Debug
            string user_dir = Path.Combine(root_dir, account.Account);
            if(!Directory.Exists(user_dir)) // when mail inbox is empty
            {
                Directory.CreateDirectory(user_dir);
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

        public ObservableCollection<MailItem> DisplayMailItems
        {
            get
            {
                return displayMailItems;
            }
            set
            {
                displayMailItems = value;
                RaisePropertyChanged("DisplayMailItems");
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
        public bool IsSnackActive
        {
            get
            {
                return isSnackActive;
            }
            set
            {
                isSnackActive = value;
                RaisePropertyChanged("IsSnackActive");
            }
        }
        public string TipMessage
        {
            get
            {
                return tipMessage;
            }
            set
            {
                tipMessage = value;
                RaisePropertyChanged("TipMessage");
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
        private async void ShowDeleteDialog(object param)
        {
            // show confirm dialog
            await DialogHost.Show(new ConfirmDeleteController(), new DialogClosingEventHandler(DeleteMail));
            Console.WriteLine("Dialog end");
        }
        private async void DeleteMail(object param, DialogClosingEventArgs args)
        {
            if (!((bool)args.Parameter) || CurrentMail == null)
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

                // index reduce 1 if mail's original index greater than deleted one's
                string dir = Path.Combine(Directory.GetCurrentDirectory(), account.Account);
                foreach (string f in Directory.GetFiles(dir))
                {
                    Group g = regex.Match(f).Groups[1];
                    uint i = UInt32.Parse(g.Value);
                    if (i > index)
                    {
                        string newFile = String.Concat(f.Substring(0, g.Index), i - 1, f.Substring(g.Index + g.Length));
                        File.Move(Path.Combine(dir, f), Path.Combine(dir, newFile));
                    }
                }
                // Flush binding item list
                DisplayMailItems = GetMailItems(account);

                // show snackbar
                TipMessage = "删除成功";
                IsSnackActive = true;
                await Task.Delay(3000);
                IsSnackActive = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // show snackbar
                TipMessage = "删除失败";
                IsSnackActive = true;
                await Task.Delay(3000);
                IsSnackActive = false;
            }
        }

        public void SearchMail(string searchKey)
        {
            if (String.IsNullOrEmpty(searchKey))
                DisplayMailItems = mailItems;
            else
            {
                // search all elements which contains key
                ObservableCollection<MailItem> items = new ObservableCollection<MailItem>();
                foreach(MailItem item in mailItems)
                {
                    if(item.Sender.Contains(searchKey) || item.Subject.Contains(searchKey) || item.Date.ToString("yyyy-MM-dd HH:mm").Contains(searchKey))
                        items.Add(item);
                }
                DisplayMailItems = items;
            }
        }
    }
}
