using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        [DllImport("smtplib.dll", EntryPoint = "login_send_mail_extern")]
        public static extern Int32 login_send_mail_extern(LoginInfo info, MailInfo email);

        [DllImport("pop3lib.dll", EntryPoint = "get_num_mails")]
        public static extern Int32 get_num_mails(LoginInfo info);

        [DllImport("pop3lib.dll", EntryPoint = "pull_save_mail")]
        public static extern Int32 pull_save_mail(LoginInfo info, UInt32 index);

        [DllImport("pop3lib.dll", EntryPoint = "del_mail")]
        public static extern Int32 del_mail(LoginInfo info, UInt32 index);
    }
    #endregion
}
