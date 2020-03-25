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

namespace Rust_DLL_Test_Stage
{
    class TestPlace
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
                = new MailUtil.LoginInfo() { account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25" };
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
                from="alertdoll@163.com",
                to="ale_li_pona@163.com",
                cc="alertdoll@163.com",
                subject="test",
                body="Haha",
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
        public static void Main(string[] args)
        {
            //Validate_Example();

            //SendMail_Example();

            //GetNumMails_Example();

            //ReceiveMail_Example();
            try
            {

                Receive_All_Example();
            }
            catch (Exception e)
            {
                Console.WriteLine("fail");
            }

            //SendMail_Example_Extern();
        }

    }
}
