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

        [DllImport("pop3lib.dll", EntryPoint = "pull_a_mail", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringHandle pull_a_mail(LoginInfo info, UInt32 index);

        [DllImport("pop3lib.dll", EntryPoint = "free_mail_str")]
        public static extern void free_mail_str(IntPtr str_ptr);

        [DllImport("pop3lib.dll", EntryPoint = "get_simple_email")]
        public static extern ResultStruct get_simple_email(LoginInfo info, UInt32 index);

        [DllImport("pop3lib.dll", EntryPoint = "rustffi_get_version", CallingConvention = CallingConvention.Cdecl)]
        public static extern string rustffi_get_version(LoginInfo info, UInt32 index);

        [DllImport("pop3lib.dll", EntryPoint = "rustffi_get_version_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void rustffi_get_version_free(string s);

        [DllImport("pop3lib.dll", EntryPoint = "pull_save_mail")]
        public static extern Int32 pull_save_mail(LoginInfo info, UInt32 index);
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
                MailUtil.free_mail_str(handle);
            }

            return true;
        }
    }
    #endregion
}
