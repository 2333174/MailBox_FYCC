using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// loginController.xaml 的交互逻辑
    /// </summary>
    public partial class LoginController : UserControl
    {
        public LoginController()
        {
            InitializeComponent();
        }

        private void AccountEnter(object sender, RoutedEventArgs e)
        {
            if (this.NameTextBox.Text.Split('@').Length <= 1) return;
            string suffix = this.NameTextBox.Text.Split('@')[1];
            this.PopHostTextBox.Text = "pop." + suffix + ":110";
            this.SmtpHostTextBox.Text = "smtp." + suffix + ":25";
        }
    }
}
