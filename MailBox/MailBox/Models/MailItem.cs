using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using EAGetMail;
using mk = MimeKit;
using MailBee;
using MailBee.Mime;

namespace MailBox.Models
{
    class MailItem
    {
        public string Subject { get; set; }
        public string SenderMailAddress { get; set; }
        public string Sender { get; set; }
        public char FirstSenderLetter { get; set; }
        public DateTime Date { get; set; }
        public string HtmlBody { get; set; } 
        public List<Attachment> Attachments { get; set; }
        public string FilePath { get; set; }

        public MailItem() { }

        public MailItem(string file_path)
        {

            MailMessage message = new MailMessage();
            message.LoadMessage(file_path);
            Subject = message.Subject;
            SenderMailAddress = message.From != null || message.From.DisplayName.Length == 0? message.From.ToString() : "Unknown";
            SenderMailAddress = SenderMailAddress.Replace("\"", "");
            if (SenderMailAddress != "Unknown")
            {
                int p = SenderMailAddress.IndexOf('<');
                if (p > 0)
                    Sender = SenderMailAddress.Substring(0, SenderMailAddress.IndexOf('<'));
                else
                    Sender = SenderMailAddress.Substring(1, SenderMailAddress.IndexOf('@') - 1);
            }

            FirstSenderLetter = Sender.Length != 0 ? Sender.Substring(0, 1).ToLower()[0] : 'a';
            char f = FirstSenderLetter;
            if (f < 'a' || f > 'z')
            {
                // indicate chinese
                if (f < 'a')
                {
                    while (f < 'a')
                    {
                        f = (char)((int)f + 26);
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
            Date = message.Date;
            HtmlBody = message.BodyHtmlText == null || message.BodyHtmlText == "" ? message.BodyPlainText : message.BodyHtmlText;
            if (HtmlBody.Length == 0) HtmlBody = " ";
            AttachmentCollection attachments = message.Attachments;
            Attachments = new List<Attachment>();
            foreach (Attachment att in attachments)
                Attachments.Add(att);

            FilePath = file_path;
        }
    }
}
