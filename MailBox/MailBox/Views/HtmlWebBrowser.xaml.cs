using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailBox.Views
{
    /// <summary>
    /// HtmlMailView.xaml 的交互逻辑
    /// </summary>
    public partial class HtmlWebBrowser : UserControl
    {
        public HtmlWebBrowser()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty HtmlTextProperty = DependencyProperty.Register("HtmlText", typeof(string), typeof(HtmlWebBrowser));

        public string HtmlText
        {
            get { return (string)GetValue(HtmlTextProperty); }
            set { SetValue(HtmlTextProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == HtmlTextProperty)
            {
                DoBrowse();
            }
        }
        private void DoBrowse()
        {
            if (!string.IsNullOrEmpty(HtmlText))
            {
                mailBrowser.NavigateToString(HtmlText);
            }
        }

        private void mailBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //WebBrowser wb = (WebBrowser)sender;
            //string script = "document.documentElement.style.overflow ='hidden'";
            //wb.InvokeScript("execScript", new Object[] { script, "JavaScript" });
        }
    }
}
