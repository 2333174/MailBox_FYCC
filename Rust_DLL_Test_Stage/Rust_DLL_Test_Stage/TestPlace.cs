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
        // smtp 身份验证
        [DllImport("smtplib.dll", EntryPoint = "validate_account")]
        private static extern Boolean validate_account_smtp(string account, string passwd, string site);

        // pop3 身份验证
        [DllImport("pop3lib.dll", EntryPoint = "validate_account")]
        private static extern Boolean validate_account_pop3(string account, string passwd, string site);

        static void Validate_Example()
        {
            if (validate_account_smtp("alertdoll@163.com", "ybgissocute2020", "smtp.163.com:25"))
            {
                Console.WriteLine("Succ");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            if (validate_account_pop3("alertdoll@163.com", "ybgissocute2020", "pop.163.com:110"))
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

        public static void Main(string[] args)
        {
            Validate_Example();
        }

    }

}
