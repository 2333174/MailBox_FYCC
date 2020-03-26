﻿using MailBox.Models;
using MailBox.Services;
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
                try
                {
                    Int32 num_mails = MailUtil.get_num_mails(loginInfo);
                    if (num_mails == -1) return items;
                    for (uint i = 1; i <= num_mails; i++)
                    {
                        MailUtil.pull_save_mail(loginInfo, i);
                        Console.WriteLine("rec one mail");
                    }
                }
                catch (Exception e)
                {
                    Console.Write("Get Mail list failed, error:" + e.Message);
                    return items;
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
