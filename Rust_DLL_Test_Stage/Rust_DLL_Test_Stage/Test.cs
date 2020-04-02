using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using MailBox.Models;
using System.Collections.ObjectModel;

namespace Rust_DLL_Test_Stage
{
    class Test
    {
        //static String mailstr = "Content-Type: text/plain; charset=\"utf-8\"\r\nMIME-Version: 1.0\r\nContent-Transfer-Encoding: base64\r\n" +
        //    "From: alertdoll@163.com\r\nTo: 935802216@qq.com\r\nSubject: =?utf-8?b?5ZOI5ZOI?=\r\n\r\n" +
        //    "54ix5LiK55yL5Yiw5ZKv5oiR\r\n\r\n";
        static String mailstr = "Content-Type: text/plain; charset=\"utf-8\"\r\nMIME-Version: 1.0\r\nContent-Transfer-Encoding: base64\r\n" +
            "From: alertdoll@163.com\r\nTo: 935802216@qq.com\r\nSubject: =?utf-8?b?5ZOI5ZOI?=\r\n\r\nMTIz\r\n\r\n";

        #region SMTP/POP3 身份验证
        static void Validate_Example()
        {
            MailUtil.LoginInfo info_smtp
                = new MailUtil.LoginInfo()
                {
                    account = "alertdoll@163.com",
                    passwd = "ybgissocute2020",
                    site = "smtp.163.com:25"
                };
            if (MailUtil.validate_account_smtp(info_smtp))
            {
                Console.WriteLine("Succ");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };
            if (MailUtil.validate_account_pop3(info_pop3))
            {
                Console.WriteLine("Succ");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            Console.ReadKey();
        }
        #endregion

        #region SMTP send mail
        static void SendMail_Example()
        {
            MailUtil.LoginInfo info_smtp = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25"
            };

            MailUtil.MailInfo mail_info = new MailUtil.MailInfo()
            {
                from = "alertdoll@163.com",
                to = "ale_li_pona@163.com",
                cc = "alertdoll@163.com",
                subject = "test",
                body = "Haha",
            };

            Int32 result = MailUtil.login_send_mail(info_smtp, mail_info);

            Console.WriteLine(result);

            Console.ReadKey();
        }

        static void SendMail_Example_Extern()
        {
            string text = System.IO.File.ReadAllText(@"D:\project\网络工程与编程实践\1.txt");

            MailUtil.LoginInfo info_smtp = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25"
            };

            MailUtil.MailInfo mail_info = new MailUtil.MailInfo()
            {
                from = "alertdoll@163.com",
                to = "ale_li_pona@163.com",
                cc = "alertdoll@163.com",
                subject = "test",
                body = text,
            };

            Int32 result = MailUtil.login_send_mail_extern(info_smtp, mail_info);

            Console.WriteLine(result);

            Console.ReadKey();
        }
        #endregion

        #region POP3 get the number of mails
        static void GetNumMails_Example()
        {
            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };

            Int32 num = MailUtil.get_num_mails(info_pop3);
            Console.WriteLine(num);

            Console.ReadKey();
        }
        #endregion

        #region POP3 receive mail
        static void ReceiveMail_Example()
        {
            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };

            MailUtil.pull_save_mail(info_pop3, 5);

            Console.ReadKey();
        }
        #endregion

        #region POP3 receive mail
        static void Receive_All_Example()
        {
            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };
            try
            {

                int num = MailUtil.get_num_mails(info_pop3);
                for (uint i = 1; i <= num; i++)
                {

                    MailUtil.pull_save_mail(info_pop3, i);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("fail, error:" + e.Message);
            }

            Console.ReadKey();
        }
        #endregion

        public static void Main1(string[] args)
        {
            try
            {
                //Console.WriteLine("iiqweqwe, my thread ID is {0}", Thread.CurrentThread.ManagedThreadId);
                //var re  = AsyncMethod();
                //Console.WriteLine("1324564766346, my thread ID is {0}", Thread.CurrentThread.ManagedThreadId);
                //Console.WriteLine("Async result = {0}", re.Result);
                //runASyncTest();
                //ObservableCollection<MailItem> items =  uTask().Result;
                ObservableCollection<MailItem> items = Get_Mail();

                //Receive_All_Example();
            }
            catch (Exception e)
            {
                Console.WriteLine("fail, error: " + e.Message);
            }

            Console.ReadKey();
        }
        static async Task< ObservableCollection<MailItem> > uTask() {
            return await Task.Run(() =>
            {
                Save_Mail();
                return Get_Mail();
            });
        }
        static ObservableCollection<MailItem> Get_Mail()
        {
            ObservableCollection<MailItem> items = new ObservableCollection<MailItem>();
            string root_dir = Environment.CurrentDirectory; // temporary using /bin/Debug
            string user_dir = Path.Combine(root_dir, "alertdoll@163.com");
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
                        //items.Add(new MailItem(ab_path));
                        items.Add(new MailItem(ab_path, true));

                }
            }
            return items;
        }
        static void Save_Mail()
        {
            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };

            int num = MailUtil.get_num_mails(info_pop3);
            //info_pop3.account = "11";
            Task[] tasks = new Task[num];
            for (uint i = 1; i <= num; i++)
            {
                uint param = i;
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                tasks[i - 1] = WaitAsync(Task.Factory.StartNew(() =>
                {
                    int r = MailUtil.pull_save_mail(info_pop3, param);
                    if (r != -1)
                        Console.WriteLine("Receive mail-{0} success", param);
                    else
                        Console.WriteLine("Receive mail-{0} fail", param);
                }), TimeSpan.FromSeconds(3.0));


            }
            Task.WaitAll(tasks, TimeSpan.FromSeconds(4.0)); // wait for 10 seconds
            Console.WriteLine("tasks all completed");
        }

        static async Task WaitAsync(Task task, TimeSpan timeout)
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
                    //throw new TimeoutException("The operation has timed out.");
                    Console.WriteLine("timeout happened.");
            }
        }

        //public static async Task<TResult> WaitAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
        //{
        //    using (var timeoutCancellationTokenSource = new CancellationTokenSource())
        //    {
        //        var delayTask = Task.Delay(timeout, timeoutCancellationTokenSource.Token);
        //        if (await Task.WhenAny(task, delayTask) == task)
        //        {
        //            timeoutCancellationTokenSource.Cancel();
        //            return await task;
        //        }
        //        throw new TimeoutException("The operation has timed out.");
        //    }
        //}
        static async Task<string> WaitAsynchronouslyAsync()
        {
            await Task.Delay(3000);
            Console.WriteLine("123123");
            return "Finished";
        }

        // The following method runs synchronously, despite the use of async.
        // You cannot move or resize the Form1 window while Thread.Sleep
        // is running because the UI thread is blocked.
        static async Task<string> WaitSynchronously()
        {
            // Add a using directive for System.Threading.
            Thread.Sleep(3000);
            Console.WriteLine("hahaha");
            return "Finished";
        }
        static async void runASyncTest()
        {
            // Call the method that runs asynchronously.
            //string result = await WaitAsynchronouslyAsync();
            Console.WriteLine("next step");

            //Call the method that runs synchronously.
            string result = await WaitSynchronously();

            Console.WriteLine("\n\n\n\n Result = {0}", result);
        }

        private static async Task<string> AsyncMethod()
        {
            var ResultFromTimeConsumingMethod = TimeConsumingMethod();
            Console.WriteLine("huaqwel, current id is " + Thread.CurrentThread.ManagedThreadId);
            string Result = await ResultFromTimeConsumingMethod + " + AsyncMethod. My Thread ID is :" + Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine(Result + "  current ID: " + Thread.CurrentThread.ManagedThreadId);
            //返回值是Task的函数可以不用return

            return Result;
        }

        //这个函数就是一个耗时函数，可能是IO操作，也可能是cpu密集型工作。
        private static Task<string> TimeConsumingMethod()
        {
            var task = Task.Run(() =>
            {
                Console.WriteLine("Helo I am TimeConsumingMethod. My Thread ID is :" + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(5000);
                Console.WriteLine("Helo I am TimeConsumingMethod after Sleep(5000). My Thread ID is :" + Thread.CurrentThread.ManagedThreadId);
                return "Hello I am TimeConsumingMethod";
            });

            return task;
        }

       

    }
}
