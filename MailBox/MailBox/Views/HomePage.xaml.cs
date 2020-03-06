using MailBox.Models;
using MailBox.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage(ObservableCollection<AccountInfo> accountInfos, int selectIndex)
        {
            InitializeComponent();
            this.DataContext = new HomeViewModel(accountInfos, selectIndex);
        }

        private void CloseMenu(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }

        private void testClick(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(123);
        }

        private void newEmailClick(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
            var vm = this.DataContext as HomeViewModel;
            vm.NewMailCommand.Execute(null);
        }

        private void ChangeAccount(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
            var vm = this.DataContext as HomeViewModel;
            vm.ReceiveMailCommand.Execute(null);
        }
    }
}
