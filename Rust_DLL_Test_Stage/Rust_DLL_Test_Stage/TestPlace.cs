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
        [DllImport("smtplib.dll", EntryPoint = "validate_account")]
        private static extern Boolean validate_account_smtp(string account, string passwd, string site);

        public static void Main(string[] args)
        {
            if(validate_account_smtp("ale_li_pona@163.com","ybg19970203ybg","smtp.163.com:25"))
            {
                Console.WriteLine("Succ");
            }
            else
            {
                Console.WriteLine("Fail");
            }

            Console.ReadKey();
        }

    }

}
