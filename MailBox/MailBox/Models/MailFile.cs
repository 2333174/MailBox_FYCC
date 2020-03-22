using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBox.Models
{
    public class MailFile
    {
        /// <summary>
        /// 邮件附件文件名称
        /// </summary>
        public string MailFileName{ get; set; }

        /// <summary>
        /// 邮件附件文件路径  例如：图片 MailFilePath=@"C:\Files\123.png"
        /// </summary>
        public string MailFilePath { get; set; }

        public MailFile(string name, string path)
        {
            MailFileName = name;
            MailFilePath = path;
        }
    }
}
