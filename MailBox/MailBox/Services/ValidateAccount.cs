using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MailBox.Services
{
    class ValidateAccount
    {
        [DllImport("smtplib.dll", EntryPoint = "validate_account")]
        private static extern Boolean validate_account_smtp(string account, string passwd, string site);

        // pop3 身份验证
        [DllImport("pop3lib.dll", EntryPoint = "validate_account")]
        private static extern Boolean validate_account_pop3(string account, string passwd, string site);

        public static string Validate(string account, string passwd, string popSite, string smtpSite)
        {
            try
            {
                //"alertdoll@163.com", "ybgissocute2020", "smtp.163.com:25","pop.163.com:110"
                if (!validate_account_smtp(account,passwd,smtpSite))
                {
                    return "smtp服务器地址错误";
                }

                if (!validate_account_pop3(account, passwd, popSite))
                {
                    return "pop3服务器地址错误";
                }
                return "验证成功";
            }
            catch (Exception)
            {
                return "验证失败";
            }

        }
    }
}
