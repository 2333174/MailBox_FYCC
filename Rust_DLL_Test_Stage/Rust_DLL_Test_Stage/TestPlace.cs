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
        [StructLayout(LayoutKind.Sequential)]
        public struct LoginInfo
        {
            public string account;
            public string passwd;
            public string site;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MailInfo
        {
            public string from;
            public string to;
            public string cc;
            public string subject;
            public string body;
        }

        #region SMTP/POP3 身份验证
        // smtp 身份验证
        [DllImport("smtplib.dll", EntryPoint = "validate_account")]
        private static extern Boolean validate_account_smtp(LoginInfo info);

        // pop3 身份验证
        [DllImport("pop3lib.dll", EntryPoint = "validate_account")]
        private static extern Boolean validate_account_pop3(LoginInfo info);

        static void Validate_Example()
        {
            LoginInfo info_smtp = new LoginInfo() { account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25" };
            if (validate_account_smtp(info_smtp))
            {
                Console.WriteLine("Succ");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            LoginInfo info_pop3 = new LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };
            if (validate_account_pop3(info_pop3))
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
        [DllImport("smtplib.dll", EntryPoint = "login_send_mail")]
        private static extern Int32 login_send_mail(LoginInfo info, MailInfo email);
        static void SendMail_Example()
        {
            LoginInfo info_smtp = new LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "smtp.163.com:25"
            };

            MailInfo mail_info = new MailInfo()
            {
                from="alertdoll@163.com",
                to="ale_li_pona@163.com",
                cc="alertdoll@163.com",
                subject="test",
                body="Haha",
            };

            Int32 result = login_send_mail(info_smtp, mail_info);

            Console.WriteLine(result);

            Console.ReadKey();
        }
        #endregion

        public static void Main(string[] args)
        {
            //Validate_Example();

            SendMail_Example();
        }

    }

}
