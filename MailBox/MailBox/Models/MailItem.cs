using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;

namespace MailBox.Models
{
    class MailItem
    { 
        public string Subject { get; set; }
        public string Sender { get; set; }
        public string SenderMailAddress { get; set; }
        public DateTime Date { get; set; }
        public string MailID { get; set; } // used to oepn specific eml file

        public string HtmlBody { get; set; } // extract from MimeMessage

        public MimePart MimePart { get; set; } // TODO: Attachment handle
        public string FilePath { get; set; }
        public char FirstSenderLetter { get; set; }

        public MailItem() { }
        public MailItem(string subject, string sender, DateTime date, string htmlBody, string mailID)
        {
            Subject = subject;
            Sender = sender;
            Date = date;
            HtmlBody = htmlBody;
            MailID = mailID;
        }

        // mode == 0 indicates read from file, mode == 1 read from mail string
        public MailItem(string file_path, int mode) 
        {
            MimeMessage message = null;
            if (mode == 0)
                message = MimeMessage.Load(new FileStream(file_path, FileMode.Open));
            else
                message = MimeMessage.Load(GenerateStreamFromString(file_path));

            Subject = message.Subject;
            SenderMailAddress = message.From != null? message.From.ToString() : "Unknown";
            SenderMailAddress = SenderMailAddress.Replace("\"", "");
            if (SenderMailAddress != "Unknown")
                Sender = SenderMailAddress.Substring(0, SenderMailAddress.IndexOf('<'));

            FirstSenderLetter = Sender.Length != 0 ? Sender.Substring(0, 1).ToLower()[0] : 'a';
            char f = FirstSenderLetter;
            if(f < 'a' || f > 'z')
            {
                // indicate chinese
                if(f < 'a')
                {
                    while(f < 'a')
                    {
                        f  = (char)((int)f + 26);
                    }
                }
                else
                {
                    while (f > 'z')
                    {
                        f = (char)((int)f - 26);
                    }
                }
                FirstSenderLetter = f;
            }
            Date = message.Date.DateTime;
            HtmlBody = message.HtmlBody?? message.TextBody;
            //MimePart = message.Attachments;
            FilePath = file_path;
        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
