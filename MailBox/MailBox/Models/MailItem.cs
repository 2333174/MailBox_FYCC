using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using EAGetMail;
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
            //if (mode == 0)
            //    message = MimeMessage.Load(new FileStream(file_path, FileMode.Open));
            //else
            //    message = MimeMessage.Load(GenerateStreamFromString(file_path));

            MimeMessage m2 = new MimeMessage();
            MailMessage m3 = new MailMessage();
            Mail mm = new Mail("TryIt");
            mm.Load(File.ReadAllBytes(file_path));


            Subject = message.Subject;
            SenderMailAddress = message.From != null? message.From.ToString() : "Unknown";
            SenderMailAddress = SenderMailAddress.Replace("\"", "");
            if (SenderMailAddress != "Unknown")
            {
                int p = SenderMailAddress.IndexOf('<');
                if (p > 0)
                    Sender = SenderMailAddress.Substring(0, SenderMailAddress.IndexOf('<'));
                else
                    Sender = SenderMailAddress.Substring(0, SenderMailAddress.IndexOf('@'));
            }

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
            var attachments = message.Attachments.ToList();
            FilePath = file_path;
          
        }

        public MailItem(string file_path)
        {
            // using EGAMail which can parse attachments from eml files
            Mail m = new Mail("TryIt");
            m.Load(File.ReadAllBytes(file_path));


            Subject = m.Subject.Replace(" (Trial Version)","");
            SenderMailAddress = m.From != null ? m.From.ToString() : "Unknown";
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
            Date = m.ReceivedDate;
            HtmlBody = m.HtmlBody ?? m.TextBody;
            //MimePart = message.Attachments;
            var attachments = m.Attachments.ToList();
            //EAGetMail.Attachment a = new EAGetMail.Attachment();
            //a.SaveAs()
            foreach(var att in attachments)
            {
                att.SaveAs(att.Name, true);
            }

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
