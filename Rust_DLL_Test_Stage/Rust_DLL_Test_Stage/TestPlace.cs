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
    #region DEPENDENCE
    public class MailUtil
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

        [StructLayout(LayoutKind.Sequential)]
        public struct ResultStruct
        {
            public Int32 status;
            public IntPtr mail_string;
        }

        // smtp 身份验证
        [DllImport("smtplib.dll", EntryPoint = "validate_account")]
        public static extern Boolean validate_account_smtp(LoginInfo info);

        // pop3 身份验证
        [DllImport("pop3lib.dll", EntryPoint = "validate_account")]
        public static extern Boolean validate_account_pop3(LoginInfo info);

        [DllImport("smtplib.dll", EntryPoint = "login_send_mail")]
        public static extern Int32 login_send_mail(LoginInfo info, MailInfo email);

        [DllImport("pop3lib.dll", EntryPoint = "pull_a_mail")]
        public static extern StringHandle pull_a_mail(LoginInfo info, UInt32 index);

        [DllImport("pop3lib.dll", EntryPoint = "free_string")]
        public static extern void free_string(IntPtr str_ptr);

        [DllImport("pop3lib.dll", EntryPoint = "get_simple_email")]
        public static extern ResultStruct get_simple_email(LoginInfo info, UInt32 index);
    }
    public class MailStr : IDisposable
    {
        private StringHandle mailStrHandle;
        private string mailString;

        public MailStr(MailUtil.LoginInfo login_info, UInt32 index)
        {
            mailStrHandle = MailUtil.pull_a_mail(login_info, index);
        }

        public override string ToString()
        {
            if (mailString == null)
            {
                mailString = mailStrHandle.AsString();
            }
            return mailString;
        }

        public void Dispose()
        {
            mailStrHandle.Dispose();
        }
    }

    public class StringHandle : SafeHandle
    {
        public StringHandle() : base(IntPtr.Zero, true) { }

        public override bool IsInvalid
        {
            get { return this.handle == IntPtr.Zero; }
        }

        public string AsString()
        {
            int len = 0;
            while (Marshal.ReadByte(handle, len) != 0) { ++len; }
            byte[] buffer = new byte[len];
            Marshal.Copy(handle, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        protected override bool ReleaseHandle()
        {
            if (!this.IsInvalid)
            {
                MailUtil.free_string(handle);
            }

            return true;
        }
    }
    #endregion

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

        #region POP3 receive mail

        static void ReceiveMail_Example()
        {
            MailUtil.LoginInfo info_pop3 = new MailUtil.LoginInfo()
            {
                account = "alertdoll@163.com",
                passwd = "ybgissocute2020",
                site = "pop.163.com:110"
            };

            //MailStr mail_str = new MailStr(info_pop3, 3);

            //Console.WriteLine(mail_str);
            MailUtil.ResultStruct rs = MailUtil.get_simple_email(info_pop3, 3);

            Console.WriteLine(Marshal.PtrToStringAnsi(rs.mail_string));

            Console.ReadKey();
        }
        #endregion

        public static void Main(string[] args)
        {
            Validate_Example();

            SendMail_Example();

            //ReceiveMail_Example();
        }

    }

}
