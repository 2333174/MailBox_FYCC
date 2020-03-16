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

            MailStr mail_str = new MailStr(info_pop3, 3);
            Console.WriteLine(mail_str);

            //MailUtil.ResultStruct rs = MailUtil.get_simple_email(info_pop3, 3);
            //Console.WriteLine(Marshal.PtrToStringAnsi(rs.mail_string));

            //string temp = MailUtil.rustffi_get_version(info_pop3, 3);
            //string ver = (string)temp.Clone();
            //MailUtil.rustffi_get_version_free(temp);
            //Console.WriteLine(ver);

            Console.ReadKey();
        }
        #endregion

        public static void Main(string[] args)
        {
            //Validate_Example();

            //SendMail_Example();

            GetNumMails_Example();

            //ReceiveMail_Example();
        }

    }

}
