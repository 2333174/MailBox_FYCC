﻿using MailBee.Mime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MailBox.Services
{
    public class SenderToLogoConverter : IValueConverter
    {
        // forward convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string uriStr = String.Format(@"/Resources/Logos/{0}.png", (char)value);
            return new Uri(uriStr, UriKind.Relative);
        }

        //backward convert
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HtmlBodyEncodingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string pattern = @"content\s*=\s*""[\w/\s]+;\s*charset\s*=\s*([\w\s-]*)""";
            Regex regex = new Regex(pattern);
            string htmlText = (string)value;
            MatchCollection mc = regex.Matches(htmlText);
            foreach (Match m in mc)
            {
                string encoding = m.Groups[1].Value;
                string replacement = m.Groups[0].Value.Replace(encoding, "utf-8");
                htmlText = regex.Replace(htmlText, replacement);
            }
            //string html_test = "<html><head><meta http-equiv=3D\"Content - Type\" content=\"text / html; charset = gbk\"></head></html>";

            // Mail content is plain text
            if (htmlText.Length != 0 && htmlText[0] != '<')
                htmlText = "<html><head><meta charset='UTF-8'></head><pre>" + htmlText + "</pre></html>";

            //File.WriteAllText(@"ShowTemp.html", htmlText);

            return htmlText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SelectToImageVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int param = (Int32)parameter; // 0 indicate background_image, 1 indicates mail content
            int selectedIndex = (int)value;
            if (selectedIndex != -1)
                return param == 0 ? "Hidden" : "Visible";
            else
                return param == 0 ? "Visible" : "Hidden";///Resources/bg_mail.jpg
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AttachmentsVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = ((List<Attachment>)value).Count;
            if (count > 0)
                return "Visible";
            else
                return "Hidden";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
